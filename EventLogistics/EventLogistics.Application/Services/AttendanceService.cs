using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Application.DTOs;
using EventLogistics.Application.Mappers;

namespace EventLogistics.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly ICredentialService _credentialService;

        // Implementación de RegisterAttendanceManuallyAsync
        public async Task<(bool success, string message, int participantId, string participantName, string accessType)>
            RegisterAttendanceManuallyAsync(string document, string fullName)
        {
            // Implementación de ejemplo, ajustar según lógica de negocio
            var participant = await _participantRepository.GetByDocumentAsync(document);
            if (participant == null)
                return (false, "Participante no encontrado.", 0, "", "");

            var existingAttendance = await _attendanceRepository.GetByEventAndParticipantAsync(participant.EventId, participant.Id);
            if (existingAttendance != null)
                return (false, "Asistencia ya registrada.", 0, "", "");

            var attendance = new Attendance
            {
                ParticipantId = participant.Id,
                EventId = participant.EventId,
                CheckInTime = DateTime.UtcNow,
                CreatedBy = "Manual",
                UpdatedBy = "Manual",
                Method = "Manual" // <-- Agrega esto
            };

            await _attendanceRepository.AddAsync(attendance);
            return (true, "Asistencia registrada manualmente.", participant.Id, participant.FullName, participant.AccessType ?? "");
        }

        // Implementación de GetCheckInStatusAsync
        public async Task<bool> GetCheckInStatusAsync(int participantId)
        {
            var attendances = await _attendanceRepository.GetByParticipantIdAsync(participantId);
            return attendances.Any();
        }

        // Implementación de RegularizeAccessAsync
        public async Task<(bool success, string message)> RegularizeAccessAsync(int participantId, Dictionary<string, string> data)
        {
            var participant = await _participantRepository.GetByIdAsync(participantId);
            if (participant == null)
                return (false, "Participante no encontrado.");

            // Aquí puedes actualizar los datos del participante según el diccionario 'data'
            // Ejemplo: si data contiene "FullName", actualizar el nombre
            if (data.ContainsKey("FullName"))
                participant.FullName = data["FullName"];
            // Agregar más campos según sea necesario

            // Suponiendo que el repositorio tiene un método UpdateAsync
            await _participantRepository.UpdateAsync(participant);
            return (true, "Datos regularizados correctamente.");
        }

        public AttendanceService(
            IAttendanceRepository attendanceRepository,
            IParticipantRepository participantRepository,
            ICredentialService credentialService)
        {
            _attendanceRepository = attendanceRepository;
            _participantRepository = participantRepository;
            _credentialService = credentialService;
        }

        // Registrar asistencia por QR (firma requerida por la interfaz)
        public async Task<(bool success, string message, int participantId, string participantName, string accessType)>
            RegisterAttendanceAsync(string? qrCode)
        {
            if (string.IsNullOrEmpty(qrCode))
                return (false, "Código QR no proporcionado.", 0, "", "");

            var participant = await _participantRepository.GetByQrCodeAsync(qrCode);

            if (participant == null)
                return (false, "Participante no encontrado o inscripción incompleta.", 0, "", "");

            // Validar si ya está registrado el día del evento
            var existingAttendance = await _attendanceRepository.GetByEventAndParticipantAsync(participant.EventId, participant.Id);
            if (existingAttendance != null)
                return (false, "Asistencia ya registrada.", 0, "", "");

            // Crear registro de asistencia
            var attendance = new Attendance
            {
                ParticipantId = participant.Id,
                EventId = participant.EventId,
                CheckInTime = DateTime.UtcNow,
                CreatedBy = "System",
                UpdatedBy = "System",
                Method = "QR"
            };

            await _attendanceRepository.AddAsync(attendance);

            // Generar credencial y obtener horario personalizado
            await _credentialService.GenerateCredentialAsync(participant.Id);
            await _credentialService.GetPersonalizedScheduleAsync(participant.Id);

            return (true, "Asistencia registrada y credencial generada correctamente.", participant.Id, participant.FullName, participant.AccessType ?? "");
        }

        public async Task<IEnumerable<AttendanceResponseDto>> GetAttendanceByParticipantAsync(int participantId)
        {
            var attendances = await _attendanceRepository.GetByParticipantIdAsync(participantId);
            var participant = await _participantRepository.GetByIdAsync(participantId);

            return attendances.Select(a => new AttendanceResponseDto
            {
                ParticipantId = participant.Id,
                FullName = participant.FullName,
                AccessType = participant.AccessType,
                CheckInTime = a.CheckInTime,
                RegistrationComplete = participant.IsRegistrationComplete,
                Message = "Asistencia encontrada"
            });
        }

        public async Task<IEnumerable<AttendanceResponseDto>> GetAttendanceByEventAsync(int eventId)
        {
            var attendances = await _attendanceRepository.GetByEventIdAsync(eventId);
            var result = new List<AttendanceResponseDto>();

            foreach (var attendance in attendances)
            {
                var participant = await _participantRepository.GetByIdAsync(attendance.ParticipantId);
                result.Add(AttendanceMapper.ToResponseDto(attendance, participant));
            }
            return result;
        }
    }
}
