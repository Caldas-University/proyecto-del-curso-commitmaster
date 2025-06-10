namespace EventLogistics.Infrastructure.Persistence;

using EventLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class EventLogisticsDbContext : DbContext
{
    public DbSet<Activity> Activities { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Reasignacion> Reasignaciones { get; set; } = null!;

    public DbSet<Incident> Incidents { get; set; } = null!;
    public DbSet<IncidentSolution> IncidentSolutions { get; set; } = null!;
    public DbSet<Resource> Resources { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Participant> Participants { get; set; } = null!;
    public DbSet<Attendance> Attendances { get; set; } = null!;
    public DbSet<ParticipantActivity> ParticipantActivities { get; set; } = null!;

    public EventLogisticsDbContext(DbContextOptions<EventLogisticsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración para User.Preferences (Dictionary) - CORREGIDO
        modelBuilder.Entity<User>()
            .Property(u => u.Preferences)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null) ?? new Dictionary<string, string>());
    }
}
