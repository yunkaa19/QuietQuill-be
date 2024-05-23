using System.Text.RegularExpressions;

namespace Domain.Entities.Meditation;

public class MeditationSession
{
        public string Id { get; private set; }
        public string Title { get; private set; }
        public TimeSpan Duration { get; private set; }
        public string Description { get; private set; }
        public MeditationType Type { get; private set; }
        
        // A private constructor for EF
        private MeditationSession()
        {
            // Used by EF Core
        }

        // Public constructor for creating a new MeditationSession
        public MeditationSession(string title, TimeSpan duration, string description, MeditationType type)
        {
            Id = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Duration = duration;
            Description = description;
            Type = type;
        }


        public void UpdateDescription(string newDescription)
        {
            Description = newDescription ?? throw new ArgumentNullException(nameof(newDescription));
        }

        
}