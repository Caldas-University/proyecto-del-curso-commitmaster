namespace EventLogistics.Infrastructure.Persistence;

using EventLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public static class SeedData
{
    public static void Initialize(EventLogisticsDbContext context)
    {
        context.Database.EnsureCreated();        // Verifica si ya existen datos
        if (context.Events.Any() || context.Reasignaciones.Any())
        {
            return;   // La base de datos ya tiene datos
        }

        // Crea ubicaciones de ejemplo
        var salaConferencias = new Location("Sala de Conferencias", "Edificio A - Piso 2", "Disponible");
        var auditorio = new Location("Auditorio Principal", "Edificio B - Planta Baja", "Disponible");
        var salaReuniones = new Location("Sala de Reuniones", "Edificio A - Piso 1", "Disponible");
        var laboratorio = new Location("Laboratorio de Sistemas", "Edificio C - Piso 3", "Mantenimiento");
        context.Locations.AddRange(salaConferencias, auditorio, salaReuniones, laboratorio);
        context.SaveChanges(); // Guardar las ubicaciones primero para obtener sus IDs        // Crea usuarios de ejemplo
        var organizador = new User { Role = "Organizador", Contact = "organizador@ejemplo.com", Email = "organizador@ejemplo.com", Preferences = "{}", PhoneNumber = "123456789" };
        var asistente = new User { Role = "Asistente", Contact = "asistente@ejemplo.com", Email = "asistente@ejemplo.com", Preferences = "{}", PhoneNumber = "987654321" };
        context.Users.AddRange(organizador, asistente);
        
        // Crea organizadores de ejemplo
        var organizator1 = new Organizator("Dr. María García", "maria.garcia@universidad.edu", "+57-300-123-4567", "Coordinador Académico");
        var organizator2 = new Organizator("Ing. Carlos López", "carlos.lopez@empresa.com", "+57-310-987-6543", "Director de Tecnología");
        context.Organizators.AddRange(organizator1, organizator2);
        
        // Crea recursos de ejemplo
        var sala = new Resource { Type = "Sala", Name = "Sala Principal", Capacity = 100, Availability = true, FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(30), Assignments = new List<Guid>(), ReassignmentRules = new List<ReassignmentRule>() };
        var equipo = new Resource { Type = "Equipo", Name = "Proyector", Capacity = 1, Availability = true, FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(30), Assignments = new List<Guid>(), ReassignmentRules = new List<ReassignmentRule>() };
        context.Resources.AddRange(sala, equipo);
        
        // Crea un evento de ejemplo
        var evento = new Event
        {
            Name = "Conferencia de Tecnología",
            Place = "Sala de Conferencias",
            Schedule = DateTime.Now.AddDays(7),
            Status = "Activo",
            LocationId = salaConferencias.Id,
            Resources = new List<ResourceAssignment>(),
            Activities = new List<Activity>()
        };
        context.Events.Add(evento);        // Crea actividades de ejemplo
        var actividad1 = new Activity
        {
            Name = "Charla de IA",
            StartTime = DateTime.Now.AddDays(7).AddHours(9),
            EndTime = DateTime.Now.AddDays(7).AddHours(10),
            EventId = evento.Id,
            OrganizatorId = organizator1.Id,
            Place = "Sala de Conferencias", // <--- Agregado
            ResourceAssignments = new List<ResourceAssignment>()
        };
        var actividad2 = new Activity
        {
            Name = "Taller de Desarrollo",
            StartTime = DateTime.Now.AddDays(7).AddHours(11),
            EndTime = DateTime.Now.AddDays(7).AddHours(12),
            EventId = evento.Id,
            OrganizatorId = organizator2.Id,
            Place = "Auditorio Principal", // <--- Agregado
            ResourceAssignments = new List<ResourceAssignment>()
        };
        context.Activities.AddRange(actividad1, actividad2);

        // 3 participantes con asistencia registrada al evento
        var participante1 = new Participant("Juan Pérez", "123456", "juan@correo.com", "Asistente");
        var participante2 = new Participant("Ana Gómez", "654321", "ana@correo.com", "VIP");
        var participante3 = new Participant("Carlos Ruiz", "789012", "carlos@correo.com", "Ponente");

        // 3 participantes registrados en alguna actividad pero sin asistencia
        var participante4 = new Participant("Laura Torres", "111222", "laura@correo.com", "Asistente");
        var participante5 = new Participant("Miguel Salazar", "333444", "miguel@correo.com", "VIP");
        var participante6 = new Participant("Sofía Martínez", "555666", "sofia@correo.com", "Ponente");

        // 3 participantes solo registrados (sin actividades ni asistencia)
        var participante7 = new Participant("Pedro Jiménez", "777888", "pedro@correo.com", "Asistente");
        var participante8 = new Participant("Lucía Herrera", "999000", "lucia@correo.com", "VIP");
        var participante9 = new Participant("Elena Ríos", "112233", "elena@correo.com", "Ponente");

        context.Participants.AddRange(
            participante1, participante2, participante3,
            participante4, participante5, participante6,
            participante7, participante8, participante9
        );

        // Inscribir en actividades (solo para participante4, 5 y 6)
        var pa4 = new ParticipantActivity(participante4.Id, actividad1.Id, actividad1); // Laura en Charla de IA
        var pa5 = new ParticipantActivity(participante5.Id, actividad2.Id, actividad2); // Miguel en Taller de Desarrollo
        var pa6 = new ParticipantActivity(participante6.Id, actividad2.Id, actividad2); // Sofía en Taller de Desarrollo
        context.ParticipantActivities.AddRange(pa4, pa5, pa6);

        // Inscribir en actividades y asistencia (solo para participante1, 2 y 3)
        var pa1 = new ParticipantActivity(participante1.Id, actividad1.Id, actividad1); // Juan en Charla de IA
        var pa2 = new ParticipantActivity(participante2.Id, actividad1.Id, actividad1); // Ana en Charla de IA
        var pa3 = new ParticipantActivity(participante3.Id, actividad2.Id, actividad2); // Carlos en Taller de Desarrollo
        context.ParticipantActivities.AddRange(pa1, pa2, pa3);

        var asistencia1 = new Attendance(participante1.Id, evento.Id, "Manual");
        var asistencia2 = new Attendance(participante2.Id, evento.Id, "Manual");
        var asistencia3 = new Attendance(participante3.Id, evento.Id, "Manual");
        context.Attendances.AddRange(asistencia1, asistencia2, asistencia3);

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
