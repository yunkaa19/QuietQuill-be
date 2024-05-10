using Domain.Entities;

namespace Application.Journals.DTOs;

public class JournalDTO
{
    
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Content { get; set; }
    public string Day { get; set; }
    public string Month { get; set; }
    public string Year { get; set; }
    public Mood Mood { get; set; }
    public string Tags { get; set; }
}