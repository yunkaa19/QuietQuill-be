using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Users.Commands.RegisterUser;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;
using Application.Abstraction.Authentication;

namespace ArchitectureTests.HandlerTests.Users
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly Mock<IValidator<RegisterUserCommand>> _validatorMock;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _validatorMock = new Mock<IValidator<RegisterUserCommand>>();
            _handler = new RegisterUserCommandHandler(_unitOfWorkMock.Object, _userRepositoryMock.Object, _authenticationServiceMock.Object, _validatorMock.Object);
        }

        private RegisterUserCommand CreateValidCommand()
        {
            return new RegisterUserCommand

                ("user@example.com",
                    "Password123!")
            ;
        }

        [Fact]
        public async Task Handle_Should_RegisterUser_When_ValidationPasses_And_UserDoesNotExist()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationResult = new ValidationResult();
            var user = new User(Guid.NewGuid(), "null", command.Email, "username", null);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _userRepositoryMock.Setup(r => r.UserExists(command.Email)).ReturnsAsync(false);
            _authenticationServiceMock.Setup(a => a.RegisterAsync(command.Email, command.Password)).ReturnsAsync("GeneratedIdentityID");
            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var expectedJson = JsonSerializer.Serialize(new { identityID = "GeneratedIdentityID" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedJson, result);

            _validatorMock.Verify(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(r => r.UserExists(command.Email), Times.Once);
            _authenticationServiceMock.Verify(a => a.RegisterAsync(command.Email, command.Password), Times.Once);
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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

            _userRepositoryMock.Verify(r => r.UserExists(It.IsAny<string>()), Times.Never);
            _authenticationServiceMock.Verify(a => a.RegisterAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_UserAlreadyExists()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationResult = new ValidationResult();
            var user = new User(Guid.NewGuid(), "null", command.Email, "username", null);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _userRepositoryMock.Setup(r => r.UserExists(command.Email)).ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal("User already exists.", exception.Message);

            _validatorMock.Verify(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
            _userRepositoryMock.Verify(r => r.UserExists(command.Email), Times.Once);
            _authenticationServiceMock.Verify(a => a.RegisterAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
