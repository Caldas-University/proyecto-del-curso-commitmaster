using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
    }
}
