using Infrastructure.Persistence;
using Infrastructure.Persistence.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class ReminderRepositoryTests
{
    private readonly ReminderRepository _reminderRepository;
    private readonly ApplicationDbContext _dbContext;
    private readonly Mock<IPublisher> _publisherMock;

    public ReminderRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _publisherMock = new Mock<IPublisher>();
        _dbContext = new ApplicationDbContext(options, _publisherMock.Object);
        _reminderRepository = new ReminderRepository(_dbContext);
    }

    [Fact]
    public void Constructor_ShouldInitialize()
    {
        Assert.NotNull(_reminderRepository);
    }

    // Additional tests...
}