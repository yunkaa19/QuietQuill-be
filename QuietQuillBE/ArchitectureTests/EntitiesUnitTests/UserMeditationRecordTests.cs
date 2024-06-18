using Domain.Entities.Meditation;

namespace ArchitectureTests.EntitiesUnitTests
{
    public class UserMeditationRecordTests
    {
        [Fact]
        public void CreateUserMeditationRecord_ShouldInitializeProperties()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var meditationSessionId = Guid.NewGuid();
            var duration = new TimeSpan(0, 30, 0); // 30 minutes

            // Act
            var userMeditationRecord = new UserMeditationRecord(userId, meditationSessionId, duration);

            // Assert
            Assert.Equal(userId, userMeditationRecord.UserId);
            Assert.Equal(meditationSessionId, userMeditationRecord.MeditationSessionId);
            Assert.Equal(duration, userMeditationRecord.Duration);
            Assert.NotEqual(Guid.Empty, userMeditationRecord.Id);
            Assert.True(userMeditationRecord.SessionDate <= DateTime.UtcNow);
        }
    }
}