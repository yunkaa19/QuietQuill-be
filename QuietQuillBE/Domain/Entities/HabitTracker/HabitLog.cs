namespace Domain.Entities.HabbitTracker;

public class HabitLog
{
    public Guid Id { get; private set; }
    public Guid HabitId { get; private set; }
    public DateTime DateLogged { get; private set; }
    public bool Completed { get; private set; }
    public string Notes { get; private set; }

    // Relationships - Example: linking back to the Habit
    // public virtual Habit Habit { get; private set; }

    // A private constructor for EF
    private HabitLog() 
    {
        // Used by EF Core
    }

    // Public constructor for creating a new HabitLog entry
    public HabitLog(Guid habitId, bool completed, string notes = null)
    {
        Id = Guid.NewGuid();
        HabitId = habitId;
        DateLogged = DateTime.UtcNow;
        Completed = completed;
        Notes = notes;
    }

    // Methods for domain logic
    public void UpdateCompletionStatus(bool newStatus)
    {
        Completed = newStatus;
    }

    public void AddNotes(string newNotes)
    {
        if (string.IsNullOrEmpty(newNotes))
        {
            throw new ArgumentException("Notes cannot be null or empty.", nameof(newNotes));
        }

        Notes = newNotes;
    }

    // ... other methods as needed
}