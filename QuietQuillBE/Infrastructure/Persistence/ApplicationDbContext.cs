using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Entities.HabbitTracker;
using Domain.Entities.Meditation;
using Domain.Entities.MentalHealthSupport;


namespace Infrastructure.Persistence;
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<MeditationSession> MeditationSessions { get; set; }
        public DbSet<UserQuizRecord> UserQuizRecords { get; set; }
        
    }

        