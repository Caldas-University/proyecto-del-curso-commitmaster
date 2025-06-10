using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using EventLogistics.Application.Mappers;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

namespace EventLogistics.Application.Services;

public class AttendanceServiceApp : IAttendanceServiceApp
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly IParticipantActivityRepository _participantActivityRepository;

    public AttendanceServiceApp(
        IAttendanceRepository attendanceRepository,
        IParticipantRepository participantRepository,
        IParticipantActivityRepository participantActivityRepository)
    {
        _attendanceRepository = attendanceRepository;
        _participantRepository = participantRepository;
        _participantActivityRepository = participantActivityRepository;
    }    public async Task<AttendanceDto> RegisterAttendanceAsync(Guid participantId, Guid eventId, string method)
    {
        // 1. Validar que el participante exista
        var participant = await _participantRepository.GetByIdAsync(participantId);
        if (participant == null)
            throw new InvalidOperationException("El participante no existe.");

        // 2. Validar que esté inscrito en al menos una actividad del evento
        var inscrito = await _participantActivityRepository
            .AnyAsync(pa => pa.ParticipantId == participantId && pa.Activity != null && pa.Activity.EventId == eventId);

        if (!inscrito)
            throw new InvalidOperationException("El participante no está inscrito en ninguna actividad de este evento.");

        // 3. Validar que no haya asistencia duplicada
        if (await _attendanceRepository.ExistsAsync(participantId, eventId))
            throw new InvalidOperationException("La asistencia ya fue registrada.");

        // Genera el contenido del QR
        var qrContent = $"{participantId}|{eventId}|{DateTime.UtcNow:yyyyMMddHHmmss}";
        var qrCode = GenerateQrBase64(qrContent); // Este método debe devolver el string base64 del QR

        // 4. Registrar asistencia con QR
        var attendance = new Attendance(participantId, eventId, method)
        {
            QRCode = qrCode
        };
        await _attendanceRepository.AddAsync(attendance);

        return AttendanceMapper.ToDto(attendance);
    }

    public async Task<AttendanceDto> RegisterAttendanceByDocumentAsync(string document, Guid eventId, string method)
    {
        // Buscar participante por documento
        var participant = await _participantRepository.GetByDocumentAsync(document);
        if (participant == null)
            throw new InvalidOperationException("Participante no encontrado.");
        
        // Delegar al método principal
        return await RegisterAttendanceAsync(participant.Id, eventId, method);
    }

    public async Task<CredentialDto> GenerateCredentialAsync(Guid participantId, Guid eventId)
    {
        // 1. Obtener participante real
        var participant = await _participantRepository.GetByIdAsync(participantId);
        if (participant == null)
            throw new InvalidOperationException("El participante no existe.");

        // 2. Obtener nombre del evento real (ajusta si tienes IEventRepository)
        var eventName = "Nombre del Evento"; // Reemplaza por el nombre real si tienes el repositorio

        // 3. Generar QR real con el formato correcto
        var qrContent = $"{participant.Id}|{eventId}|{DateTime.UtcNow:yyyyMMddHHmmss}";

        // 4. Obtener cronograma real (actividades en las que está inscrito el participante)
        var participantActivities = await _participantActivityRepository.GetByParticipantAndEventAsync(participantId, eventId);
        var schedule = participantActivities.Select(pa => new ScheduleItemDto
        {
            ActivityName = pa.Activity?.Name ?? "Sin nombre",
            StartTime = pa.Activity?.StartTime ?? DateTime.MinValue,
            EndTime = pa.Activity?.EndTime ?? DateTime.MinValue,
            Lugar = pa.Activity?.Lugar ?? "Sin ubicación"
        }).ToList();

        return new CredentialDto
        {
            ParticipantName = participant.Name,
            AccessType = participant.AccessType,
            EventName = eventName,
            QRCode = qrContent, // <-- Devuelve el string plano
            Schedule = schedule
        };
    }

    public async Task<List<AttendanceDto>> GetAttendanceByParticipantAsync(Guid participantId, Guid eventId)
    {
        var attendances = await _attendanceRepository.GetByParticipantAsync(participantId, eventId);
        return attendances.Select(AttendanceMapper.ToDto).ToList();
    }

    // Método privado para generar el QR en base64
    private string GenerateQrBase64(string content)
    {
        // Simulación de QR: convierte el contenido a base64 (no es un QR real)
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(content));
    }
}