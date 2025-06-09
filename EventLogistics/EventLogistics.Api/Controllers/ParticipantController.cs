using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EventLogistics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantController : ControllerBase
{
    private readonly IParticipantRepository _participantRepository;

    public ParticipantController(IParticipantRepository participantRepository)
    {
        _participantRepository = participantRepository;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParticipantDto>> GetById(Guid id)
    {
        var participant = await _participantRepository.GetByIdAsync(id);
        if (participant == null)
            return NotFound();

        // Puedes mapear a DTO si lo prefieres
        var dto = new ParticipantDto
        {
            Id = participant.Id,
            Name = participant.Name,
            Document = participant.Document,
            Email = participant.Email,
            AccessType = participant.AccessType
        };
        return Ok(dto);
    }

    [HttpGet("by-document/{document}")]
    public async Task<ActionResult<ParticipantDto>> GetByDocument(string document)
    {
        var participant = await _participantRepository.GetByDocumentAsync(document);
        if (participant == null)
            return NotFound();

        var dto = new ParticipantDto
        {
            Id = participant.Id,
            Name = participant.Name,
            Document = participant.Document,
            Email = participant.Email,
            AccessType = participant.AccessType
        };
        return Ok(dto);
    }

    [HttpGet("by-email/{email}")]
    public async Task<ActionResult<ParticipantDto>> GetByEmail(string email)
    {
        var participant = await _participantRepository.GetByEmailAsync(email);
        if (participant == null)
            return NotFound();

        var dto = new ParticipantDto
        {
            Id = participant.Id,
            Name = participant.Name,
            Document = participant.Document,
            Email = participant.Email,
            AccessType = participant.AccessType
        };
        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<List<ParticipantDto>>> GetAll()
    {
        var participants = await _participantRepository.GetAllAsync();
        // Si tienes un mapper, úsalo aquí
        var dtos = participants.Select(p => new ParticipantDto
        {
            Id = p.Id,
            Name = p.Name,
            Document = p.Document,
            Email = p.Email,
            AccessType = p.AccessType
        }).ToList();
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<ParticipantDto>> Create([FromBody] ParticipantDto dto)
    {
        var participant = new Participant(dto.Name, dto.Document, dto.Email, dto.AccessType);
        await _participantRepository.AddAsync(participant);
        dto.Id = participant.Id;
        return CreatedAtAction(nameof(GetAll), new { id = dto.Id }, dto);
    }
}