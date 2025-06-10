using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportServiceApp _reportService;

        public ReportController(IReportServiceApp reportService)
        {
            _reportService = reportService;
        }        // GET: api/report?eventId=1&resourceType=Audio&status=Asignado
        [HttpGet]
        public async Task<ActionResult<List<ResourceDto>>> GetReport(
            [FromQuery] Guid? eventId,
            [FromQuery] string? resourceType,
            [FromQuery] string? status)
        {
            if (!eventId.HasValue && string.IsNullOrEmpty(resourceType) && string.IsNullOrEmpty(status))
            {
                return BadRequest(new { message = "Debe seleccionar al menos un filtro" });
            }

            var report = await _reportService.GenerateReportAsync(eventId, resourceType, status);
            return Ok(report);
        }
    }
}