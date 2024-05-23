using Domain.Entities.Meditation;

namespace Domain.Repositories;

public interface IMeditationSessionRepository
{
    public bool CreateMeditationSession(MeditationSession session);
    public MeditationSession GetMeditationSessionById(string sessionId);
    public List<MeditationSession> GetAllMeditationSessions();
    public List<MeditationSession> GetMeditationSessionsByType(MeditationType type);
    public bool UpdateMeditationSession(MeditationSession session);
    public bool DeleteMeditationSession(string sessionId);
}