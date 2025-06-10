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
        context.Resources.AddRange(sala, equipo);        // Crea un evento de ejemplo
        var evento = new Event("Evento de Prueba", "Sala Principal", DateTime.Now.AddDays(7), "Activo");
        context.Events.Add(evento);

        // Crea actividades de ejemplo
        var actividad1 = new Activity(evento.Id, "Sala A", DateTime.Now.AddDays(7)) { Name = "Charla 1", StartTime = DateTime.Now.AddDays(7).AddHours(9), EndTime = DateTime.Now.AddDays(7).AddHours(10) };
        var actividad2 = new Activity(evento.Id, "Sala B", DateTime.Now.AddDays(7)) { Name = "Taller VIP", StartTime = DateTime.Now.AddDays(7).AddHours(11), EndTime = DateTime.Now.AddDays(7).AddHours(12) };
        context.Activities.AddRange(actividad1, actividad2);

        // Crea participantes de ejemplo con distintos tipos de acceso
        var participante1 = new Participant("Juan Pérez", "123456", "juan@correo.com", "Asistente");
        var participante2 = new Participant("Ana Gómez", "654321", "ana@correo.com", "VIP");
        var participante3 = new Participant("Carlos Ruiz", "789012", "carlos@correo.com", "Ponente");
        context.Participants.AddRange(participante1, participante2, participante3);

        // Inscribe participantes en actividades
        var pa1 = new ParticipantActivity(participante1.Id, actividad1.Id, actividad1); // Juan en Charla 1
        var pa2 = new ParticipantActivity(participante2.Id, actividad1.Id, actividad1); // Ana en Charla 1
        var pa3 = new ParticipantActivity(participante2.Id, actividad2.Id, actividad2); // Ana en Taller VIP
        var pa4 = new ParticipantActivity(participante3.Id, actividad1.Id, actividad1); // Carlos en Charla 1
        context.ParticipantActivities.AddRange(pa1, pa2, pa3, pa4);

        // Opcional: registra asistencia previa para pruebas
        var attendance = new Attendance(participante1.Id, evento.Id, "QR");
        context.Attendances.Add(attendance);

        // Agrega incidentes de prueba
        var incident1 = new Incident
        {
            Id = Guid.NewGuid(),
            EventId = evento.Id,
            Description = "Proyector no funciona",
            Location = "Sala A",
            IncidentDate = DateTime.Now.AddDays(7).AddHours(9),
            Status = "Pendiente"
        };
        var incident2 = new Incident
        {
            Id = Guid.NewGuid(),
            EventId = evento.Id,
            Description = "Falta de sillas",
            Location = "Sala B",
            IncidentDate = DateTime.Now.AddDays(7).AddHours(10),
            Status = "Pendiente"
        };
        context.Incidents.AddRange(incident1, incident2);

        // Crea una reasignación de ejemplo
        var reasignacion = new Reasignacion("Cambio de horario");
        context.Reasignaciones.Add(reasignacion);

        // Guarda los cambios
        context.SaveChanges();
    }
}
