using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EventLogistics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventRepository _eventRepository;

    public EventController(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<EventDto>>> GetAll()
    {
        var events = await _eventRepository.GetAllAsync();
        var dtos = events.Select(ev => new EventDto
        {
            Id = ev.Id,
            Place = ev.Place,
            Schedule = ev.Schedule,
            Resources = ev.Resources,
            Status = ev.Status
        }).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetById(Guid id)
    {
        var ev = await _eventRepository.GetByIdAsync(id);
        if (ev == null)
            return NotFound();

        var dto = new EventDto
        {
            Id = ev.Id,
            Place = ev.Place,
            Schedule = ev.Schedule,
            Resources = ev.Resources,
            Status = ev.Status
        };
        return Ok(dto);
    }
}