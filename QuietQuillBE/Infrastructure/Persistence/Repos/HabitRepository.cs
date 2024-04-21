using Domain.Repositories;

namespace Infrastructure.Persistence.Repos;

public class HabitRepository : IHabitRepository
{
    private readonly ApplicationDbContext _dbContext;

    public HabitRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}