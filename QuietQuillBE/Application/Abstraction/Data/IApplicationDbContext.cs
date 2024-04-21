using Domain.Entities;
using Domain.Entities.HabbitTracker;
using Domain.Entities.Meditation;
using Domain.Entities.MentalHealthSupport;
using Microsoft.EntityFrameworkCore;


namespace Domain.Abstraction;

public interface IApplicationDbContext
{ 
    DbSet<User> Users { get; set; }
    DbSet<JournalEntry> JournalEntries { get; set; }
    DbSet<Reminder> Reminders { get; set; }
    DbSet<Habit> Habits { get; set; }
    DbSet<MeditationSession> MeditationSessions { get; set; }
    DbSet<UserQuizRecord> UserQuizRecords { get; set; }
}