using Domain.Entities.PaperPlane;

namespace Domain.Repositories;

public interface IPaperPlaneRepository
{
  public Task AddPaperPlane(PaperPlaneEntity paperPlaneEntity);
  
  public Task<bool> HasSentOneRecently(Guid userId);
  public Task<PaperPlaneEntity> GetPaperPlaneById(int id);
  public Task<PaperPlaneEntity> GetLatestPaperPlane();
  
  
}