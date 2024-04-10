namespace Domain.Entities.MentalHealthSupport;

public class QuizQuestion
{
    public Guid Id { get; private set; }
    public Guid QuizId { get; private set; }
    public string Text { get; private set; }
    public List<string> Options { get; private set; }
    public int CorrectAnswerIndex { get; private set; }

    // You might want a separate class for this or store it as a simple index

    public QuizQuestion(string text, List<string> options, int correctAnswerIndex)
    {
        Id = Guid.NewGuid();
        Text = text ?? throw new ArgumentNullException(nameof(text));
        Options = options ?? throw new ArgumentNullException(nameof(options));
        CorrectAnswerIndex = correctAnswerIndex;
    }

    // ... any additional logic or properties
}