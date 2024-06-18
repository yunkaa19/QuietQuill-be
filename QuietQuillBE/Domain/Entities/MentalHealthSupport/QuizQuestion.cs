using System;
using System.Collections.Generic;

namespace Domain.Entities.MentalHealthSupport
{
    public class QuizQuestion
    {
        public Guid Id { get; private set; }
        public Guid QuizId { get; private set; }
        public string Text { get; private set; }
        public List<string> Options { get; private set; }
        public int CorrectAnswerIndex { get; private set; }

        public QuizQuestion(string text, List<string> options, int correctAnswerIndex)
        {
            Id = Guid.NewGuid();
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Options = options ?? throw new ArgumentNullException(nameof(options));

            if (options.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(options), "Options list cannot be empty.");

            if (correctAnswerIndex < 0 || correctAnswerIndex >= options.Count)
                throw new ArgumentOutOfRangeException(nameof(correctAnswerIndex), "CorrectAnswerIndex is out of range.");

            CorrectAnswerIndex = correctAnswerIndex;
        }

        // ... any additional logic or properties
    }
}