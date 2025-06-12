using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces; // Ensure this is the correct namespace for IParticipantServiceApp
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participantService;

        public ParticipantController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipantDto>> GetById(Guid id)
        {
            var dto = await _participantService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();
            return Ok(dto);
        }

        [HttpGet("by-document/{document}")]
        public async Task<ActionResult<ParticipantDto>> GetByDocument(string document)
        {
            var dto = await _participantService.GetByDocumentAsync(document);
            if (dto == null)
                return NotFound();
            return Ok(dto);
        }

        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<ParticipantDto>> GetByEmail(string email)
        {
            var dto = await _participantService.GetByEmailAsync(email);
            if (dto == null)
                return NotFound();
            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<List<ParticipantDto>>> GetAll()
        {
            var dtos = await _participantService.GetAllAsync();
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<ParticipantDto>> Create([FromBody] ParticipantDto dto)
        {
            var created = await _participantService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ParticipantDto>> Update(Guid id, [FromBody] ParticipantDto dto)
        {
            var updated = await _participantService.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _participantService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}