using EventLogistics.Application.Services;
using EventLogistics.Domain.DTOs;
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
        public async Task<ActionResult<IEnumerable<ResourceAssignment>>> GetReport(
            [FromQuery] int? eventId,
            [FromQuery] string? resourceType,
            [FromQuery] string? status)
        {
            if (!eventId.HasValue && string.IsNullOrEmpty(resourceType) && string.IsNullOrEmpty(status))
            {
                return BadRequest(new { message = "Debe seleccionar al menos un filtro" });
            }

            var report = await _reportService.GenerateReport(eventId, resourceType, status);
            return Ok(report);
        }

        // GET: api/report/metrics
        [HttpGet("metrics")]
        public async Task<ActionResult<IEnumerable<ResourceMetricsDto>>> GetResourceMetrics()
        {
            var metrics = await _reportService.GetResourceMetricsAsync();
            return Ok(metrics);
        }

        // GET: api/report/metrics/excel
        [HttpGet("metrics/excel")]
        public async Task<IActionResult> ExportMetricsToExcel()
        {
            var metrics = await _reportService.GetResourceMetricsAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Métricas");
            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "Type";
            worksheet.Cell(1, 3).Value = "TotalCount";
            worksheet.Cell(1, 4).Value = "UsedCount";
            worksheet.Cell(1, 5).Value = "AvailableCount";
            worksheet.Cell(1, 6).Value = "EventsCount";
            worksheet.Cell(1, 7).Value = "ActivitiesCount";
            worksheet.Cell(1, 8).Value = "TotalUsage";
            worksheet.Cell(1, 9).Value = "Availability";

            int row = 2;
            foreach (var m in metrics)
            {
                worksheet.Cell(row, 1).Value = m.Name;
                worksheet.Cell(row, 2).Value = m.Type;
                worksheet.Cell(row, 3).Value = m.TotalCount;
                worksheet.Cell(row, 4).Value = m.UsedCount;
                worksheet.Cell(row, 5).Value = m.AvailableCount;
                worksheet.Cell(row, 6).Value = m.EventsCount;
                worksheet.Cell(row, 7).Value = m.ActivitiesCount;
                worksheet.Cell(row, 8).Value = m.TotalUsage;
                worksheet.Cell(row, 9).Value = m.Availability;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "metrics.xlsx");
        }

        // GET: api/report/metrics/pdf
        [HttpGet("metrics/pdf")]
        public async Task<IActionResult> ExportMetricsToPdf()
        {
            // Configura la licencia de QuestPDF
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var metrics = await _reportService.GetResourceMetricsAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                for (int i = 0; i < 9; i++) columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Name");
                                header.Cell().Element(CellStyle).Text("Type");
                                header.Cell().Element(CellStyle).Text("TotalCount");
                                header.Cell().Element(CellStyle).Text("UsedCount");
                                header.Cell().Element(CellStyle).Text("AvailableCount");
                                header.Cell().Element(CellStyle).Text("EventsCount");
                                header.Cell().Element(CellStyle).Text("ActivitiesCount");
                                header.Cell().Element(CellStyle).Text("TotalUsage");
                                header.Cell().Element(CellStyle).Text("Availability");
                            });

                            foreach (var m in metrics)
                            {
                                table.Cell().Element(CellStyle).Text(m.Name);
                                table.Cell().Element(CellStyle).Text(m.Type);
                                table.Cell().Element(CellStyle).Text(m.TotalCount.ToString());
                                table.Cell().Element(CellStyle).Text(m.UsedCount.ToString());
                                table.Cell().Element(CellStyle).Text(m.AvailableCount.ToString());
                                table.Cell().Element(CellStyle).Text(m.EventsCount.ToString());
                                table.Cell().Element(CellStyle).Text(m.ActivitiesCount.ToString());
                                table.Cell().Element(CellStyle).Text(m.TotalUsage.ToString());
                                table.Cell().Element(CellStyle).Text(m.Availability ? "Sí" : "No");
                            }

                            IContainer CellStyle(IContainer container) =>
                                container.PaddingVertical(2).PaddingHorizontal(4);
                        });
                });
            });

            var pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", "metrics.pdf");
        }
    }
}