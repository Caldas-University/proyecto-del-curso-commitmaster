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
}