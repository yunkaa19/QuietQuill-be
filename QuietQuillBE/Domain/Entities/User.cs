using Domain.Entities.HabbitTracker;
using Domain.Entities.Meditation;
using Domain.Entities.MentalHealthSupport;

namespace Domain.Entities;

public class User
{
    #region Properties
   public  Guid UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public string Role { get; protected set; }
    
    public string IdentityID { get; set; }
    #endregion

    #region Relationships

    public virtual ICollection<JournalEntry> JournalEntries { get; private set; }
    public virtual ICollection<Reminder> Reminders { get; private set; }
    public virtual ICollection<Habit> Habits { get; private set; }
    public virtual ICollection<MeditationSession> MeditationSessions { get; private set; }
    public virtual ICollection<UserQuizRecord> UserQuizRecords { get; private set; }
    #endregion
    
    #region Constructors

    protected User()
    {
        
    }
    public User(Guid guid, string username, string passwordHash, string email, string identityId)
    {
        UserId = guid;
        
        Username = username;
        PasswordHash = passwordHash;
        Email = email;
        
        Role = "User";
        
        this.IdentityID = identityId;
        JournalEntries = new List<JournalEntry>();
        Reminders = new List<Reminder>();
        Habits = new List<Habit>();
        MeditationSessions = new List<MeditationSession>();
        UserQuizRecords = new List<UserQuizRecord>();
        
    }
    #endregion

    #region Methods

    
    public void UpdateIdentityId(string identityId)
    {
        if (identityId == null)
        {
            
            throw new ArgumentNullException(nameof(identityId));
        }
        
        IdentityID = identityId;
    }
    public void setPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
    
    public void addJournalEntry(JournalEntry journalEntry)
    {
        if (journalEntry == null)
        {
            throw new ArgumentNullException();
        }
        JournalEntries.Add(journalEntry);
    }
    
    public void addReminder(Reminder reminder)
    {
        if (reminder == null)
        {
            throw new ArgumentNullException();
        }
        Reminders.Add(reminder);
    }
    
    public void addHabit(Habit habit)
    {
        if (habit == null)
        {
            throw new ArgumentNullException();
        }
        Habits.Add(habit);
    }
    
    public void addMeditationSession(MeditationSession meditationSession)
    {
        if (meditationSession == null)
        {
            throw new ArgumentNullException();
        }
        MeditationSessions.Add(meditationSession);
    }
    
    public void addQuizRecord(UserQuizRecord userQuizRecord)
    {
        if (userQuizRecord == null)
        {
            throw new ArgumentNullException();
        }
        UserQuizRecords.Add(userQuizRecord);
    }
    
    public void removeJournalEntry(JournalEntry journalEntry)
    {
        if (journalEntry == null)
        {
            throw new ArgumentNullException();
        }
        JournalEntries.Remove(journalEntry);
    }
    
    public void removeReminder(Reminder reminder)
    {
        if (reminder == null)
        {
            throw new ArgumentNullException();
        }
        Reminders.Remove(reminder);
    }
    
    public void removeHabit(Habit habit)
    {
        if (habit == null)
        {
            throw new ArgumentNullException();
        }
        Habits.Remove(habit);
    }
    
    public void removeMeditationSession(MeditationSession meditationSession)
    {
        if (meditationSession == null)
        {
            throw new ArgumentNullException();
        }
        MeditationSessions.Remove(meditationSession);
    }   
    
    public void removeQuizRecord(UserQuizRecord userQuizRecord)
    {
        if (userQuizRecord == null)
        {
            throw new ArgumentNullException();
        }
        UserQuizRecords.Remove(userQuizRecord);
    }
    
    
    
    #endregion
}