using Domain.Entities;

namespace ArchitectureTests.EntitiesUnitTests
{
    public class JournalEntryTests
    {
        [Fact]
        public void CreateJournalEntry_ShouldInitializeProperties()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var content = "This is a test entry.";
            var entryDate = new DateOnly(2023, 6, 19);
            var mood = new Mood(); // Assume Mood has a parameterless constructor
            var tags = "test,entry";

            // Act
            var journalEntry = new JournalEntry(userId, content, entryDate, mood, tags);

            // Assert
            Assert.False(string.IsNullOrEmpty(journalEntry.Id));
            Assert.Equal(userId, journalEntry.UserId);
            Assert.Equal(content, journalEntry.Content);
            Assert.Equal(entryDate, journalEntry.EntryDate);
            Assert.Equal(mood, journalEntry.Mood);
            Assert.Equal(tags, journalEntry.Tags);
        }

        [Fact]
        public void UpdateContent_ShouldUpdateContent()
        {
            // Arrange
            var journalEntry = new JournalEntry(Guid.NewGuid(), "Old content", new DateOnly(2023, 6, 19), new Mood(), "tag1");
            var newContent = "Updated content";

            // Act
            journalEntry.UpdateContent(newContent);

            // Assert
            Assert.Equal(newContent, journalEntry.Content);
        }

        [Fact]
        public void UpdateContent_ShouldThrowArgumentException_WhenNewContentIsNullOrEmpty()
        {
            // Arrange
            var journalEntry = new JournalEntry(Guid.NewGuid(), "Old content", new DateOnly(2023, 6, 19), new Mood(), "tag1");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => journalEntry.UpdateContent(null));
            Assert.Throws<ArgumentException>(() => journalEntry.UpdateContent(string.Empty));
        }

        [Fact]
        public void UpdateMood_ShouldUpdateMood()
        {
            // Arrange
            var journalEntry = new JournalEntry(Guid.NewGuid(), "Test content", new DateOnly(2023, 6, 19), new Mood(), "tag1");
            var newMood = new Mood(); // Assume Mood has a parameterless constructor

            // Act
            journalEntry.UpdateMood(newMood);

            // Assert
            Assert.Equal(newMood, journalEntry.Mood);
        }

        [Fact]
        public void AddTag_ShouldAddNewTag()
        {
            // Arrange
            var journalEntry = new JournalEntry(Guid.NewGuid(), "Test content", new DateOnly(2023, 6, 19), new Mood(), "tag1");
            var newTag = "tag2";

            // Act
            journalEntry.AddTag(newTag);

            // Assert
            Assert.Equal("tag1,tag2", journalEntry.Tags);
        }

        [Fact]
        public void AddTag_ShouldThrowArgumentException_WhenTagIsNullOrEmpty()
        {
            // Arrange
            var journalEntry = new JournalEntry(Guid.NewGuid(), "Test content", new DateOnly(2023, 6, 19), new Mood(), "tag1");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => journalEntry.AddTag(null));
            Assert.Throws<ArgumentException>(() => journalEntry.AddTag(string.Empty));
        }
    }
}
