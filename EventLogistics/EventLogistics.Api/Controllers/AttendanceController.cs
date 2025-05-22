using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly ICredentialService _credentialService;

        public AttendanceController(
            IAttendanceRepository attendanceRepository,
            IParticipantRepository participantRepository,
            ICredentialService credentialService)
        {
            _attendanceRepository = attendanceRepository;
            _participantRepository = participantRepository;
            _credentialService = credentialService;
        }

        // ✅ Registro por QR
        [HttpPost("qr")]
        public async Task<IActionResult> RegisterByQr([FromBody] string qrCodeData)
        {
            var participant = await _participantRepository.GetByQrCodeAsync(qrCodeData);
            if (participant == null)
                return NotFound("Participante no encontrado");

            var now = DateTime.UtcNow;
            var asistencia = new Attendance
            {
                ParticipantId = participant.Id,
                EventId = participant.EventId,
                Timestamp = now,
                CheckInTime = now,
                Method = "QR"
            };

            await _attendanceRepository.AddAsync(asistencia);
            return Ok("Asistencia registrada por QR");
        }

        // ✅ Registro manual
        [HttpPost("manual")]
        public async Task<IActionResult> RegisterManual([FromBody] string documentoIdentidad)
        {
            var participant = await _participantRepository.GetByDocumentAsync(documentoIdentidad);
            if (participant == null)
                return NotFound("Participante no encontrado");

            var now = DateTime.UtcNow;
            var asistencia = new Attendance
            {
                ParticipantId = participant.Id,
                EventId = participant.EventId,
                Timestamp = now,
                CheckInTime = now,
                Method = "Manual"
            };

            await _attendanceRepository.AddAsync(asistencia);
            return Ok("Asistencia registrada manualmente");
        }

        // ✅ Endpoint unificado que valida, registra, genera PDF y cronograma
        [HttpPost("check-in/{identifier}")]
        public async Task<IActionResult> CheckIn(string identifier)
        {
            // 1. Buscar por documento o QR
            var participant = await _participantRepository.GetByDocumentAsync(identifier)
                            ?? await _participantRepository.GetByQrCodeAsync(identifier);

            if (participant == null)
                return NotFound("Participante no encontrado");

            // 2. Validar inscripción completa
            if (!participant.IsRegistrationComplete)
                return BadRequest("Inscripción incompleta. Regularizar antes de continuar.");

            // 3. Registrar asistencia
            var now = DateTime.UtcNow;
            var asistencia = new Attendance
            {
                ParticipantId = participant.Id,
                EventId = participant.EventId,
                Timestamp = now,
                CheckInTime = now,
                Method = "CheckIn"
            };

            await _attendanceRepository.AddAsync(asistencia);

            // 4. Generar credencial (PDF) y cronograma
            var badgeBytes = await _credentialService.GenerateCredentialAsync(participant.Id);
            var schedule = await _credentialService.GetPersonalizedScheduleAsync(participant.Id);

            // 5. Retornar todo junto
            return Ok(new
            {
                Message = "Asistencia registrada con éxito",
                Participant = new
                {
                    participant.FullName,
                    participant.AccessType
                },
                BadgePdfBase64 = badgeBytes != null ? Convert.ToBase64String(badgeBytes) : null,
                Schedule = schedule
            });
        }
    }
}
