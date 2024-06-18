using Application.Journals.Commands.UpdateEntry;
using Application.Journals.DTOs;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace ArchitectureTests.HandlerTests.Journal
{
    public class UpdateEntryCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJournalEntryRepository> _journalEntryRepositoryMock;
        private readonly Mock<IValidator<UpdateEntryCommand>> _validatorMock;
        private readonly UpdateEntryCommandHandler _handler;

        public UpdateEntryCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _journalEntryRepositoryMock = new Mock<IJournalEntryRepository>();
            _validatorMock = new Mock<IValidator<UpdateEntryCommand>>();

            _handler = new UpdateEntryCommandHandler(_unitOfWorkMock.Object, _journalEntryRepositoryMock.Object, _validatorMock.Object);
        }

        private UpdateEntryCommand CreateValidCommand()
        {
            var journalDto = new JournalDTO
            {
                UserId = Guid.NewGuid(),
                Id = Guid.NewGuid().ToString(),
                Day = "1",
                Month = "1",
                Year = "2024",
                Content = "Updated content",
                Mood = Mood.HAPPY,
                Tags = "tag1,tag2"
            };
            return new UpdateEntryCommand(journalDto);
        }

        [Fact]
        public async Task Handle_Should_UpdateEntry_When_ValidRequest()
        {
            // Arrange
            var command = CreateValidCommand();

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            var journalEntry = new JournalEntry
            (
                command.Entry.UserId,
                 "Original content",
                DateOnly.Parse("1/1/2024"),
                Mood.SAD,
                "tag3,tag4"
            );

            _journalEntryRepositoryMock.Setup(r => r.GetJournalEntryById(It.IsAny<string>()))
                                       .Returns(journalEntry);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Entry.Content, result.Content);
            _journalEntryRepositoryMock.Verify(r => r.GetJournalEntryById(It.IsAny<string>()), Times.Once);
            _journalEntryRepositoryMock.Verify(r => r.UpdateJournalEntry(It.IsAny<JournalEntry>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowValidationException_When_InvalidRequest()
        {
            // Arrange
            var command = CreateValidCommand();

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Property", "Error") }));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

            _journalEntryRepositoryMock.Verify(r => r.GetJournalEntryById(It.IsAny<string>()), Times.Never);
            _journalEntryRepositoryMock.Verify(r => r.UpdateJournalEntry(It.IsAny<JournalEntry>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_JournalEntryNotFound()
        {
            // Arrange
            var command = CreateValidCommand();

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _journalEntryRepositoryMock.Setup(r => r.GetJournalEntryById(It.IsAny<string>()))
                                       .Returns((JournalEntry)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            _journalEntryRepositoryMock.Verify(r => r.GetJournalEntryById(It.IsAny<string>()), Times.Once);
            _journalEntryRepositoryMock.Verify(r => r.UpdateJournalEntry(It.IsAny<JournalEntry>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_UnitOfWorkFailsToSaveChanges()
        {
            // Arrange
            var command = CreateValidCommand();

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            var journalEntry = new JournalEntry
            (
                command.Entry.UserId,
                "Original content",
                DateOnly.Parse("1/1/2024"),
                Mood.SAD,
                "tag3,tag4"
            );

            _journalEntryRepositoryMock.Setup(r => r.GetJournalEntryById(It.IsAny<string>()))
                                       .Returns(journalEntry);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                           .ThrowsAsync(new Exception("UnitOfWork save failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            _journalEntryRepositoryMock.Verify(r => r.GetJournalEntryById(It.IsAny<string>()), Times.Once);
            _journalEntryRepositoryMock.Verify(r => r.UpdateJournalEntry(It.IsAny<JournalEntry>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
