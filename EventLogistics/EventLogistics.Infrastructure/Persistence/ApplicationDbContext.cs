using EventLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Organizator> Organizators { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationHistory> NotificationHistories { get; set; }
        public DbSet<ResourceAssignment> ResourceAssignments { get; set; }
        public DbSet<ReassignmentRule> ReassignmentRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships and constraints

            // User - Notification relationship (one-to-many)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Recipient)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.RecipientId);

            // Notification - NotificationHistory relationship (one-to-many)
            modelBuilder.Entity<NotificationHistory>()
                .HasOne(nh => nh.Notification)
                .WithMany()
                .HasForeignKey(nh => nh.NotificationId);

            // Resource - ResourceAssignment relationship (one-to-many)
            modelBuilder.Entity<ResourceAssignment>()
                .HasOne(ra => ra.Resource)
                .WithMany(r => r.Assignments)
                .HasForeignKey(ra => ra.ResourceId);

            // Event - ResourceAssignment relationship (one-to-many)
            modelBuilder.Entity<ResourceAssignment>()
                .HasOne(ra => ra.Event)
                .WithMany(e => e.Resources)
                .HasForeignKey(ra => ra.EventId);

            // User - ResourceAssignment relationship (one-to-many, optional)
            modelBuilder.Entity<ResourceAssignment>()
                .HasOne(ra => ra.AssignedTo)
                .WithMany()
                .HasForeignKey(ra => ra.AssignedToUserId)
                .IsRequired(false);

            // Activity - ResourceAssingment relationship (one-to-many)
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.Organizator)
                .WithMany(o => o.Activities)
                .HasForeignKey(a => a.OrganizatorId);

            // Resource - ReassignmentRule relationship (optional)
            modelBuilder.Entity<ReassignmentRule>()
                .HasOne(rr => rr.ResourceType)
                .WithMany(r => r.ReassignmentRules)
                .HasForeignKey(rr => rr.ResourceTypeId)
                .IsRequired(false);

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.Property(r => r.Type).HasColumnName("TipoEquipo");
                entity.Property(r => r.Capacity).HasColumnName("Cantidad");
                entity.Property(r => r.FechaInicio);
                entity.Property(r => r.FechaFin);
                entity.Property(r => r.Tags).HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            });

            modelBuilder.Entity<ReassignmentRule>(entity =>
            {
                entity.Property(r => r.Priority).HasDefaultValue(1);
                entity.Property(r => r.IsActive).HasDefaultValue(true);
                entity.Property(r => r.SimilarityThreshold).HasDefaultValue(0.5);
            });
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                else
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configuración para tiempo de diseño (migrations)
                optionsBuilder.UseSqlite("Data Source=EventLogistics.db");
            }
        }
    }
}