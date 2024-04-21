using Domain.Repositories;

namespace Infrastructure.Persistence.Repos;

public class ReminderRepository : IReminderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ReminderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}