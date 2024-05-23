using Domain.Entities.Meditation;

namespace Application.Meditation.DTOs;

public record MeditationSessionDTO
{
    public string Id { get; set; }
    public string Title { get; set; }
    public TimeSpan Duration { get; set; }
    public string Description { get; set; }
    public MeditationType Type { get; set; }
}
