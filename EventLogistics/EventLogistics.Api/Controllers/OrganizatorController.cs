using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizatorController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public OrganizatorController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-request")]
        public async Task<IActionResult> SendRequest([FromBody] Activity activity)
        {
            // Lógica para procesar la solicitud del organizador
            await _emailService.SendNotificationAsync(activity.Organizator.Email, "Solicitud recibida");
            return Ok();
        }
    }
}