using Infrastructure.Persistence;
using Infrastructure.Persistence.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class UserMeditationRecordRepositoryTests
{
    private readonly UserMeditationRecordRepository _userMeditationRecordRepository;
    private readonly ApplicationDbContext _dbContext;
    private readonly Mock<IPublisher> _publisherMock;

    public UserMeditationRecordRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _publisherMock = new Mock<IPublisher>();
        _dbContext = new ApplicationDbContext(options, _publisherMock.Object);
        _userMeditationRecordRepository = new UserMeditationRecordRepository(_dbContext);
    }

    [Fact]
    public void Constructor_ShouldInitialize()
    {
        Assert.NotNull(_userMeditationRecordRepository);
    }

    // Additional tests...
}