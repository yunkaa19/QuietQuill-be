using Domain.Entities.Meditation;
using Domain.Repositories;

namespace Infrastructure.Persistence.Repos;

public class MeditationSessionRepository : IMeditationSessionRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public MeditationSessionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool CreateMeditationSession(MeditationSession session)
    {
        _dbContext.MeditationSessions.Add(session);
        return _dbContext.SaveChanges() > 0;
    }

    public MeditationSession GetMeditationSessionById(string sessionId)
    {
        MeditationSession session = _dbContext.MeditationSessions.Find(sessionId);
        if (session == null)
        {
            throw new Exception("Meditation session not found");
        }
        return session;
    }

    public List<MeditationSession> GetAllMeditationSessions()
    {
        List<MeditationSession> sessions = _dbContext.MeditationSessions.ToList();
        if (sessions.Count == 0)
        {
            throw new Exception("No meditation sessions found");
        }
        return sessions;
        
    }

    public List<MeditationSession> GetMeditationSessionsByType(MeditationType type)
    {
        List<MeditationSession> sessions = _dbContext.MeditationSessions.Where(s => s.Type == type).ToList();
            if (sessions.Count == 0)
            {
                throw new Exception("No meditation sessions found");
            }
        return sessions;
    }

    public bool UpdateMeditationSession(MeditationSession session)
    {
        _dbContext.MeditationSessions.Update(session);
        return _dbContext.SaveChanges() > 0;
    }

    public bool DeleteMeditationSession(string sessionId)
    {
        _dbContext.MeditationSessions.Remove(GetMeditationSessionById(sessionId));
        return _dbContext.SaveChanges() > 0;
    }
    
    
}