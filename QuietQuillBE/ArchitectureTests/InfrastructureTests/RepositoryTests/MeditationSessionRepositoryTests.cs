using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities.Meditation;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class MeditationSessionRepositoryTests
{
    private readonly MeditationSessionRepository _meditationSessionRepository;
    private readonly ApplicationDbContext _dbContext;
    private readonly Mock<IPublisher> _publisherMock;

    public MeditationSessionRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _publisherMock = new Mock<IPublisher>();
        _dbContext = new ApplicationDbContext(options, _publisherMock.Object);
        _meditationSessionRepository = new MeditationSessionRepository(_dbContext);
    }

    [Fact]
    public void CreateMeditationSession_ShouldAddEntity()
    {
        var session = new MeditationSession("Title", TimeSpan.FromMilliseconds(30), "desc", MeditationType.Guided);
        _meditationSessionRepository.CreateMeditationSession(session);

        var dbEntry = _dbContext.MeditationSessions.FirstOrDefault(ms => ms.Id == session.Id);
        Assert.NotNull(dbEntry);
        Assert.Equal(session.Title, dbEntry.Title);
    }

    [Fact]
    public void GetMeditationSessionById_ShouldReturnEntity()
    {
        var session = new MeditationSession("Title", TimeSpan.FromMilliseconds(30), "desc", MeditationType.Guided);
        _dbContext.MeditationSessions.Add(session);
        _dbContext.SaveChanges();

        var result = _meditationSessionRepository.GetMeditationSessionById(session.Id.ToString());
        Assert.NotNull(result);
        Assert.Equal(session.Id, result.Id);
    }

    [Fact]
    public void GetMeditationSessionById_ShouldThrowException_WhenNotFound()
    {
        var sessionId = Guid.NewGuid().ToString();

        var exception = Assert.Throws<Exception>(() => _meditationSessionRepository.GetMeditationSessionById(sessionId));
        Assert.Equal("Meditation session not found", exception.Message);
    }

    // Additional tests for other methods...
}
