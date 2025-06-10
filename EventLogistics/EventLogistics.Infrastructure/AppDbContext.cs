// Ruta esperada: EventLogistics.Infrastructure/Persistence/ApplicationDbContext.cs

using Microsoft.EntityFrameworkCore;
using EventLogistics.Domain.Entities;

namespace EventLogistics.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Event> Events { get; set; } 
        public DbSet<IncidentSolution> IncidentSolutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Event)
                .WithMany(e => e.Incidents)
                .HasForeignKey(i => i.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
