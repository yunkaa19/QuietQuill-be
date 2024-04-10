namespace Domain.Entities.HabbitTracker;

public class Habit
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public int TargetFrequency { get; private set; }
    public int CurrentStreak { get; private set; }
    public int LongestStreak { get; private set; }
    public bool isActive { get; private set; }
        
    // Relationships
    //public virtual ICollection<HabitLog> HabitLogs { get; private set; }
        
    // A private constructor for EF
    private Habit() 
    {
        // Used by EF Core
    }

    // Public constructor for creating a new Habit
    public Habit(Guid userId, string name, string description, int targetFrequency)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        StartDate = DateTime.UtcNow;
        TargetFrequency = targetFrequency;
        CurrentStreak = 0;
        LongestStreak = 0;
        //HabitLogs = new List<HabitLog>();
    }

    // Methods for domain logic
    public void UpdateDescription(string newDescription)
    {
        Description = newDescription;
    }
        
    public void RecordCompletion()
    {
        var today = DateTime.UtcNow.Date;
        // Logic to update streaks and logs, considering the last completion date, etc.
        // This may involve checking the HabitLogs to see when the habit was last completed.
    }
        
    // Additional methods to handle frequency changes, reminders, etc.
}