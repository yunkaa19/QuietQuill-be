using Domain.Entities;

namespace Domain.Repositories;

public interface IJournalEntryRepository
{
    public bool CreateJournalEntry(JournalEntry journalEntry);
    public JournalEntry GetJournalEntryById(int journalEntryId);
    public List<JournalEntry> GetJournalEntriesByUserId(int userId);
    public bool UpdateJournalEntry(JournalEntry journalEntry);
    public bool DeleteJournalEntry(JournalEntry journalEntry);
    
}