using Domain.Entities.MentalHealthSupport;

namespace ArchitectureTests.EntitiesUnitTests
{
    public class QuizTests
    {
        [Fact]
        public void CreateQuiz_ShouldInitializeProperties()
        {
            // Arrange
            var title = "Sample Quiz Title";
            var description = "Sample Quiz Description";

            // Act
            var quiz = new Quiz(title, description);

            // Assert
            Assert.Equal(title, quiz.Title);
            Assert.Equal(description, quiz.Description);
            Assert.NotEqual(Guid.Empty, quiz.Id);
            Assert.Empty(quiz.Questions);
        }

        [Fact]
        public void CreateQuiz_ShouldThrowArgumentNullException_WhenTitleIsNull()
        {
            // Arrange
            string title = null;
            var description = "Sample Quiz Description";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Quiz(title, description));
        }

        [Fact]
        public void AddQuestion_ShouldAddQuestionToList()
        {
            // Arrange
            var quiz = new Quiz("Sample Quiz Title", "Sample Quiz Description");
            var question = new QuizQuestion("Sample Question Text", new List<string> { "Option 1", "Option 2" }, 0);

            // Act
            quiz.AddQuestion(question);

            // Assert
            Assert.Contains(question, quiz.Questions);
        }

        [Fact]
        public void AddQuestion_ShouldThrowArgumentNullException_WhenQuestionIsNull()
        {
            // Arrange
            var quiz = new Quiz("Sample Quiz Title", "Sample Quiz Description");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => quiz.AddQuestion(null));
        }

        [Fact]
        public void UpdateTitleAndDescription_ShouldUpdateBothFields()
        {
            // Arrange
            var quiz = new Quiz("Old Title", "Old Description");
            var newTitle = "New Title";
            var newDescription = "New Description";

            // Act
            quiz.UpdateTitleAndDescription(newTitle, newDescription);

            // Assert
            Assert.Equal(newTitle, quiz.Title);
            Assert.Equal(newDescription, quiz.Description);
        }

        [Fact]
        public void UpdateTitleAndDescription_ShouldThrowArgumentNullException_WhenNewTitleIsNull()
        {
            // Arrange
            var quiz = new Quiz("Old Title", "Old Description");
            string newTitle = null;
            var newDescription = "New Description";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => quiz.UpdateTitleAndDescription(newTitle, newDescription));
        }

        [Fact]
        public void UpdateTitleAndDescription_ShouldThrowArgumentNullException_WhenNewDescriptionIsNull()
        {
            // Arrange
            var quiz = new Quiz("Old Title", "Old Description");
            var newTitle = "New Title";
            string newDescription = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => quiz.UpdateTitleAndDescription(newTitle, newDescription));
        }
    }
}
