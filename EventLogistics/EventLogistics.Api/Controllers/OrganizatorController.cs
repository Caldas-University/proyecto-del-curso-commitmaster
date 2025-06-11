using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizatorController : ControllerBase
    {
        private readonly IOrganizatorService _organizatorService;

        public OrganizatorController(IOrganizatorService organizatorService)
        {
            _organizatorService = organizatorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrganizatorDto>>> GetAll()
        {
            var organizators = await _organizatorService.GetAllAsync();
            return Ok(organizators);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizatorDto>> GetById(Guid id)
        {
            var organizator = await _organizatorService.GetByIdAsync(id);
            if (organizator == null)
                return NotFound();
            
            return Ok(organizator);
        }        [HttpPost]
        public async Task<ActionResult<OrganizatorDto>> Create([FromBody] CreateOrganizatorRequest organizatorRequest)
        {
            try
            {
                var result = await _organizatorService.CreateAsync(organizatorRequest);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al crear organizador", details = ex.Message });
            }
        }[HttpGet("role/{role}")]
        public async Task<ActionResult<List<OrganizatorDto>>> GetByRole(string role)
        {
            var organizators = await _organizatorService.GetByRoleAsync(role);
            return Ok(organizators);
        }
    }
}