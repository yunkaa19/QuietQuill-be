using Domain.Repositories;

namespace Infrastructure.Persistence.Repos;

public class UserMeditationRecordRepository : IUserMeditationRecordRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserMeditationRecordRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}