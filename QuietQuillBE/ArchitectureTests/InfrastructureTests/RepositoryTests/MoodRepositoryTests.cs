using Infrastructure.Persistence;
using Infrastructure.Persistence.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class MoodRepositoryTests
{
    private readonly MoodRepository _moodRepository;
    private readonly ApplicationDbContext _dbContext;
    private readonly Mock<IPublisher> _publisherMock;

    public MoodRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _publisherMock = new Mock<IPublisher>();
        _dbContext = new ApplicationDbContext(options, _publisherMock.Object);
        _moodRepository = new MoodRepository(_dbContext);
    }

    [Fact]
    public void Constructor_ShouldInitialize()
    {
        Assert.NotNull(_moodRepository);
    }

    // Additional tests...
}