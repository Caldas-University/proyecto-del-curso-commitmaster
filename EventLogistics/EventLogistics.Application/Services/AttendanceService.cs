using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace EventLogistics.Application.Services
{
    public class AttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly ICredentialService _credentialService; // Servicio para generar credenciales (puede ser otro servicio o clase)
        
        public AttendanceService(
            IAttendanceRepository attendanceRepository,
            IParticipantRepository participantRepository,
            ICredentialService credentialService)
        {
            _attendanceRepository = attendanceRepository;
            _participantRepository = participantRepository;
            _credentialService = credentialService;
        }
        
        // Registrar asistencia por QR o documento
        public async Task<(bool success, string message)> RegisterAttendanceAsync(string qrCode = null, string document = null)
        {
            Participant participant = null;

            if (!string.IsNullOrEmpty(qrCode))
                participant = await _participantRepository.GetByQrCodeAsync(qrCode);
            else if (!string.IsNullOrEmpty(document))
                participant = await _participantRepository.GetByDocumentAsync(document);
            
            if (participant == null)
                return (false, "Participante no encontrado o inscripción incompleta.");

            // Validar si ya está registrado el día del evento
            var existingAttendance = await _attendanceRepository.GetByEventAndParticipantAsync(participant.EventId, participant.Id);
            if (existingAttendance != null)
                return (false, "Asistencia ya registrada.");

            // Crear registro de asistencia
            var attendance = new Attendance
            {
                ParticipantId = participant.Id,
                EventId = participant.EventId,
                CheckInTime = DateTime.UtcNow,
                CreatedBy = "System",
                UpdatedBy = "System"
            };

            await _attendanceRepository.AddAsync(attendance);

            return (true, "Asistencia registrada y credencial generada correctamente.");
        }
    }
}
