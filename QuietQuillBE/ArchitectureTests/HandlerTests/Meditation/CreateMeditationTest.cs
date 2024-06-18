using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Meditation.Commands.CreateMeditation;
using Application.Meditation.DTOs;
using Domain.Abstraction;
using Domain.Entities.Meditation;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace ArchitectureTests.HandlerTests.Meditation
{
    public class CreateMeditationCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IValidator<CreateMeditationCommand>> _validatorMock;
        private readonly Mock<IMeditationSessionRepository> _meditationSessionRepositoryMock;
        private readonly CreateMeditationCommandHandler _handler;

        public CreateMeditationCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<CreateMeditationCommand>>();
            _meditationSessionRepositoryMock = new Mock<IMeditationSessionRepository>();
            _handler = new CreateMeditationCommandHandler(_unitOfWorkMock.Object, _validatorMock.Object, _meditationSessionRepositoryMock.Object);
        }

        private CreateMeditationCommand CreateValidCommand()
        {
            return new CreateMeditationCommand
            (
                new MeditationSessionDTO
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Meditation Name",
                    Duration = new TimeSpan(0, 30, 0), // 30 minutes
                    Description = "Meditation Description",
                    Type = MeditationType.Guided
                }
            );
        }

        [Fact]
        public async Task Handle_Should_CreateMeditationSession_When_ValidationPasses()
        {
            // Arrange
            var command = CreateValidCommand();
            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.MeditationSessionDto.Id, result.Id);
            Assert.Equal(command.MeditationSessionDto.Title, result.Title);
            Assert.Equal(command.MeditationSessionDto.Duration, result.Duration);
            Assert.Equal(command.MeditationSessionDto.Description, result.Description);
            Assert.Equal(command.MeditationSessionDto.Type, result.Type);

            _meditationSessionRepositoryMock.Verify(r => r.CreateMeditationSession(It.IsAny<MeditationSession>()), Times.Once);
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

            _meditationSessionRepositoryMock.Verify(r => r.CreateMeditationSession(It.IsAny<MeditationSession>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
