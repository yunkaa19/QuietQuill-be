using Domain.Entities.PaperPlane;
using Domain.Exceptions.PaperPlane;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repos;

public class PaperPlaneRepository : IPaperPlaneRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public PaperPlaneRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddPaperPlane(PaperPlaneEntity paperPlaneEntity)
    {
        await _dbContext.AddAsync(paperPlaneEntity);
        await _dbContext.SaveChangesAsync();
        
    }

    public async Task<PaperPlaneEntity> GetPaperPlaneById(int id)
    {
        var paperPlane = await _dbContext.PaperPlanes.FirstOrDefaultAsync(p => p.id == id);
        if (paperPlane  == null)
        {
            throw new PaperPlaneNotFoundException(id);
        }
        return paperPlane;
    }

    public async Task<bool> HasSentOneRecently(Guid userId)
    {
        var lastPaperPlane = await _dbContext.PaperPlanes.OrderByDescending(p => p.date).FirstOrDefaultAsync(p => p.userId == userId);
        if (lastPaperPlane != null)
        {
            if (DateTime.Now - lastPaperPlane.date < TimeSpan.FromMinutes(10))
                return true;
        }
        return false;
    }
    

    public async Task<PaperPlaneEntity> GetLatestPaperPlane()
    {
        var paperPlane = await _dbContext.PaperPlanes.OrderByDescending(p => p.date).FirstOrDefaultAsync();
        if (paperPlane == null)
        {
            throw new PaperPlaneNotFoundException();
        }
        return paperPlane;
        
    }
}