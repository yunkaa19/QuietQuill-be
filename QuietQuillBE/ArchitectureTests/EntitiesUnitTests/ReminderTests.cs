using Domain.Entities;

namespace ArchitectureTests.EntitiesUnitTests
{
    public class ReminderTests
    {
        [Fact]
        public void CreateReminder_ShouldInitializeProperties()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var reminderTime = DateTime.Now;
            var message = "This is a reminder.";
            var isRecurring = true;

            // Act
            var reminder = new Reminder(userId, reminderTime, message, isRecurring);

            // Assert
            Assert.Equal(userId, reminder.UserId);
            Assert.Equal(reminderTime, reminder.ReminderTime);
            Assert.Equal(message, reminder.Message);
            Assert.Equal(isRecurring, reminder.IsRecurring);
            Assert.NotEqual(Guid.Empty, reminder.Id);
        }

        [Fact]
        public void CreateReminder_ShouldThrowArgumentNullException_WhenMessageIsNull()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var reminderTime = DateTime.Now;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Reminder(userId, reminderTime, null, true));
        }
    }
}