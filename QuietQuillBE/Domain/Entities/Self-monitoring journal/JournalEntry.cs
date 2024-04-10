namespace Domain.Entities;

public class JournalEntry
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Content { get; private set; }
    public DateTime EntryDate { get; private set; }
    public Mood Mood { get; private set; }
    public string Tags { get; private set; }

    // A private constructor for EF
    private JournalEntry()
    {
        // Used by EF Core
    }

    // Public constructor for creating a new JournalEntry
    public JournalEntry(Guid userId, string content, DateTime entryDate, Mood mood, string tags = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        EntryDate = entryDate;
        Mood = mood ?? throw new ArgumentNullException(nameof(mood));
        Tags = tags; // handle tags appropriately (e.g., parse if string or directly assign if collection)
    }

    // Additional domain behaviors and methods could include updating the content, mood, or tags.
    public void UpdateContent(string newContent)
    {
        if (string.IsNullOrEmpty(newContent))
        {
            throw new ArgumentException("Content cannot be null or empty.", nameof(newContent));
        }

        Content = newContent;
    }

    public void UpdateMood(Mood newMood)
    {
        Mood = newMood ?? throw new ArgumentNullException(nameof(newMood));
    }

    public void AddTag(string tag)
    {
        if (string.IsNullOrEmpty(tag))
        {
            throw new ArgumentException("Tag cannot be null or empty.", nameof(tag));
        }

        if (string.IsNullOrEmpty(Tags))
        {
            Tags = tag;
        }
        else
        {
            Tags = $"{Tags},{tag}";
        }
    }
        
}
