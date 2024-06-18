using Domain.Entities.Meditation;

namespace ArchitectureTests.EntitiesUnitTests
{
    public class MeditationSessionTests
    {
        [Fact]
        public void CreateMeditationSession_ShouldInitializeProperties()
        {
            // Arrange
            var title = "Morning Meditation";
            var duration = new TimeSpan(0, 30, 0); // 30 minutes
            var description = "A calming morning meditation session.";
            var type = new MeditationType(); // Assume MeditationType has a parameterless constructor

            // Act
            var meditationSession = new MeditationSession(title, duration, description, type);

            // Assert
            Assert.False(string.IsNullOrEmpty(meditationSession.Id));
            Assert.Equal(title, meditationSession.Title);
            Assert.Equal(duration, meditationSession.Duration);
            Assert.Equal(description, meditationSession.Description);
            Assert.Equal(type, meditationSession.Type);
        }

        [Fact]
        public void CreateMeditationSession_ShouldThrowArgumentNullException_WhenTitleIsNull()
        {
            // Arrange
            string title = null;
            var duration = new TimeSpan(0, 30, 0);
            var description = "A calming morning meditation session.";
            var type = new MeditationType();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MeditationSession(title, duration, description, type));
        }

        [Fact]
        public void UpdateDescription_ShouldUpdateDescription()
        {
            // Arrange
            var meditationSession = new MeditationSession("Morning Meditation", new TimeSpan(0, 30, 0), "A calming morning meditation session.", new MeditationType());
            var newDescription = "An updated description for the meditation session.";

            // Act
            meditationSession.UpdateDescription(newDescription);

            // Assert
            Assert.Equal(newDescription, meditationSession.Description);
        }

        [Fact]
        public void UpdateDescription_ShouldThrowArgumentNullException_WhenNewDescriptionIsNull()
        {
            // Arrange
            var meditationSession = new MeditationSession("Morning Meditation", new TimeSpan(0, 30, 0), "A calming morning meditation session.", new MeditationType());

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => meditationSession.UpdateDescription(null));
        }
    }
}
