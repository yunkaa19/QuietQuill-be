using Domain.Repositories;

namespace Infrastructure.Persistence.Repos;

public class QuizQuestionRepository : IQuizQuestionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public QuizQuestionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}