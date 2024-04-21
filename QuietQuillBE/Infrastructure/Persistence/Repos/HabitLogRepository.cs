using Domain.Repositories;

namespace Infrastructure.Persistence.Repos;

public class HabitLogRepository : IHabitLogRepository
{
    private readonly ApplicationDbContext _dbContext;

    public HabitLogRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}