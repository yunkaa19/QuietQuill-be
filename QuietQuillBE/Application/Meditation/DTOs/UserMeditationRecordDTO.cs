namespace Application.Meditation.DTOs;

public record UserMeditationRecordDTO
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string MeditationSessionId { get; set; }
    public DateOnly SessionDate { get; set; }
    public TimeSpan Duration { get; set; }
}