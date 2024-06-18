using Domain.Entities;
using Domain.Entities.HabbitTracker;
using Domain.Entities.Meditation;
using Domain.Entities.MentalHealthSupport;

namespace ArchitectureTests.EntitiesUnitTests
{
    public class UserTests
    {
        [Fact]
        public void CreateUser_ShouldInitializeProperties()
        {
            // Arrange
            var id = Guid.NewGuid();
            var username = "TestUser";
            var passwordHash = "hashedpassword";
            var email = "test.user@example.com";
            var identityId = "identity123";

            // Act
            var user = new User(id, username, passwordHash, email, identityId);

            // Assert
            Assert.Equal(id, user.UserId);
            Assert.Equal(username, user.Username);
            Assert.Equal(passwordHash, user.PasswordHash);
            Assert.Equal(email, user.Email);
            Assert.Equal("User", user.Role);
            Assert.Equal(identityId, user.IdentityID);
            Assert.Empty(user.JournalEntries);
            Assert.Empty(user.Reminders);
            Assert.Empty(user.Habits);
            Assert.Empty(user.MeditationSessions);
            Assert.Empty(user.UserQuizRecords);
        }

        [Fact]
        public void UpdateIdentityId_ShouldUpdateIdentityId()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");
            var newIdentityId = "newidentity123";

            // Act
            user.UpdateIdentityId(newIdentityId);

            // Assert
            Assert.Equal(newIdentityId, user.IdentityID);
        }

        [Fact]
        public void UpdateIdentityId_ShouldThrowArgumentNullException_WhenIdentityIdIsNull()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => user.UpdateIdentityId(null));
        }

        [Fact]
        public void SetPassword_ShouldUpdatePasswordHash()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");
            var newPasswordHash = "newhashedpassword";

            // Act
            user.setPassword(newPasswordHash);

            // Assert
            Assert.Equal(newPasswordHash, user.PasswordHash);
        }

        [Fact]
        public void AddJournalEntry_ShouldAddEntryToJournalEntries()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");
            var journalEntry = new JournalEntry(user.UserId, "Test content", DateOnly.FromDateTime(DateTime.Now), Mood.HAPPY);

            // Act
            user.addJournalEntry(journalEntry);

            // Assert
            Assert.Contains(journalEntry, user.JournalEntries);
        }

        [Fact]
        public void AddReminder_ShouldAddReminderToReminders()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");
            var reminder = new Reminder(user.UserId, DateTime.Now, "Test reminder", false);

            // Act
            user.addReminder(reminder);

            // Assert
            Assert.Contains(reminder, user.Reminders);
        }

        [Fact]
        public void AddHabit_ShouldAddHabitToHabits()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");
            var habit = new Habit(user.UserId, "Test habit", "Test description", 4);

            // Act
            user.addHabit(habit);

            // Assert
            Assert.Contains(habit, user.Habits);
        }

        [Fact]
        public void AddMeditationSession_ShouldAddSessionToMeditationSessions()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");
            var session = new MeditationSession("Test session", TimeSpan.FromMinutes(10), "Test description", MeditationType.Guided);

            // Act
            user.addMeditationSession(session);

            // Assert
            Assert.Contains(session, user.MeditationSessions);
        }

        [Fact]
        public void AddQuizRecord_ShouldAddRecordToUserQuizRecords()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");
            var quizRecord = new UserQuizRecord(user.UserId, Guid.NewGuid(), 10);

            // Act
            user.addQuizRecord(quizRecord);

            // Assert
            Assert.Contains(quizRecord, user.UserQuizRecords);
        }

        [Fact]
        public void RemoveJournalEntry_ShouldRemoveEntryFromJournalEntries()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");
            var journalEntry = new JournalEntry(user.UserId, "Test content", DateOnly.FromDateTime(DateTime.Now), Mood.HAPPY);
            user.addJournalEntry(journalEntry);

            // Act
            user.removeJournalEntry(journalEntry);

            // Assert
            Assert.DoesNotContain(journalEntry, user.JournalEntries);
        }

        [Fact]
        public void RemoveReminder_ShouldRemoveReminderFromReminders()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");
            var reminder = new Reminder(user.UserId, DateTime.Now, "Test reminder", false);
            user.addReminder(reminder);

            // Act
            user.removeReminder(reminder);

            // Assert
            Assert.DoesNotContain(reminder, user.Reminders);
        }

        [Fact]
        public void RemoveHabit_ShouldRemoveHabitFromHabits()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");
            var habit = new Habit(user.UserId, "Test habit", "Test description", 4);
            user.addHabit(habit);

            // Act
            user.removeHabit(habit);

            // Assert
            Assert.DoesNotContain(habit, user.Habits);
        }

        [Fact]
        public void RemoveMeditationSession_ShouldRemoveSessionFromMeditationSessions()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");
            var session = new MeditationSession("Test session", TimeSpan.FromMinutes(10), "Test description", MeditationType.Guided);

            user.addMeditationSession(session);

            // Act
            user.removeMeditationSession(session);

            // Assert
            Assert.DoesNotContain(session, user.MeditationSessions);
        }

        [Fact]
        public void RemoveQuizRecord_ShouldRemoveRecordFromUserQuizRecords()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "TestUser", "hashedpassword", "test.user@example.com", "identity123");
            var quizRecord = new UserQuizRecord(user.UserId, Guid.NewGuid(), 10);
            user.addQuizRecord(quizRecord);

            // Act
            user.removeQuizRecord(quizRecord);

            // Assert
            Assert.DoesNotContain(quizRecord, user.UserQuizRecords);
        }
    }
}
