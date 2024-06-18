using Application.Abstraction.Data;
using Application.Journals.Queries.GetJournalEntryByID;
using Application.Journals.DTOs;
using Domain.Entities;
using Moq;

namespace ArchitectureTests.HandlerTests.Journal
{
    public class GetJournalEntryByIDHandlerTests
    {
        private readonly Mock<IDbQueryExecutor> _dbQueryExecutorMock;
        private readonly GetJournalEntryByIDHandler _handler;

        public GetJournalEntryByIDHandlerTests()
        {
            _dbQueryExecutorMock = new Mock<IDbQueryExecutor>();
            _handler = new GetJournalEntryByIDHandler(_dbQueryExecutorMock.Object);
        }

        private GetJournalEntryByIDQuery CreateValidQuery()
        {
            return new GetJournalEntryByIDQuery
            (
                new Guid("d2719b2a-6af7-4b8b-96df-a2f3a6f1f9b2").ToString(),
                "valid-entry-id"
            );
        }

        [Fact]
        public async Task Handle_Should_ReturnJournalEntry_When_ValidRequest()
        {
            // Arrange
            var query = CreateValidQuery();
            var expectedJournal = new JournalDTO
            {
                Id = "valid-entry-id",
                UserId = new Guid("d2719b2a-6af7-4b8b-96df-a2f3a6f1f9b2"),
                Content = "Some content",
                Day = "1",
                Month = "January",
                Year = "2022",
                Mood = Mood.HAPPY,
                Tags = "tag1,tag2"
            };

            _dbQueryExecutorMock.Setup(db => db.QueryFirstOrDefaultAsync<JournalDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null)).ReturnsAsync(expectedJournal);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedJournal.Id, result.Entry.Id);
            Assert.Equal(expectedJournal.UserId, result.Entry.UserId);
            Assert.Equal(expectedJournal.Content, result.Entry.Content);
            Assert.Equal(expectedJournal.Day, result.Entry.Day);
            Assert.Equal(expectedJournal.Month, result.Entry.Month);
            Assert.Equal(expectedJournal.Year, result.Entry.Year);
            Assert.Equal(expectedJournal.Mood, result.Entry.Mood);
            Assert.Equal(expectedJournal.Tags, result.Entry.Tags);

            _dbQueryExecutorMock.Verify(db => db.QueryFirstOrDefaultAsync<JournalDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ReturnNull_When_JournalEntryNotFound()
        {
            // Arrange
            var query = CreateValidQuery();

            _dbQueryExecutorMock.Setup(db => db.QueryFirstOrDefaultAsync<JournalDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null)).ReturnsAsync((JournalDTO)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result.Entry);
            _dbQueryExecutorMock.Verify(db => db.QueryFirstOrDefaultAsync<JournalDTO>(
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

            _dbQueryExecutorMock.Setup(db => db.QueryFirstOrDefaultAsync<JournalDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null)).ThrowsAsync(new Exception("Database connection failed"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("Database connection failed", exception.Message);

            _dbQueryExecutorMock.Verify(db => db.QueryFirstOrDefaultAsync<JournalDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);
        }
    }
}
