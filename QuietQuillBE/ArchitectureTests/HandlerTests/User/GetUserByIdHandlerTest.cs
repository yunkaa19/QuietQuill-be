using Application.Users.Queries.GetUserById;
using Application.Abstraction.Data;
using Domain.Exceptions.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Application.Users.DTOs;

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

        private FullUserDTO CreateValidFullUserDTO(Guid userId)
        {
            return new FullUserDTO
            {
                UserId = userId,
                Username = "testuser",
                Email = "testuser@example.com",
                JournalEntries = new List<JournalEntryDto>(),
                Reminders = new List<ReminderDto>(),
                Habits = new List<HabitDto>(),
                MeditationSessions = new List<MeditationSessionDto>(),
                UserQuizRecords = new List<UserQuizRecordDto>()
            };
        }

        [Fact]
        public async Task Handle_Should_ReturnUser_When_UserExists()
        {
            // Arrange
            var query = CreateValidQuery();
            var expectedResponse = CreateValidFullUserDTO(query.UserId);

            _dbQueryExecutorMock.Setup(db => db.QueryFirstOrDefaultAsync<FullUserDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
                .ReturnsAsync(expectedResponse);

            _dbQueryExecutorMock.Setup(db => db.QueryAsync<JournalEntryDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
                .ReturnsAsync(new List<JournalEntryDto>());

            _dbQueryExecutorMock.Setup(db => db.QueryAsync<ReminderDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
                .ReturnsAsync(new List<ReminderDto>());

            _dbQueryExecutorMock.Setup(db => db.QueryAsync<HabitDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
                .ReturnsAsync(new List<HabitDto>());

            _dbQueryExecutorMock.Setup(db => db.QueryAsync<MeditationSessionDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
                .ReturnsAsync(new List<MeditationSessionDto>());

            _dbQueryExecutorMock.Setup(db => db.QueryAsync<UserQuizRecordDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
                .ReturnsAsync(new List<UserQuizRecordDto>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResponse.UserId, result.UserId);
            Assert.Equal(expectedResponse.Email, result.Email);
            Assert.Equal(expectedResponse.Username, result.Username);
            Assert.Empty(result.JournalEntries);
            Assert.Empty(result.Reminders);
            Assert.Empty(result.Habits);
            Assert.Empty(result.MeditationSessions);
            Assert.Empty(result.UserQuizRecords);

            _dbQueryExecutorMock.Verify(db => db.QueryFirstOrDefaultAsync<FullUserDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);

            _dbQueryExecutorMock.Verify(db => db.QueryAsync<JournalEntryDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);

            _dbQueryExecutorMock.Verify(db => db.QueryAsync<ReminderDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);

            _dbQueryExecutorMock.Verify(db => db.QueryAsync<HabitDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);

            _dbQueryExecutorMock.Verify(db => db.QueryAsync<MeditationSessionDto>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);

            _dbQueryExecutorMock.Verify(db => db.QueryAsync<UserQuizRecordDto>(
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

            _dbQueryExecutorMock.Setup(db => db.QueryFirstOrDefaultAsync<FullUserDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null))
                .ReturnsAsync((FullUserDTO)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _handler.Handle(query, CancellationToken.None));

            _dbQueryExecutorMock.Verify(db => db.QueryFirstOrDefaultAsync<FullUserDTO>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null), Times.Once);
        }
    }
}
