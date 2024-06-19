using Infrastructure.Persistence;
using Infrastructure.Persistence.Repos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class AdminRepositoryTests
{
    private readonly AdminRepository _adminRepository;
    private readonly ApplicationDbContext _dbContext;
    private readonly Mock<IPublisher> _publisherMock;

    public AdminRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _publisherMock = new Mock<IPublisher>();
        _dbContext = new ApplicationDbContext(options, _publisherMock.Object);
        _adminRepository = new AdminRepository(_dbContext);
    }

    [Fact]
    public void Constructor_ShouldInitialize()
    {
        Assert.NotNull(_adminRepository);
    }
    
}