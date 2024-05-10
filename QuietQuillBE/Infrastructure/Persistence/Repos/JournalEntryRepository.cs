using Domain.Entities;
using Domain.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Persistence.Repos;

public class JournalEntryRepository : IJournalEntryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public JournalEntryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public bool CreateJournalEntry(JournalEntry journalEntry)
    {
        try
        {
            var JournalEntry = _dbContext.JournalEntries.Add(journalEntry);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public JournalEntry GetJournalEntryById(string journalEntryId)
    {
        var journalEntry = _dbContext.JournalEntries.Find(journalEntryId);
        if (journalEntry == null)
        {
            throw new Exception($"No journal entry found with ID {journalEntryId}");
        }
        return journalEntry;
    }

    public List<JournalEntry> GetJournalEntriesByUserId(int userId)
    {
        List<JournalEntry> journalEntries = _dbContext.Users.Find(userId).JournalEntries.ToList();
        if (journalEntries.IsNullOrEmpty())
        {
            throw new Exception($"No journal entries found for user with ID {userId}");
        }
        return journalEntries;
    }

    public bool UpdateJournalEntry(JournalEntry journalEntry)
    {
        try
        {
            _dbContext.JournalEntries.Update(journalEntry);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public bool DeleteJournalEntry(string EntryId)
    {
        try
        {
            var journalEntry = _dbContext.JournalEntries.Find(EntryId);
            if (journalEntry == null)
            {
                throw new Exception($"No journal entry found with ID {EntryId}");
            }
            _dbContext.JournalEntries.Remove(journalEntry);
            return true; 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }   
    }
}