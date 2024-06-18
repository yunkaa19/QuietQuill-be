using System;
using System.Collections.Generic;
using Xunit;
using Domain.Entities.MentalHealthSupport;

namespace ArchitectureTests.EntitiesUnitTests
{
    public class QuizQuestionTests
    {
        [Fact]
        public void CreateQuizQuestion_ShouldInitializeProperties()
        {
            // Arrange
            var text = "What is the capital of France?";
            var options = new List<string> { "Paris", "London", "Berlin", "Madrid" };
            var correctAnswerIndex = 0;

            // Act
            var quizQuestion = new QuizQuestion(text, options, correctAnswerIndex);

            // Assert
            Assert.Equal(text, quizQuestion.Text);
            Assert.Equal(options, quizQuestion.Options);
            Assert.Equal(correctAnswerIndex, quizQuestion.CorrectAnswerIndex);
            Assert.NotEqual(Guid.Empty, quizQuestion.Id);
        }

        [Fact]
        public void CreateQuizQuestion_ShouldThrowArgumentNullException_WhenTextIsNull()
        {
            // Arrange
            string text = null;
            var options = new List<string> { "Option1", "Option2" };
            var correctAnswerIndex = 0;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new QuizQuestion(text, options, correctAnswerIndex));
        }

        [Fact]
        public void CreateQuizQuestion_ShouldThrowArgumentNullException_WhenOptionsIsNull()
        {
            // Arrange
            var text = "Sample question";
            List<string> options = null;
            var correctAnswerIndex = 0;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new QuizQuestion(text, options, correctAnswerIndex));
        }

        [Fact]
        public void CreateQuizQuestion_ShouldThrowArgumentOutOfRangeException_WhenCorrectAnswerIndexIsOutOfRange()
        {
            // Arrange
            var text = "Sample question";
            var options = new List<string> { "Option1", "Option2" };
            var correctAnswerIndex = -1; // Invalid index

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new QuizQuestion(text, options, correctAnswerIndex));

            correctAnswerIndex = options.Count; // Invalid index

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new QuizQuestion(text, options, correctAnswerIndex));
        }

        [Fact]
        public void CreateQuizQuestion_ShouldThrowArgumentOutOfRangeException_WhenOptionsIsEmpty()
        {
            // Arrange
            var text = "Sample question";
            var options = new List<string>(); // Empty options list
            var correctAnswerIndex = 0;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new QuizQuestion(text, options, correctAnswerIndex));
        }
    }
}
