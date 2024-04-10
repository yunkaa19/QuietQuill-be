namespace Domain.Entities.Meditation;

public class MeditationSession
{
    public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Title { get; private set; }
        public TimeSpan Duration { get; private set; }
        public string Description { get; private set; }
        public MeditationType Type { get; private set; } // Could be an enum or a string
        public DateTime SessionDate { get; private set; }
        
        // Relationships - Example: tracking the user's meditation records
        // public virtual User User { get; private set; }
        // public virtual ICollection<UserMeditationRecord> UserMeditationRecords { get; private set; }

        // A private constructor for EF
        private MeditationSession()
        {
            // Used by EF Core
        }

        // Public constructor for creating a new MeditationSession
        public MeditationSession(Guid userId, string title, TimeSpan duration, string description, MeditationType type)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Duration = duration;
            Description = description;
            Type = type;
            SessionDate = DateTime.UtcNow;
            // UserMeditationRecords = new List<UserMeditationRecord>();
        }

        // Additional domain logic could include methods for extending the session duration,
        // updating the description, etc.
        
        public void ExtendSession(TimeSpan additionalTime)
        {
            if (additionalTime <= TimeSpan.Zero)
            {
                throw new ArgumentException("Additional time must be greater than zero.", nameof(additionalTime));
            }

            Duration += additionalTime;
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription ?? throw new ArgumentNullException(nameof(newDescription));
        }

        
}