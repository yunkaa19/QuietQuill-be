using Domain.Repositories;

namespace Infrastructure.Persistence.Repos;

public class QuizRepository : IQuizRepository
{
    private readonly ApplicationDbContext _dbContext;

    public QuizRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}