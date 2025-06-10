using EventLogistics.Application.DTOs;
using EventLogistics.Application.Services;
using EventLogistics.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

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

        // GET: api/report/export/excel?eventId=1&resourceType=Audio&status=Asignado
        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] int? eventId, [FromQuery] string? resourceType, [FromQuery] string? status)
        {
            var metrics = await _reportService.GetResourceMetrics(eventId, resourceType, status);
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Reporte");
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Tipo";
            worksheet.Cell(1, 3).Value = "Total";
            worksheet.Cell(1, 4).Value = "Utilizados";
            worksheet.Cell(1, 5).Value = "Disponibles";
            worksheet.Cell(1, 6).Value = "Eventos";
            worksheet.Cell(1, 7).Value = "Actividades";
            worksheet.Cell(1, 8).Value = "Uso Total";
            worksheet.Cell(1, 9).Value = "Disponible";

            int row = 2;
            foreach (var m in metrics)
            {
                worksheet.Cell(row, 1).Value = m.Id.ToString();
                worksheet.Cell(row, 2).Value = m.Type;
                worksheet.Cell(row, 3).Value = m.Total;
                worksheet.Cell(row, 4).Value = m.Utilized;
                worksheet.Cell(row, 5).Value = m.Available;
                worksheet.Cell(row, 6).Value = m.Events;
                worksheet.Cell(row, 7).Value = m.Activities;
                worksheet.Cell(row, 8).Value = m.TotalUsage;
                worksheet.Cell(row, 9).Value = m.Availability ? "Sí" : "No";
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporte.xlsx");
        }

        // GET: api/report/export/pdf?eventId=1&resourceType=Audio&status=Asignado
        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportToPdf([FromQuery] int? eventId, [FromQuery] string? resourceType, [FromQuery] string? status)
        {
            var metrics = (await _reportService.GetResourceMetrics(eventId, resourceType, status)).ToList();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.Header().Text("Reporte de Recursos").FontSize(20).Bold();
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Id");
                            header.Cell().Element(CellStyle).Text("Tipo");
                            header.Cell().Element(CellStyle).Text("Total");
                            header.Cell().Element(CellStyle).Text("Utilizados");
                            header.Cell().Element(CellStyle).Text("Disponibles");
                            header.Cell().Element(CellStyle).Text("Eventos");
                            header.Cell().Element(CellStyle).Text("Actividades");
                            header.Cell().Element(CellStyle).Text("Uso Total");
                            header.Cell().Element(CellStyle).Text("Disponible");
                        });

                        foreach (var m in metrics)
                        {
                            table.Cell().Element(CellStyle).Text(m.Id.ToString());
                            table.Cell().Element(CellStyle).Text(m.Type);
                            table.Cell().Element(CellStyle).Text(m.Total.ToString());
                            table.Cell().Element(CellStyle).Text(m.Utilized.ToString());
                            table.Cell().Element(CellStyle).Text(m.Available.ToString());
                            table.Cell().Element(CellStyle).Text(m.Events.ToString());
                            table.Cell().Element(CellStyle).Text(m.Activities.ToString());
                            table.Cell().Element(CellStyle).Text(m.TotalUsage.ToString());
                            table.Cell().Element(CellStyle).Text(m.Availability ? "Sí" : "No");
                        }

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.Padding(2).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                        }
                    });
                });
            });

            var pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", "reporte.pdf");
        }

        // GET: api/report/alerts/critical?minAvailable=1
        [HttpGet("alerts/critical")]
        public async Task<ActionResult<IEnumerable<CriticalResourceAlertDto>>> GetCriticalResources([FromQuery] int minAvailable = 1)
        {
            var alerts = await _reportService.GetCriticalResources(minAvailable);

            if (alerts == null || !alerts.Any())
                return Ok(new List<CriticalResourceAlertDto>()); // Sin alertas

            return Ok(alerts);
        }
    }
}