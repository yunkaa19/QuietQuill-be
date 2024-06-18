using Application.Meditation.Commands.DeleteMeditation;
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
    public class DeleteMeditationCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IValidator<DeleteMeditationCommand>> _validatorMock;
        private readonly Mock<IMeditationSessionRepository> _meditationSessionRepositoryMock;
        private readonly DeleteMeditationCommandHandler _handler;

        public DeleteMeditationCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<DeleteMeditationCommand>>();
            _meditationSessionRepositoryMock = new Mock<IMeditationSessionRepository>();
            _handler = new DeleteMeditationCommandHandler(_unitOfWorkMock.Object, _validatorMock.Object, _meditationSessionRepositoryMock.Object);
        }

        private DeleteMeditationCommand CreateValidCommand()
        {
            return new DeleteMeditationCommand
            (
                new MeditationSessionDTO
                {
                    Id = Guid.NewGuid().ToString()
                }
            );
        }

        [Fact]
        public async Task Handle_Should_DeleteMeditationSession_When_ValidationPasses_And_SessionExists()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationResult = new ValidationResult();
            var meditationSession = new MeditationSession("Meditation Name", new TimeSpan(0, 30, 0), "Meditation Description", MeditationType.Guided);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _meditationSessionRepositoryMock.Setup(r => r.GetMeditationSessionById(command.MeditationSessionDto.Id)).Returns(meditationSession);
            _meditationSessionRepositoryMock.Setup(r => r.DeleteMeditationSession(command.MeditationSessionDto.Id)).Verifiable();
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _meditationSessionRepositoryMock.Verify(r => r.DeleteMeditationSession(command.MeditationSessionDto.Id), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowValidationException_When_ValidationFails()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Id", "Id is required") };
            var validationResult = new ValidationResult(validationFailures);

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

            Assert.Equal(validationResult.Errors, exception.Errors);

            _meditationSessionRepositoryMock.Verify(r => r.GetMeditationSessionById(It.IsAny<string>()), Times.Never);
            _meditationSessionRepositoryMock.Verify(r => r.DeleteMeditationSession(It.IsAny<string>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowMeditationNotFoundException_When_SessionDoesNotExist()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationResult = new ValidationResult();

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);
            _meditationSessionRepositoryMock.Setup(r => r.GetMeditationSessionById(command.MeditationSessionDto.Id)).Returns((MeditationSession)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<MeditationNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _meditationSessionRepositoryMock.Verify(r => r.GetMeditationSessionById(command.MeditationSessionDto.Id), Times.Once);
            _meditationSessionRepositoryMock.Verify(r => r.DeleteMeditationSession(It.IsAny<string>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
