namespace Domain.Entities.Meditation;

public class UserMeditationRecord
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid MeditationSessionId { get; private set; }
    public DateTime SessionDate { get; private set; }
    public TimeSpan Duration { get; private set; }

    private UserMeditationRecord() { }

    public UserMeditationRecord(Guid userId, Guid meditationSessionId, TimeSpan duration)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        MeditationSessionId = meditationSessionId;
        SessionDate = DateTime.UtcNow;
        Duration = duration;
    }
}