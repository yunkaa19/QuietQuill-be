using System.Text.Json;
using Application.Users.Commands.LoginUser;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Exceptions.Users;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Application.Abstraction.Authentication;

namespace ArchitectureTests.HandlerTests.Users
{
    public class LoginUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IValidator<LoginUserCommand>> _validatorMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJWTProvider> _jwtProviderMock;
        private readonly LoginUserCommandHandler _handler;

        public LoginUserCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<LoginUserCommand>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtProviderMock = new Mock<IJWTProvider>();
            _handler = new LoginUserCommandHandler(_jwtProviderMock.Object, _unitOfWorkMock.Object, _validatorMock.Object, _userRepositoryMock.Object);
        }

        private LoginUserCommand CreateValidCommand()
        {
            return  new LoginUserCommand(

                    "user@example.com", 
                 "Password123!");
        }

        [Fact]
        public async Task Handle_Should_ReturnJsonWithTokenAndUserDetails_When_ValidationPasses_And_UserExists()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationResult = new ValidationResult();
            var user = new User(Guid.NewGuid(), command.Email, "username", "hashedPassword", "UserRole");

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _userRepositoryMock.Setup(r => r.GetUserByEmail(command.Email)).ReturnsAsync(user);
            _jwtProviderMock.Setup(j => j.GetForCredentialsAsync(command.Email, command.Password)).ReturnsAsync("GeneratedJWTToken");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var expectedJson = JsonSerializer.Serialize(new
            {
                Token = "GeneratedJWTToken",
                user.UserId,
                user.Email,
                user.Role
            });
            Assert.Equal(expectedJson, result);

            _validatorMock.Verify(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(r => r.GetUserByEmail(command.Email), Times.Once);
            _jwtProviderMock.Verify(j => j.GetForCredentialsAsync(command.Email, command.Password), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowValidationException_When_ValidationFails()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Email", "Email is required") };
            var validationResult = new ValidationResult(validationFailures);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal(validationResult.Errors, exception.Errors);

            _userRepositoryMock.Verify(r => r.GetUserByEmail(It.IsAny<string>()), Times.Never);
            _jwtProviderMock.Verify(j => j.GetForCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowUserNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationResult = new ValidationResult();

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _userRepositoryMock.Setup(r => r.GetUserByEmail(command.Email)).ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Contains("The user with the identifier", exception.Message);
            Assert.Contains("was not found.", exception.Message);

            _userRepositoryMock.Verify(r => r.GetUserByEmail(command.Email), Times.Once);
            _jwtProviderMock.Verify(j => j.GetForCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

    }
}
