using EventLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationHistory> NotificationHistories { get; set; }
        public DbSet<ResourceAssignment> ResourceAssignments { get; set; }
        public DbSet<ReassignmentRule> ReassignmentRules { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ParticipantActivity> ParticipantActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User - Notification (1:N)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Recipient)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.RecipientId);

            // Notification - NotificationHistory (1:N)
            modelBuilder.Entity<NotificationHistory>()
                .HasOne(nh => nh.Notification)
                .WithMany()
                .HasForeignKey(nh => nh.NotificationId);

            // Resource - ResourceAssignment (1:N)
            modelBuilder.Entity<ResourceAssignment>()
                .HasOne(ra => ra.Resource)
                .WithMany(r => r.Assignments)
                .HasForeignKey(ra => ra.ResourceId);

            // Event - ResourceAssignment (1:N)
            modelBuilder.Entity<ResourceAssignment>()
                .HasOne(ra => ra.Event)
                .WithMany(e => e.Resources)
                .HasForeignKey(ra => ra.EventId);

            // User - ResourceAssignment (1:N, optional)
            modelBuilder.Entity<ResourceAssignment>()
                .HasOne(ra => ra.AssignedTo)
                .WithMany()
                .HasForeignKey(ra => ra.AssignedToUserId)
                .IsRequired(false);

            // Resource - ReassignmentRule (optional)
            modelBuilder.Entity<ReassignmentRule>()
                .HasOne(rr => rr.ResourceType)
                .WithMany(r => r.ReassignmentRules)
                .HasForeignKey(rr => rr.ResourceTypeId)
                .IsRequired(false);

            // ParticipantActivity (N:N)
            modelBuilder.Entity<ParticipantActivity>()
                .HasKey(pa => new { pa.ParticipantId, pa.ActivityId });

            modelBuilder.Entity<ParticipantActivity>()
                .HasOne(pa => pa.Participant)
                .WithMany(p => p.ParticipantActivities)
                .HasForeignKey(pa => pa.ParticipantId);

            modelBuilder.Entity<ParticipantActivity>()
                .HasOne(pa => pa.Activity)
                .WithMany(a => a.ParticipantActivities)
                .HasForeignKey(pa => pa.ActivityId);

            // Attendance
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Participant)
                .WithMany()
                .HasForeignKey(a => a.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Credential
            modelBuilder.Entity<Credential>()
                .HasOne(c => c.Participant)
                .WithMany()
                .HasForeignKey(c => c.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);
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
    }
}