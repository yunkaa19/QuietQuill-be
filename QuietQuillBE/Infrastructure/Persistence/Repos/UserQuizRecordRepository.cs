using Domain.Repositories;

namespace Infrastructure.Persistence.Repos;

public class UserQuizRecordRepository : IUserQuizRecordRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserQuizRecordRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }   
}