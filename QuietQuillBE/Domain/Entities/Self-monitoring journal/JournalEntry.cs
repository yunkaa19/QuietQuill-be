using System.Text.RegularExpressions;

namespace Domain.Entities;

public class JournalEntry
{
    public string Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Content { get;  set; }
    public DateOnly EntryDate { get; set; }
    public Mood Mood { get;  set; }
    public string Tags { get;  set; }

    // A private constructor for EF
    private JournalEntry()
    {
        // Used by EF Core
    }

    // Public constructor for creating a new JournalEntry
    public JournalEntry(Guid userId, string content, DateOnly entryDate, Mood mood, string tags = null)
    {
        Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        UserId = userId;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        EntryDate = entryDate;
        Mood = mood;
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
        Mood = newMood;
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
