using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventLogistics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantController : ControllerBase
{
    private readonly IParticipantServiceApp _participantService;

    public ParticipantController(IParticipantServiceApp participantService)
    {
        _participantService = participantService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParticipantDto>> GetById(Guid id)
    {
        var participantDto = await _participantService.GetByIdAsync(id);
        if (participantDto == null)
            return NotFound();

        return Ok(participantDto);
    }

    [HttpGet("by-document/{document}")]
    public async Task<ActionResult<ParticipantDto>> GetByDocument(string document)
    {
        var participantDto = await _participantService.GetByDocumentAsync(document);
        if (participantDto == null)
            return NotFound();

        return Ok(participantDto);
    }

    [HttpGet("by-email/{email}")]
    public async Task<ActionResult<ParticipantDto>> GetByEmail(string email)
    {
        var participantDto = await _participantService.GetByEmailAsync(email);
        if (participantDto == null)
            return NotFound();

        return Ok(participantDto);
    }

    [HttpGet]
    public async Task<ActionResult<List<ParticipantDto>>> GetAll()
    {
        var participants = await _participantService.GetAllAsync();
        return Ok(participants);
    }

    [HttpPost]
    public async Task<ActionResult<ParticipantDto>> Create([FromBody] ParticipantDto dto)
    {
        var createdParticipant = await _participantService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdParticipant.Id }, createdParticipant);
    }
}
