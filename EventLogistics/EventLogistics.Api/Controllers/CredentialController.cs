using EventLogistics.Application.Services;
using EventLogistics.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EventLogistics.Application.DTOs;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CredentialController : ControllerBase
    {
        private readonly ICredentialService _credentialService;

        public CredentialController(ICredentialService credentialService)
        {
            _credentialService = credentialService;
        }

        // GET: api/credential/{participantId}/badge
        [HttpGet("{participantId}/badge")]
        public async Task<IActionResult> GenerateCredential(int participantId)
        {
            var badgeData = await _credentialService.GenerateCredentialDataAsync(participantId);
            if (badgeData == null)
                return NotFound("Participante o actividades no encontradas");
            return Ok(badgeData);
        }

        // GET: api/credential/{participantId}/schedule
        [HttpGet("{participantId}/schedule")]
        public async Task<IActionResult> GetSchedule(int participantId)
        {
            var schedule = await _credentialService.GetPersonalizedScheduleAsync(participantId);
            return schedule == null ? NotFound("Cronograma no disponible") : Ok(schedule);
        }

        // GET: api/credential/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCredential(int id)
        {
            var credential = await _credentialService.GetByIdAsync(id);
            return credential == null ? NotFound() : Ok(credential);
        }

        // POST: api/credential
        [HttpPost]
        public async Task<IActionResult> CreateCredential([FromBody] CredentialCreateDto dto)
        {
            if (dto == null)
                return BadRequest();

            var credential = new Credential
            {
                ParticipantId = dto.ParticipantId,
                AccessType = dto.AccessType,
                IssuedAt = dto.IssuedAt,
                Printed = dto.Printed,
                BadgeData = dto.BadgeData,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            await _credentialService.AddAsync(credential);
            return CreatedAtAction(nameof(GetCredential), new { id = credential.Id }, credential);
        }

        // PUT: api/credential/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCredential(int id, [FromBody] Credential credential)
        {
            if (credential == null || credential.Id != id)
                return BadRequest();

            var existing = await _credentialService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _credentialService.UpdateAsync(credential);
            return NoContent();
        }

        // DELETE: api/credential/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCredential(int id)
        {
            var credential = await _credentialService.GetByIdAsync(id);
            if (credential == null)
                return NotFound();

            await _credentialService.DeleteAsync(id);
            return NoContent();
        }
    }
}