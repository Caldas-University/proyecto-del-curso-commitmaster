using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventLogistics.Api.Models;
using EventLogistics.Api.Services.Interfaces;

namespace EventLogistics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetReports()
        {
            var reports = await _reportService.GetAllReports();
            return Ok(reports);
        }

        [HttpPost]
        public async Task<ActionResult<ReportDto>> CreateReport([FromBody] ReportDto reportDto)
        {
            var createdReport = await _reportService.CreateReport(reportDto);
            return CreatedAtAction(nameof(GetReports), new { id = createdReport.Id }, createdReport);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] ReportDto reportDto)
        {
            if (id != reportDto.Id)
            {
                return BadRequest();
            }

            await _reportService.UpdateReport(reportDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            await _reportService.DeleteReport(id);
            return NoContent();
        }
    }
}