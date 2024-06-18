using Application.Abstraction.Data;
using Application.Journals.Queries.GetJournalsByMonth;
using Application.Journals.DTOs;
using Domain.Entities;
using Moq;

namespace ArchitectureTests.HandlerTests.Journal
{
    public class GetJournalsByMonthHandlerTests
    {
        private readonly Mock<IDbQueryExecutor> _dbQueryExecutorMock;
        private readonly GetJournalsByMonthHandler _handler;

        public GetJournalsByMonthHandlerTests()
        {
            _dbQueryExecutorMock = new Mock<IDbQueryExecutor>();
            _handler = new GetJournalsByMonthHandler(_dbQueryExecutorMock.Object);
        }

        private GetJournalsByMonthQuery CreateValidQuery()
        {
            return new GetJournalsByMonthQuery
            (
                new Guid("d2719b2a-6af7-4b8b-96df-a2f3a6f1f9b2").ToString(),
                "1", // Month
                "2024" // Year
            );
        }

        [Fact]
        public async Task Handle_Should_ReturnJournals_When_ValidRequest()
        {
            // Arrange
            var query = CreateValidQuery();
            var expectedJournals = new List<JournalDTO>
            {
                new JournalDTO
                {
                    Id = "journal-entry-1",
                    UserId = new Guid("d2719b2a-6af7-4b8b-96df-a2f3a6f1f9b2"),
                    Content = "Content 1",
                    Day = "1",
                    Month = "1",
                    Year = "2024",
                    Mood = Mood.HAPPY,
                    Tags = "tag1,tag2"
                },
                new JournalDTO
                {
                    Id = "journal-entry-2",
                    UserId = new Guid("d2719b2a-6af7-4b8b-96df-a2f3a6f1f9b2"),
                    Content = "Content 2",
                    Day = "2",
                    Month = "1",
                    Year = "2024",
                    Mood = Mood.SAD,
                    Tags = "tag3,tag4"
                }
            };

            _dbQueryExecutorMock.Setup(db => db.QueryAsync<JournalDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null)).ReturnsAsync(expectedJournals);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedJournals.Count, result.Journals.Length);
            for (int i = 0; i < expectedJournals.Count; i++)
            {
                Assert.Equal(expectedJournals[i].Id, result.Journals[i].Id);
                Assert.Equal(expectedJournals[i].UserId, result.Journals[i].UserId);
                Assert.Equal(expectedJournals[i].Content, result.Journals[i].Content);
                Assert.Equal(expectedJournals[i].Day, result.Journals[i].Day);
                Assert.Equal(expectedJournals[i].Month, result.Journals[i].Month);
                Assert.Equal(expectedJournals[i].Year, result.Journals[i].Year);
                Assert.Equal(expectedJournals[i].Mood, result.Journals[i].Mood);
                Assert.Equal(expectedJournals[i].Tags, result.Journals[i].Tags);
            }

            _dbQueryExecutorMock.Verify(db => db.QueryAsync<JournalDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyArray_When_NoJournalsFound()
        {
            // Arrange
            var query = CreateValidQuery();

            _dbQueryExecutorMock.Setup(db => db.QueryAsync<JournalDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null)).ReturnsAsync(new List<JournalDTO>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Journals);

            _dbQueryExecutorMock.Verify(db => db.QueryAsync<JournalDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_DatabaseConnectionFails()
        {
            // Arrange
            var query = CreateValidQuery();

            _dbQueryExecutorMock.Setup(db => db.QueryAsync<JournalDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null)).ThrowsAsync(new Exception("Database connection failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("Database connection failed", exception.Message);

            _dbQueryExecutorMock.Verify(db => db.QueryAsync<JournalDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);
        }
    }
}
