using Application.Abstraction.Data;
using Application.Meditation.Queries.GetMeditation;
using Domain.Entities.Meditation;
using Domain.Exceptions.Meditation;
using Moq;
using Xunit;

namespace ArchitectureTests.HandlerTests.Meditation
{
    public class GetMeditationHandlerTests
    {
        private readonly Mock<IDbQueryExecutor> _dbQueryExecutorMock;
        private readonly GetMeditationHandler _handler;

        public GetMeditationHandlerTests()
        {
            _dbQueryExecutorMock = new Mock<IDbQueryExecutor>();
            _handler = new GetMeditationHandler(_dbQueryExecutorMock.Object);
        }

        private GetMeditationQuery CreateValidQuery()
        {
            return new GetMeditationQuery(Guid.NewGuid().ToString());
        }

        private MeditationSession CreateValidMeditationSession()
        {
            return new MeditationSession(
                "Meditation Name",
                new TimeSpan(0, 30, 0), // 30 minutes
                "Meditation Description",
                MeditationType.Guided);
        }

        private GetMeditationResponse CreateValidResponse(MeditationSession session)
        {
            return new GetMeditationResponse(session);
        }

        [Fact]
        public async Task Handle_Should_ReturnMeditation_When_MeditationExists()
        {
            // Arrange
            var query = CreateValidQuery();
            var meditationSession = CreateValidMeditationSession();
            var expectedResponse = CreateValidResponse(meditationSession);

            _dbQueryExecutorMock.Setup(db => db.QueryFirstOrDefaultAsync<GetMeditationResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.MeditationSession.Id, result.MeditationSession.Id);
            Assert.Equal(expectedResponse.MeditationSession.Title, result.MeditationSession.Title);
            Assert.Equal(expectedResponse.MeditationSession.Duration, result.MeditationSession.Duration);
            _dbQueryExecutorMock.Verify(db => db.QueryFirstOrDefaultAsync<GetMeditationResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowMeditationNotFoundException_When_MeditationDoesNotExist()
        {
            // Arrange
            var query = CreateValidQuery();

            _dbQueryExecutorMock.Setup(db => db.QueryFirstOrDefaultAsync<GetMeditationResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null))
                .ReturnsAsync((GetMeditationResponse)null);

            // Act & Assert
            await Assert.ThrowsAsync<MeditationNotFoundException>(() => _handler.Handle(query, CancellationToken.None));

            _dbQueryExecutorMock.Verify(db => db.QueryFirstOrDefaultAsync<GetMeditationResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null, null, null), Times.Once);
        }
    }
}
