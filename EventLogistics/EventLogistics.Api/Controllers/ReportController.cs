using EventLogistics.Application.DTOs;
using EventLogistics.Application.Services;
using EventLogistics.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: api/report?eventId=1&resourceType=Audio&status=Asignado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> GetReport(
            [FromQuery] int? eventId,
            [FromQuery] string? resourceType,
            [FromQuery] string? status)
        {
            if (!eventId.HasValue && string.IsNullOrEmpty(resourceType) && string.IsNullOrEmpty(status))
            {
                return BadRequest(new { message = "Debe seleccionar al menos un filtro" });
            }

            var report = await _reportService.GenerateReport(eventId, resourceType, status);

            if (report == null || !report.Any())
            {
                return NotFound(new { message = "No se encontraron recursos con los filtros aplicados" });
            }

            return Ok(report);
        }

        // GET: api/report/metrics?eventId=1&resourceType=Audio&status=Asignado
        [HttpGet("metrics")]
        public async Task<ActionResult<IEnumerable<ResourceMetricsDto>>> GetResourceMetrics(
            [FromQuery] int? eventId,
            [FromQuery] string? resourceType,
            [FromQuery] string? status)
        {
            var metrics = await _reportService.GetResourceMetrics(eventId, resourceType, status);

            if (metrics == null || !metrics.Any())
            {
                return NotFound(new { message = "No se encontraron métricas para los filtros aplicados" });
            }

            return Ok(metrics);
        }
    }
}