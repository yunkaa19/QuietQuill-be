namespace Domain.Entities.PaperPlane;

public class PaperPlaneEntity
{
    private static int id_seed = 1;
    public int id { get; set; }
    public string content { get; set; }
    public DateTime date { get; set; }
    
    public Guid userId { get; set; }
    
    // Used by EF Core
    public PaperPlaneEntity()
    {
        
    }
    
    public PaperPlaneEntity(string content, Guid userId)
    {
        this.id = id_seed++;
        this.content = content;
        this.date =  DateTime.Now;
        this.userId = userId;
    }
    
    
    
}