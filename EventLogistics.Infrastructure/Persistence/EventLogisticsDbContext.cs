using EventLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Persistence;

public class EventLogisticsDbContext : DbContext
{    public DbSet<Activity> Activities { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Reasignacion> Reasignaciones { get; set; } = null!;
    public DbSet<Incident> Incidents { get; set; } = null!;
    public DbSet<IncidentSolution> IncidentSolutions { get; set; } = null!;
    public DbSet<Resource> Resources { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Participant> Participants { get; set; } = null!;
    public DbSet<Attendance> Attendances { get; set; } = null!;
    public DbSet<ParticipantActivity> ParticipantActivities { get; set; } = null!;
    public DbSet<ResourceAssignment> ResourceAssignments { get; set; } = null!;
    public DbSet<ReassignmentRule> ReassignmentRules { get; set; } = null!;
    public DbSet<NotificationHistory> NotificationHistories { get; set; } = null!;
    public DbSet<Organizator> Organizators { get; set; } = null!;

    public EventLogisticsDbContext(DbContextOptions<EventLogisticsDbContext> options) : base(options)
    {
    }    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Activity - Organizator relationship (many-to-one)
        modelBuilder.Entity<Activity>()
            .HasOne(a => a.Organizator)
            .WithMany(o => o.Activities)
            .HasForeignKey(a => a.OrganizatorId);

        // Event - Location relationship (many-to-one)
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Location)
            .WithMany(l => l.Events)
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Resource - ReassignmentRule relationship (optional)
        modelBuilder.Entity<ReassignmentRule>()
            .HasOne(rr => rr.ResourceType)
            .WithMany(r => r.ReassignmentRules)
            .HasForeignKey(rr => rr.ResourceTypeId)
            .IsRequired(false);

        // Configuración de Resource
        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Capacity).IsRequired();
            entity.Property(e => e.Availability).IsRequired();
            
            // Configurar Assignments como JSON - es una List<Guid>, no IEnumerable<ResourceAssignment>
            entity.Property(e => e.Assignments)
                .HasConversion(
                    v => string.Join(',', v.Select(id => id.ToString())),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .Select(id => Guid.Parse(id))
                          .ToList()
                );
        });

        // Configuración de Notification
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Timestamp).IsRequired();
            
            // Configurar la relación usando RecipientId
            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(n => n.RecipientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
