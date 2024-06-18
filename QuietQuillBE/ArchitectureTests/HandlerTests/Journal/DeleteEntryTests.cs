using Application.Journals.Commands.DeleteEntry;
using Domain.Abstraction;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Moq;


namespace ArchitectureTests.HandlerTests.Journal
{
    public class DeleteEntryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJournalEntryRepository> _journalEntryRepositoryMock;
        private readonly Mock<IValidator<DeleteEntryCommand>> _validatorMock;
        private readonly DeleteEntryHandler _handler;

        public DeleteEntryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _journalEntryRepositoryMock = new Mock<IJournalEntryRepository>();
            _validatorMock = new Mock<IValidator<DeleteEntryCommand>>();

            _handler = new DeleteEntryHandler(_unitOfWorkMock.Object, _journalEntryRepositoryMock.Object,
                _validatorMock.Object);
        }

        private DeleteEntryCommand CreateValidCommand()
        {
            var journalId = Guid.NewGuid().ToString();
            return new DeleteEntryCommand(journalId);
        }

        [Fact]
        public async Task Handle_Should_DeleteEntry_When_ValidRequest()
        {
            // Arrange
            var command = CreateValidCommand();

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _journalEntryRepositoryMock.Setup(r => r.DeleteJournalEntry(It.IsAny<string>()))
                .Returns(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _journalEntryRepositoryMock.Verify(r => r.DeleteJournalEntry(It.IsAny<string>()), Times.Once);
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

            _journalEntryRepositoryMock.Verify(r => r.DeleteJournalEntry(It.IsAny<string>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ReturnFalse_When_RepositoryFailsToDeleteEntry()
        {
            // Arrange
            var command = CreateValidCommand();

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _journalEntryRepositoryMock.Setup(r => r.DeleteJournalEntry(It.IsAny<string>()))
                .Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _journalEntryRepositoryMock.Verify(r => r.DeleteJournalEntry(It.IsAny<string>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }


        [Fact]
        public async Task Handle_Should_ThrowException_When_UnitOfWorkFailsToSaveChanges()
        {
            // Arrange
            var command = CreateValidCommand();

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _journalEntryRepositoryMock.Setup(r => r.DeleteJournalEntry(It.IsAny<string>()))
                .Returns(true);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("UnitOfWork save failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("UnitOfWork save failed", exception.Message);

            _journalEntryRepositoryMock.Verify(r => r.DeleteJournalEntry(It.IsAny<string>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}