using Domain.Entities.MentalHealthSupport;

namespace ArchitectureTests.EntitiesUnitTests
{
    public class UserQuizRecordTests
    {
        [Fact]
        public void CreateUserQuizRecord_ShouldInitializeProperties()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var quizId = Guid.NewGuid();
            var score = 85;

            // Act
            var userQuizRecord = new UserQuizRecord(userId, quizId, score);

            // Assert
            Assert.Equal(userId, userQuizRecord.UserId);
            Assert.Equal(quizId, userQuizRecord.QuizId);
            Assert.Equal(score, userQuizRecord.Score);
            Assert.NotEqual(Guid.Empty, userQuizRecord.Id);
            Assert.True(userQuizRecord.CompletedOn <= DateTime.UtcNow);
        }
    }
}