using System;
using System.Threading.Tasks;
using Domain.Entities.PaperPlane;
using Domain.Exceptions.PaperPlane;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Infrastructure.Persistence.Repos.Tests
{
    public class PaperPlaneRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public PaperPlaneRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "PaperPlaneDatabase")
                .Options;
        }

        private ApplicationDbContext CreateDbContext()
        {
            var mockPublisher = new Mock<IPublisher>();
            var context = new ApplicationDbContext(_dbContextOptions, mockPublisher.Object);
            context.Database.EnsureDeleted(); 
            context.Database.EnsureCreated(); 
            return context;
        }

        [Fact]
        public async Task GetLatestPaperPlane_ShouldThrowPaperPlaneNotFoundException_WhenNoneExist()
        {
            using (var context = CreateDbContext())
            {
                var repository = new PaperPlaneRepository(context);

                // Ensure the database is empty
                Assert.Empty(await context.PaperPlanes.ToListAsync());

                var exception =
                    await Assert.ThrowsAsync<PaperPlaneNotFoundException>(() => repository.GetLatestPaperPlane());
                Assert.Equal("No paper planes have been sent yet.", exception.Message);
            }
        }

        [Fact]
        public async Task AddPaperPlane_ShouldAddPaperPlane()
        {
            using (var context = CreateDbContext())
            {
                var repository = new PaperPlaneRepository(context);
                var paperPlane = new PaperPlaneEntity("Test content", Guid.NewGuid());

                await repository.AddPaperPlane(paperPlane);

                var result = await context.PaperPlanes.FirstOrDefaultAsync(p => p.id == paperPlane.id);
                Assert.NotNull(result);
                Assert.Equal(paperPlane.id, result.id);
            }
        }

        [Fact]
        public async Task GetPaperPlaneById_ShouldReturnPaperPlane()
        {
            using (var context = CreateDbContext())
            {
                var paperPlane = new PaperPlaneEntity("Test content", Guid.NewGuid());
                context.PaperPlanes.Add(paperPlane);
                await context.SaveChangesAsync();

                var repository = new PaperPlaneRepository(context);
                var result = await repository.GetPaperPlaneById(paperPlane.id);

                Assert.NotNull(result);
                Assert.Equal(paperPlane.id, result.id);
            }
        }

        [Fact]
        public async Task GetPaperPlaneById_ShouldThrowPaperPlaneNotFoundException_WhenNotFound()
        {
            using (var context = CreateDbContext())
            {
                var repository = new PaperPlaneRepository(context);

                var exception = await Assert.ThrowsAsync<PaperPlaneNotFoundException>(() => repository.GetPaperPlaneById(99));
                Assert.Equal("The paper plane with the identifier 99 was not found.", exception.Message);
            }
        }

        [Fact]
        public async Task HasSentOneRecently_ShouldReturnTrue()
        {
            using (var context = CreateDbContext())
            {
                var userId = Guid.NewGuid();
                var paperPlane = new PaperPlaneEntity("Test content", userId);
                context.PaperPlanes.Add(paperPlane);
                await context.SaveChangesAsync();

                var repository = new PaperPlaneRepository(context);
                var result = await repository.HasSentOneRecently(userId);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task HasSentOneRecently_ShouldReturnFalse()
        {
            using (var context = CreateDbContext())
            {
                var userId = Guid.NewGuid();
                var paperPlane = new PaperPlaneEntity("Test content", userId)
                {
                    date = DateTime.Now.AddMinutes(-20)
                };
                context.PaperPlanes.Add(paperPlane);
                await context.SaveChangesAsync();

                var repository = new PaperPlaneRepository(context);
                var result = await repository.HasSentOneRecently(userId);

                Assert.False(result);
            }
        }

        [Fact]
        public async Task GetLatestPaperPlane_ShouldReturnLatestPaperPlane()
        {
            using (var context = CreateDbContext())
            {
                var paperPlane1 = new PaperPlaneEntity("Test content 1", Guid.NewGuid())
                {
                    date = DateTime.Now.AddMinutes(-20)
                };
                var paperPlane2 = new PaperPlaneEntity("Test content 2", Guid.NewGuid());
                context.PaperPlanes.AddRange(paperPlane1, paperPlane2);
                await context.SaveChangesAsync();

                var repository = new PaperPlaneRepository(context);
                var result = await repository.GetLatestPaperPlane();

                Assert.NotNull(result);
                Assert.Equal(paperPlane2.id, result.id);
            }
        }

        
    }
}
