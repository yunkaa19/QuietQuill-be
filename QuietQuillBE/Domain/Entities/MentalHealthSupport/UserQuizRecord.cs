namespace Domain.Entities.MentalHealthSupport;

public class UserQuizRecord
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid QuizId { get; private set; }
    public DateTime CompletedOn { get; private set; }
    public int Score { get; private set; }

    private UserQuizRecord() { }

    public UserQuizRecord(Guid userId, Guid quizId, int score)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        QuizId = quizId;
        CompletedOn = DateTime.UtcNow;
        Score = score;
    }
}