using Application.Meditation.Commands.UpdateMeditation;
using Application.Meditation.DTOs;
using Domain.Abstraction;
using Domain.Entities.Meditation;
using Domain.Exceptions.Meditation;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace ArchitectureTests.HandlerTests.Meditation
{
    public class UpdateMeditationCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IValidator<UpdateMeditationCommand>> _validatorMock;
        private readonly Mock<IMeditationSessionRepository> _meditationSessionRepositoryMock;
        private readonly UpdateMeditationCommandHandler _handler;

        public UpdateMeditationCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<UpdateMeditationCommand>>();
            _meditationSessionRepositoryMock = new Mock<IMeditationSessionRepository>();
            _handler = new UpdateMeditationCommandHandler(_unitOfWorkMock.Object, _validatorMock.Object, _meditationSessionRepositoryMock.Object);
        }

        private UpdateMeditationCommand CreateValidCommand()
        {
            return new UpdateMeditationCommand
            (
                new MeditationSessionDTO
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Updated Meditation Name",
                    Duration = new TimeSpan(0, 30, 0), // 30 minutes
                    Description = "Updated Meditation Description",
                    Type = MeditationType.Guided
                }
            );
        }

        [Fact]
        public async Task Handle_Should_UpdateMeditationSession_When_ValidationPasses_And_SessionExists()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationResult = new ValidationResult();
            var existingSession = new MeditationSession("Existing Meditation Name", new TimeSpan(0, 20, 0), "Existing Description", MeditationType.Guided);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _meditationSessionRepositoryMock.Setup(r => r.GetMeditationSessionById(command.Session.Id)).Returns(existingSession);
            _meditationSessionRepositoryMock.Setup(r => r.UpdateMeditationSession(existingSession)).Verifiable();
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Session.Id, result.Id);
            Assert.Equal(command.Session.Title, result.Title);
            Assert.Equal(command.Session.Duration, result.Duration);
            Assert.Equal(command.Session.Description, result.Description);
            Assert.Equal(command.Session.Type, result.Type);

            _meditationSessionRepositoryMock.Verify(r => r.UpdateMeditationSession(existingSession), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowValidationException_When_ValidationFails()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Title", "Title is required") };
            var validationResult = new ValidationResult(validationFailures);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal(validationResult.Errors, exception.Errors);

            _meditationSessionRepositoryMock.Verify(r => r.GetMeditationSessionById(It.IsAny<string>()), Times.Never);
            _meditationSessionRepositoryMock.Verify(r => r.UpdateMeditationSession(It.IsAny<MeditationSession>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowMeditationNotFoundException_When_SessionDoesNotExist()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationResult = new ValidationResult();

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _meditationSessionRepositoryMock.Setup(r => r.GetMeditationSessionById(command.Session.Id)).Returns((MeditationSession)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<MeditationNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _meditationSessionRepositoryMock.Verify(r => r.GetMeditationSessionById(command.Session.Id), Times.Once);
            _meditationSessionRepositoryMock.Verify(r => r.UpdateMeditationSession(It.IsAny<MeditationSession>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
