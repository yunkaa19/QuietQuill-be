using Domain.Entities.HabbitTracker;

namespace ArchitectureTests.EntitiesUnitTests
{
    public class HabitLogTests
    {
        [Fact]
        public void CreateHabitLog_ShouldInitializeProperties()
        {
            // Arrange
            var habitId = Guid.NewGuid();
            var completed = true;
            var notes = "Completed the habit successfully.";

            // Act
            var habitLog = new HabitLog(habitId, completed, notes);

            // Assert
            Assert.Equal(habitId, habitLog.HabitId);
            Assert.Equal(completed, habitLog.Completed);
            Assert.Equal(notes, habitLog.Notes);
            Assert.NotEqual(Guid.Empty, habitLog.Id);
            Assert.True(habitLog.DateLogged <= DateTime.UtcNow);
        }

        [Fact]
        public void CreateHabitLog_ShouldInitializeProperties_WhenNotesIsNull()
        {
            // Arrange
            var habitId = Guid.NewGuid();
            var completed = true;

            // Act
            var habitLog = new HabitLog(habitId, completed);

            // Assert
            Assert.Equal(habitId, habitLog.HabitId);
            Assert.Equal(completed, habitLog.Completed);
            Assert.Null(habitLog.Notes);
            Assert.NotEqual(Guid.Empty, habitLog.Id);
            Assert.True(habitLog.DateLogged <= DateTime.UtcNow);
        }

        [Fact]
        public void UpdateCompletionStatus_ShouldUpdateStatus()
        {
            // Arrange
            var habitLog = new HabitLog(Guid.NewGuid(), false, "Initial log");

            // Act
            habitLog.UpdateCompletionStatus(true);

            // Assert
            Assert.True(habitLog.Completed);
        }

        [Fact]
        public void AddNotes_ShouldUpdateNotes()
        {
            // Arrange
            var habitLog = new HabitLog(Guid.NewGuid(), true, "Initial log");
            var newNotes = "Updated notes for the habit log.";

            // Act
            habitLog.AddNotes(newNotes);

            // Assert
            Assert.Equal(newNotes, habitLog.Notes);
        }

        [Fact]
        public void AddNotes_ShouldThrowArgumentException_WhenNotesIsNullOrEmpty()
        {
            // Arrange
            var habitLog = new HabitLog(Guid.NewGuid(), true, "Initial log");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => habitLog.AddNotes(null));
            Assert.Throws<ArgumentException>(() => habitLog.AddNotes(string.Empty));
        }
    }
}
