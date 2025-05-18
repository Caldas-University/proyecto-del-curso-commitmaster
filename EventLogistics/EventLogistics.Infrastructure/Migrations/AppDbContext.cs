
using System;
using Microsoft.EntityFrameworkCore;
using EventLogistics.EventLogistics.Domain.Entities;


namespace EventLogistics.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet para cada entidad
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Space> Spaces { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relaciones (opcional: puedes agregar validaciones adicionales aquí si las necesitas)
            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Activity)
                .WithMany(a => a.Incidents)
                .HasForeignKey(i => i.ActivityId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Space)
                .WithMany(s => s.Incidents)
                .HasForeignKey(i => i.SpaceId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Equipment)
                .WithMany(e => e.Incidents)
                .HasForeignKey(i => i.EquipmentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Supplier)
                .WithMany(s => s.Incidents)
                .HasForeignKey(i => i.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
