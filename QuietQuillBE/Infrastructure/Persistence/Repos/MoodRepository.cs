using Domain.Repositories;

namespace Infrastructure.Persistence.Repos;

public class MoodRepository : IMoodRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MoodRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}