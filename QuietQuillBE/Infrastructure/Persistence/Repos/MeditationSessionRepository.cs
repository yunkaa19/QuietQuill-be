using Domain.Repositories;

namespace Infrastructure.Persistence.Repos;

public class MeditationSessionRepository : IMeditationSessionRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public MeditationSessionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}