using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Users.Commands.ChangePassword;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Application.Abstraction.Authentication;
using Xunit;

namespace ArchitectureTests.HandlerTests.Users
{
    public class ChangePasswordCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly Mock<IValidator<ChangePasswordCommand>> _validatorMock;
        private readonly ChangePasswordCommandHandler _handler;

        public ChangePasswordCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _validatorMock = new Mock<IValidator<ChangePasswordCommand>>();
            _handler = new ChangePasswordCommandHandler(_unitOfWorkMock.Object, _userRepositoryMock.Object, _authenticationServiceMock.Object, _validatorMock.Object);
        }

        private ChangePasswordCommand CreateValidCommand()
        {
            return new ChangePasswordCommand(
                "user@example.com",
                "OldPassword123!",
                "NewPassword123!");
        }

        [Fact]
        public async Task Handle_Should_ChangePassword_When_ValidationPasses_And_UserExists()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationResult = new ValidationResult();
            var user = new User(Guid.NewGuid(), "username", "hashedPassword",command.Email, "User");

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _userRepositoryMock.Setup(r => r.GetUserByEmail(command.Email)).ReturnsAsync(user);
            _authenticationServiceMock.Setup(a => a.ChangePasswordAsync(command.Email, command.oldPassword, command.newPassword)).ReturnsAsync("IdentityID");
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("IdentityID", result);

            _userRepositoryMock.Verify(r => r.GetUserByEmail(command.Email), Times.Once);
            _authenticationServiceMock.Verify(a => a.ChangePasswordAsync(command.Email, command.oldPassword, command.newPassword), Times.Once);
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

            _userRepositoryMock.Verify(r => r.GetUserByEmail(It.IsAny<string>()), Times.Never);
            _authenticationServiceMock.Verify(a => a.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_UserNotFound()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationResult = new ValidationResult();

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _userRepositoryMock.Setup(r => r.GetUserByEmail(command.Email)).ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal("User not found.", exception.Message);

            _userRepositoryMock.Verify(r => r.GetUserByEmail(command.Email), Times.Once);
            _authenticationServiceMock.Verify(a => a.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
