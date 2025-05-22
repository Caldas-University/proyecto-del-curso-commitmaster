using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
/*
        [HttpGet("{participantId}/badge")]
        public async Task<IActionResult> GenerateCredential(int participantId)
        {
            var pdfBytes = await _credentialService.GenerateCredentialAsync(participantId);
            if (pdfBytes == null)
                return NotFound("Participante o actividades no encontradas");

            return File(pdfBytes, "application/pdf", $"Credencial_{participantId}.pdf");
        }

        [HttpGet("{participantId}/schedule")]
        public async Task<IActionResult> GetSchedule(int participantId)
        {
            var schedule = await _credentialService.GetPersonalizedScheduleAsync(participantId);
            return schedule == null ? NotFound("Cronograma no disponible") : Ok(schedule);
        }*/
    }
}