
using Application.Journals.Commands.CreateEntry;
using Application.Journals.DTOs;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Moq;

public class CreateEntryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IJournalEntryRepository> _journalEntryRepositoryMock;
    private readonly Mock<IValidator<CreateEntryCommand>> _validatorMock;
    private readonly CreateEntryHandler _handler;

    public CreateEntryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _journalEntryRepositoryMock = new Mock<IJournalEntryRepository>();
        _validatorMock = new Mock<IValidator<CreateEntryCommand>>();

        _handler = new CreateEntryHandler(_unitOfWorkMock.Object, _journalEntryRepositoryMock.Object, _validatorMock.Object);
    }

    private CreateEntryCommand CreateValidCommand()
    {
        var journalDto = new JournalDTO
        {
            UserId = Guid.NewGuid(),
            Day = "1",
            Month = "1",
            Year = "2024",
            Content = "Test content",
            Mood = Mood.HAPPY,
            Tags = "tag1,tag2"
        };
        return new CreateEntryCommand(journalDto);
    }

    [Fact]
    public async Task Handle_Should_CreateEntry_When_ValidRequest()
    {
        // Arrange
        var command = CreateValidCommand();

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _journalEntryRepositoryMock.Setup(r => r.CreateJournalEntry(It.IsAny<JournalEntry>()))
                                   .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Entry.Content, result.Content);
        _journalEntryRepositoryMock.Verify(r => r.CreateJournalEntry(It.IsAny<JournalEntry>()), Times.Once);
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

        _journalEntryRepositoryMock.Verify(r => r.CreateJournalEntry(It.IsAny<JournalEntry>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_RepositoryFailsToAddEntry()
    {
        // Arrange
        var command = CreateValidCommand();

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _journalEntryRepositoryMock.Setup(r => r.CreateJournalEntry(It.IsAny<JournalEntry>()))
                                   .Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_UnitOfWorkFailsToSaveChanges()
    {
        // Arrange
        var command = CreateValidCommand();

        _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _journalEntryRepositoryMock.Setup(r => r.CreateJournalEntry(It.IsAny<JournalEntry>()))
                                   .Returns(true);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .ThrowsAsync(new Exception("UnitOfWork save failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

        _journalEntryRepositoryMock.Verify(r => r.CreateJournalEntry(It.IsAny<JournalEntry>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
