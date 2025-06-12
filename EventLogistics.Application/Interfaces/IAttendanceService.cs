using EventLogistics.Application.DTOs;

namespace EventLogistics.Application.Interfaces;

public interface IAttendanceServiceApp
{
    // Obtener el contenido QR para un participante y evento
    Task<string> GetQrContentAsync(Guid participantId, Guid eventId);

    // Registrar asistencia por QR (usado por el controlador)
    Task<AttendanceDto> RegisterAttendanceAsync(Guid participantId, Guid eventId, string method);

    // Registrar asistencia manual por documento
    Task<AttendanceDto> RegisterAttendanceManualAsync(string document, Guid eventId);

    // Generar escarapela y cronograma en PDF tras registrar asistencia
    Task<byte[]> GenerateCredentialAndSchedulePdfAsync(Guid participantId, Guid eventId);

    // Listar todas las asistencias
    Task<IEnumerable<AttendanceDto>> ListAllAsync();

    // Obtener asistencia por ID
    Task<AttendanceDto?> GetByIdAsync(Guid attendanceId);

    // Listar asistencias por evento
    Task<IEnumerable<AttendanceDto>> ListByEventAsync(Guid eventId);

    // Listar asistencias por participante
    Task<IEnumerable<AttendanceDto>> ListByParticipantAsync(Guid participantId);

    // Obtener cronograma personalizado de un participante para un evento
    Task<IEnumerable<ActivityDto>> GetScheduleAsync(Guid participantId, Guid eventId);

    // Verificar inscripción de participante en evento
    Task<bool> VerifyInscriptionAsync(Guid participantId, Guid eventId);

    // Regularizar inscripción en tiempo real
    Task RegularizeInscriptionAsync(Guid participantId, Guid eventId);

    // Obtener resumen de asistencia por evento
    Task<AttendanceSummaryDto> GetAttendanceSummaryAsync(Guid eventId);

    // Listar participantes por evento
    Task<IEnumerable<ParticipantDto>> ListParticipantsByEventAsync(Guid eventId);
}