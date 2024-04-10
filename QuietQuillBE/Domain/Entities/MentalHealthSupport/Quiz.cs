namespace Domain.Entities.MentalHealthSupport;

public class Quiz
{
    public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public List<QuizQuestion> Questions { get; private set; }

        // A private constructor for EF
        private Quiz()
        {
            // Used by EF Core
        }

        // Public constructor for creating a new Quiz
        public Quiz(string title, string description)
        {
            Id = Guid.NewGuid();
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description;
            Questions = new List<QuizQuestion>();
        }

        // Methods for domain logic
        public void AddQuestion(QuizQuestion question)
        {
            if (question == null)
            {
                throw new ArgumentNullException(nameof(question));
            }

            Questions.Add(question);
        }

        public void UpdateTitleAndDescription(string newTitle, string newDescription)
        {
            Title = newTitle ?? throw new ArgumentNullException(nameof(newTitle));
            Description = newDescription ?? throw new ArgumentNullException(nameof(newDescription));
        }

        // ... other methods as needed
    }

    
