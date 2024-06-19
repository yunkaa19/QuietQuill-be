using System;
using System.Linq;
using Domain.Entities;
using Domain.Exceptions.Journal;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class JournalEntryRepositoryTests
{
    private readonly JournalEntryRepository _journalEntryRepository;
    private readonly ApplicationDbContext _dbContext;
    private readonly Mock<IPublisher> _publisherMock;

    public JournalEntryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _publisherMock = new Mock<IPublisher>();
        _dbContext = new ApplicationDbContext(options, _publisherMock.Object);
        _journalEntryRepository = new JournalEntryRepository(_dbContext);
    }

    [Fact]
    public void CreateJournalEntry_ShouldAddEntity()
    {
        var journalEntry = new JournalEntry(Guid.NewGuid(), "content", DateOnly.FromDateTime(DateTime.Now), Mood.HAPPY, "tag1, tag2");
        var result = _journalEntryRepository.CreateJournalEntry(journalEntry);
        Assert.True(result);
    }

    [Fact]
    public void GetJournalEntryById_ShouldReturnEntity()
    {
        var journalEntry = new JournalEntry(Guid.NewGuid(), "content", DateOnly.FromDateTime(DateTime.Now), Mood.HAPPY, "tag1, tag2");
        _dbContext.JournalEntries.Add(journalEntry);
        _dbContext.SaveChanges();

        var result = _journalEntryRepository.GetJournalEntryById(journalEntry.Id);
        Assert.NotNull(result);
        Assert.Equal(journalEntry.Id, result.Id);
    }

    [Fact]
    public void GetJournalEntryById_ShouldThrowJournalNotFoundException_WhenNotFound()
    {
        var journalEntryId = "nonexistent-id";

        var exception = Assert.Throws<JournalNotFoundException>(() => _journalEntryRepository.GetJournalEntryById(journalEntryId));
        Assert.Equal($"The journal with the identifier {journalEntryId} was not found.", exception.Message);
    }

    // Additional tests for other methods...
}
