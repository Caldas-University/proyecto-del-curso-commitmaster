using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EventLogistics.Application.DTOs;
using EventLogistics.Application.DTOs.Attendance; // <-- Agrega este using
using EventLogistics.Application.Services;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ICredentialService _credentialService;

        public AttendanceController(
            IAttendanceService attendanceService,
            ICredentialService credentialService)
        {
            _attendanceService = attendanceService;
            _credentialService = credentialService;
        }

        // POST: api/attendance/check-in
        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn([FromBody] AttendanceRegisterDto dto)
        {
            var (success, message, participantId, participantName, accessType) =
                await _attendanceService.RegisterAttendanceAsync(dto.QrCode);

            if (!success)
                return BadRequest(new { message });

            var badgeBytes = await _credentialService.GenerateCredentialAsync(participantId);
            var schedule = await _credentialService.GetPersonalizedScheduleAsync(participantId);

            var response = new CredentialResponseDto
            {
                ParticipantName = participantName,
                AccessType = accessType ?? string.Empty,
                CredentialPdf = badgeBytes,
                Schedule = schedule
            };

            return Ok(response);
        }

        // POST: api/attendance/manual-check-in
        [HttpPost("manual-check-in")]
        public async Task<IActionResult> ManualCheckIn([FromBody] AttendanceRegisterDto dto)
        {
            var (success, message, participantId, participantName, accessType) =
                await _attendanceService.RegisterAttendanceManuallyAsync(dto.Document!, dto.Name!);

            if (!success)
                return BadRequest(new { message });

            var badgeBytes = await _credentialService.GenerateCredentialAsync(participantId);
            var schedule = await _credentialService.GetPersonalizedScheduleAsync(participantId);

            var response = new CredentialResponseDto
            {
                ParticipantName = participantName,
                AccessType = accessType ?? string.Empty,
                CredentialPdf = badgeBytes,
                Schedule = schedule,
            };

            return Ok(response);
        }

        // GET: api/attendance/check-in-status/{participantId}
        [HttpGet("check-in-status/{participantId}")]
        public async Task<IActionResult> CheckInStatus(int participantId)
        {
            var status = await _attendanceService.GetCheckInStatusAsync(participantId);
            return Ok(new { checkedIn = status });
        }

        // GET: api/attendance/participant/{participantId}
        [HttpGet("participant/{participantId}")]
        public async Task<IActionResult> GetAttendanceByParticipant(int participantId)
        {
            var attendances = await _attendanceService.GetAttendanceByParticipantAsync(participantId);
            return Ok(attendances);
        }

        // GET: api/attendance/event/{eventId}
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetAttendanceByEvent(int eventId)
        {
            var attendances = await _attendanceService.GetAttendanceByEventAsync(eventId);
            return Ok(attendances);
        }

        // POST: api/attendance/regularize-access
        [HttpPost("regularize-access")]
        public async Task<IActionResult> RegularizeAccess([FromBody] RegularizeAccessDto dto)
        {
            var (success, message) = await _attendanceService.RegularizeAccessAsync(dto.ParticipantId, dto.DataToUpdate);
            if (!success)
                return BadRequest(new { message });
            return Ok(new { message });
        }
    }
}
