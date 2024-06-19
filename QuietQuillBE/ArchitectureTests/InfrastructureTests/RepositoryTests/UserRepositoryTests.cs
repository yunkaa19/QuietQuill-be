using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions.Users;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using MediatR;

namespace Infrastructure.Persistence.Repos.Tests
{
    public class UserRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public UserRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database name for each test
                .Options;
        }

        private ApplicationDbContext CreateDbContext()
        {
            var mockPublisher = new Mock<IPublisher>();
            return new ApplicationDbContext(_dbContextOptions, mockPublisher.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            using (var context = CreateDbContext())
            {
                var repository = new UserRepository(context);
                var user = new User(Guid.NewGuid(), "username", "passwordHash", "email@example.com", "identityId");

                await repository.AddAsync(user);

                var result = await context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                Assert.NotNull(result);
                Assert.Equal(user.UserId, result.UserId);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            using (var context = CreateDbContext())
            {
                var userId = Guid.NewGuid();
                var user = new User(userId, "username", "passwordHash", "email@example.com", "identityId");
                context.Users.Add(user);
                await context.SaveChangesAsync();

                var repository = new UserRepository(context);
                var result = await repository.GetByIdAsync(user.UserId);

                Assert.NotNull(result);
                Assert.Equal(user.UserId, result.UserId);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowUserNotFoundException()
        {
            using (var context = CreateDbContext())
            {
                var repository = new UserRepository(context);
                var userId = Guid.NewGuid();

                await Assert.ThrowsAsync<UserNotFoundException>(() => repository.GetByIdAsync(userId));
            }
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser()
        {
            using (var context = CreateDbContext())
            {
                var userId = Guid.NewGuid();
                var user = new User(userId, "username", "passwordHash", "email@example.com", "identityId");
                context.Users.Add(user);
                await context.SaveChangesAsync();

                user.Username = "newUsername";
                var repository = new UserRepository(context);
                await repository.UpdateAsync(user);

                var result = await context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                Assert.NotNull(result);
                Assert.Equal("newUsername", result.Username);
            }
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveUser()
        {
            using (var context = CreateDbContext())
            {
                var userId = Guid.NewGuid();
                var user = new User(userId, "username", "passwordHash", "email@example.com", "identityId");
                context.Users.Add(user);
                await context.SaveChangesAsync();

                var repository = new UserRepository(context);
                await repository.DeleteAsync(user);

                var result = await context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task UserExists_ShouldReturnTrue_WhenUserExists()
        {
            using (var context = CreateDbContext())
            {
                var userId = Guid.NewGuid();
                var user = new User(userId, "username", "passwordHash", "email@example.com", "identityId");
                context.Users.Add(user);
                await context.SaveChangesAsync();

                var repository = new UserRepository(context);
                var result = await repository.UserExists(user.Email);

                Assert.True(result);
            }
        }

        [Fact]
        public async Task UserExists_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            using (var context = CreateDbContext())
            {
                var repository = new UserRepository(context);
                var result = await repository.UserExists("nonexistent@example.com");

                Assert.False(result);
            }
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnUser()
        {
            using (var context = CreateDbContext())
            {
                var userId = Guid.NewGuid();
                var email = "email@example.com";
                var user = new User(userId, "username", "passwordHash", email, "identityId");
                context.Users.Add(user);
                await context.SaveChangesAsync();

                // Verify the user was added correctly
                var savedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
                Assert.NotNull(savedUser);
                Assert.Equal(userId, savedUser.UserId);

                var repository = new UserRepository(context);
                var result = await repository.GetUserByEmail(email);

                Assert.NotNull(result);
                Assert.Equal(user.UserId, result.UserId);
                Assert.Equal(user.Username, result.Username);
                Assert.Equal(user.PasswordHash, result.PasswordHash);
                Assert.Equal(user.Email, result.Email);
                Assert.Equal(user.IdentityID, result.IdentityID);
            }
        }

        [Fact]
        public async Task GetUserByEmail_ShouldThrowUserNotFoundException()
        {
            using (var context = CreateDbContext())
            {
                var repository = new UserRepository(context);

                await Assert.ThrowsAsync<UserNotFoundException>(() => repository.GetUserByEmail("nonexistent@example.com"));
            }
        }
    }
}
