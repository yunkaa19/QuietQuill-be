using Infrastructure.Persistence;
using Infrastructure.Persistence.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class ResourceRepositoryTests
{
    private readonly ResourceRepository _resourceRepository;
    private readonly ApplicationDbContext _dbContext;
    private readonly Mock<IPublisher> _publisherMock;

    public ResourceRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _publisherMock = new Mock<IPublisher>();
        _dbContext = new ApplicationDbContext(options, _publisherMock.Object);
        _resourceRepository = new ResourceRepository(_dbContext);
    }

    [Fact]
    public void Constructor_ShouldInitialize()
    {
        Assert.NotNull(_resourceRepository);
    }

    // Additional tests...
}