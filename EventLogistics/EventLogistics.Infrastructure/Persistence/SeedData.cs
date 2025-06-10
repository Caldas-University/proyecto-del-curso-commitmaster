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
        }        // Crea usuarios de ejemplo
        var organizador = new User { Role = "Organizador", Contact = "organizador@ejemplo.com", Email = "organizador@ejemplo.com", Preferences = "{}", PhoneNumber = "123456789" };
        var asistente = new User { Role = "Asistente", Contact = "asistente@ejemplo.com", Email = "asistente@ejemplo.com", Preferences = "{}", PhoneNumber = "987654321" };
        context.Users.AddRange(organizador, asistente);        // Crea recursos de ejemplo
        var sala = new Resource { Type = "Sala", Name = "Sala Principal", Capacity = 100, Availability = true, FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(30), Assignments = new List<Guid>(), ReassignmentRules = new List<ReassignmentRule>() };
        var equipo = new Resource { Type = "Equipo", Name = "Proyector", Capacity = 1, Availability = true, FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(30), Assignments = new List<Guid>(), ReassignmentRules = new List<ReassignmentRule>() };
        context.Resources.AddRange(sala, equipo);        // Crea un evento de ejemplo
        var evento = new Event
        {
            Name = "Conferencia de Tecnología",
            Place = "Sala Principal",
            Schedule = DateTime.Now.AddDays(7),
            Status = "Activo",
            Resources = new List<ResourceAssignment>(),
            Activities = new List<Activity>()
        };
        context.Events.Add(evento);

        // Crea actividades de ejemplo
        var actividad1 = new Activity
        {
            Name = "Charla de IA",
            StartTime = DateTime.Now.AddDays(7).AddHours(9),
            EndTime = DateTime.Now.AddDays(7).AddHours(10),
            EventId = evento.Id,
            OrganizatorId = Guid.NewGuid(),
            ResourceAssignments = new List<ResourceAssignment>()
        };
        var actividad2 = new Activity
        {
            Name = "Taller de Desarrollo",
            StartTime = DateTime.Now.AddDays(7).AddHours(11),
            EndTime = DateTime.Now.AddDays(7).AddHours(12),
            EventId = evento.Id,
            OrganizatorId = Guid.NewGuid(),
            ResourceAssignments = new List<ResourceAssignment>()
        };
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
