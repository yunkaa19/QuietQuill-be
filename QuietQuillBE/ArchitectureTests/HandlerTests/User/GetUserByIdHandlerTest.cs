using Application.Users.Queries.GetUserById;
using Application.Abstraction.Data;
using Domain.Exceptions.Users;
using Moq;

namespace ArchitectureTests.HandlerTests.Users
{
    public class GetUserByIdHandlerTests
    {
        private readonly Mock<IDbQueryExecutor> _dbQueryExecutorMock;
        private readonly GetUserByIdHandler _handler;

        public GetUserByIdHandlerTests()
        {
            _dbQueryExecutorMock = new Mock<IDbQueryExecutor>();
            _handler = new GetUserByIdHandler(_dbQueryExecutorMock.Object);
        }

        private GetUserByIdQuery CreateValidQuery()
        {
            return new GetUserByIdQuery(Guid.NewGuid());
        }

        private UserResponse CreateValidUserResponse(Guid userId)
        {
            return new UserResponse
            (
                userId,
                "testuser@example.com"
            );
        }

        [Fact]
        public async Task Handle_Should_ReturnUser_When_UserExists()
        {
            // Arrange
            var query = CreateValidQuery();
            var expectedResponse = CreateValidUserResponse(query.UserId);

            _dbQueryExecutorMock.Setup(db => db.QueryFirstOrDefaultAsync<UserResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.UserId, result.UserId);
            Assert.Equal(expectedResponse.Email, result.Email);

            _dbQueryExecutorMock.Verify(db => db.QueryFirstOrDefaultAsync<UserResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowUserNotFoundException_When_UserDoesNotExist()
        {
            // Arrange
            var query = CreateValidQuery();

            _dbQueryExecutorMock.Setup(db => db.QueryFirstOrDefaultAsync<UserResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
                .ReturnsAsync((UserResponse)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(query, CancellationToken.None));

            _dbQueryExecutorMock.Verify(db => db.QueryFirstOrDefaultAsync<UserResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);
        }
    }
}
