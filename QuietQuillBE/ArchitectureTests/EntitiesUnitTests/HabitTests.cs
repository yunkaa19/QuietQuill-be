using Domain.Entities.HabbitTracker;

namespace ArchitectureTests.EntitiesUnitTests
{
    public class HabitTests
    {
        [Fact]
        public void CreateHabit_ShouldInitializeProperties()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var name = "Daily Exercise";
            var description = "Exercise for at least 30 minutes every day.";
            var targetFrequency = 7;

            // Act
            var habit = new Habit(userId, name, description, targetFrequency);

            // Assert
            Assert.Equal(userId, habit.UserId);
            Assert.Equal(name, habit.Name);
            Assert.Equal(description, habit.Description);
            Assert.Equal(targetFrequency, habit.TargetFrequency);
            Assert.Equal(0, habit.CurrentStreak);
            Assert.Equal(0, habit.LongestStreak);
            Assert.NotEqual(Guid.Empty, habit.Id);
            Assert.True(habit.StartDate <= DateTime.UtcNow);
        }

        [Fact]
        public void CreateHabit_ShouldThrowArgumentNullException_WhenNameIsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            string name = null;
            var description = "Exercise for at least 30 minutes every day.";
            var targetFrequency = 7;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Habit(userId, name, description, targetFrequency));
        }

        [Fact]
        public void UpdateDescription_ShouldUpdateDescription()
        {
            // Arrange
            var habit = new Habit(Guid.NewGuid(), "Daily Exercise", "Exercise for at least 30 minutes every day.", 7);
            var newDescription = "Updated description for the habit.";

            // Act
            habit.UpdateDescription(newDescription);

            // Assert
            Assert.Equal(newDescription, habit.Description);
        }
    }
}