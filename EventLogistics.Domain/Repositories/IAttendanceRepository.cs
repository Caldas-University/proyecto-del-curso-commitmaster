using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories;

/// <summary>
/// Repositorio para operaciones de persistencia de asistencias.
/// </summary>
public interface IAttendanceRepository
{
    /// <summary>
    /// Obtiene un registro de asistencia por su Id.
    /// </summary>
    Task<Attendance?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtiene todas las asistencias de un participante en un evento.
    /// </summary>
    Task<List<Attendance>> GetByParticipantAsync(Guid participantId, Guid eventId);

    /// <summary>
    /// Agrega un nuevo registro de asistencia.
    /// </summary>
    Task AddAsync(Attendance attendance);

    /// <summary>
    /// Verifica si ya existe un registro de asistencia para un participante en un evento.
    /// </summary>
    Task<bool> ExistsAsync(Guid participantId, Guid eventId);

    /// <summary>
    /// Elimina un registro de asistencia por su Id.
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Obtiene la asistencia de un participante en un evento (único registro).
    /// </summary>
    Task<Attendance?> GetByParticipantAndEventAsync(Guid participantId, Guid eventId);

    /// <summary>
    /// Obtiene todas las asistencias de un evento.
    /// </summary>
    Task<List<Attendance>> GetAllByEventAsync(Guid eventId);
}