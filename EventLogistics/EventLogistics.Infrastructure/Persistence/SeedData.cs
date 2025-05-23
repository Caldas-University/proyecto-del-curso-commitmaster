namespace EventLogistics.Infrastructure.Persistence;

using EventLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public static class SeedData
{
    public static void Initialize(EventLogisticsDbContext context)
    {
        context.Database.EnsureCreated();
            
        // Verifica si ya existen datos
        if (context.Events.Any() || context.Reasignaciones.Any())
        {
            return;   // La base de datos ya tiene datos
        }

        // Crea usuarios de ejemplo
        var organizador = new User("Organizador", "organizador@ejemplo.com");
        var asistente = new User("Asistente", "asistente@ejemplo.com");
        
        context.Users.AddRange(organizador, asistente);

        // Crea recursos de ejemplo
        var sala = new Resource("Sala", 1, true);
        var equipo = new Resource("Equipo", 3, true);
        
        context.Resources.AddRange(sala, equipo);

        // Crea un evento de ejemplo
        var evento = new Event("Sala Principal", DateTime.Now.AddDays(7));
        
        context.Events.Add(evento);

        // Crea actividades de ejemplo - CORREGIDO
        var actividad = new Activity(evento.Id, "Sala A", DateTime.Now.AddDays(7));
        
        context.Activities.Add(actividad);

        // Crea una reasignación de ejemplo
        var reasignacion = new Reasignacion("Cambio de horario");
        
        context.Reasignaciones.Add(reasignacion);

        // Guarda los cambios
        context.SaveChanges();
    }
}
