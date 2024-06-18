using Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Entities.HabbitTracker;
using Domain.Entities.Meditation;
using Domain.Entities.MentalHealthSupport;
using Domain.Entities.PaperPlane;
using Domain.Primitives;
using MediatR;


namespace Infrastructure.Persistence;
    public class ApplicationDbContext : DbContext, IUnitOfWork, IApplicationDbContext
    {
        
        private readonly IPublisher _publisher;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
            : base(options)
        {
            _publisher = publisher;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<MeditationSession> MeditationSessions { get; set; }
        public DbSet<UserQuizRecord> UserQuizRecords { get; set; }
        public DbSet<PaperPlaneEntity> PaperPlanes { get; set; }
        
        
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var domainEvents = ChangeTracker.Entries<Entity>()
                .Select(e => e.Entity)
                .Where(e => e.GetDomainEvents().Any())
                .SelectMany(e =>
                {
                    var domainEvents = e.GetDomainEvents();

                    e.ClearDomainEvents();

                    return domainEvents;
                })
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);
        
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return result;
        }
    }

        