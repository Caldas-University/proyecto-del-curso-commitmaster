using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantRepository _participantRepository;

        public ParticipantController(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var participants = await _participantRepository.GetAllAsync();
            return participants.Any() ? Ok(participants) : NotFound("No hay participantes registrados");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var participant = await _participantRepository.GetByIdAsync(id);
            return participant != null ? Ok(participant) : NotFound();
        }

        [HttpGet("document/{doc}")]
        public async Task<IActionResult> GetByDocument(string doc)
        {
            var participant = await _participantRepository.GetByDocumentAsync(doc);
            return participant != null ? Ok(participant) : NotFound();
        }

        // POST: api/participant
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParticipantCreateDto dto)
        {
            if (dto == null)
                return BadRequest();

            // Mapea el DTO a la entidad
            var participant = new Participant
            {
                FullName = dto.FullName,
                AccessType = dto.AccessType,
                QrCode = dto.QrCode,
                Document = dto.Document,
                EventId = dto.EventId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            await _participantRepository.AddAsync(participant);
            return CreatedAtAction(nameof(Get), new { id = participant.Id }, participant);
        }

        // PUT: api/participant/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ParticipantUpdateDto dto)
        {
            if (dto == null)
                return BadRequest();

            var existing = await _participantRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            // Validar que el EventId exista (opcional pero recomendado)
            // var eventExists = await _eventRepository.GetByIdAsync(dto.EventId) != null;
            // if (!eventExists)
            //     return BadRequest($"El evento con ID {dto.EventId} no existe.");

            // Actualiza solo las propiedades necesarias
            if (dto.FullName != null)
                existing.FullName = dto.FullName;
            existing.AccessType = dto.AccessType;
            if (dto.QrCode != null)
                existing.QrCode = dto.QrCode;
            if (dto.Document != null)
                existing.Document = dto.Document;
            existing.EventId = dto.EventId;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedBy = "System";

            await _participantRepository.UpdateAsync(existing);
            return NoContent();
        }

        // DELETE: api/participant/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var participant = await _participantRepository.GetByIdAsync(id);
            if (participant == null)
                return NotFound();

            await _participantRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
