using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IReportServiceApp _reportService;

        public ReportController(IReportServiceApp reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ResourceReportDto>>> GetReport(
            [FromQuery] Guid? eventId,
            [FromQuery] string? resourceType,
            [FromQuery] string? status)
        {
            if (!eventId.HasValue && string.IsNullOrEmpty(resourceType) && string.IsNullOrEmpty(status))
            {
                return BadRequest(new { message = "Debe seleccionar al menos un filtro" });
            }

            var report = await _reportService.GenerateReportAsync(eventId, resourceType, status);

            if (report == null || report.Count == 0)
            {
                return NotFound(new { message = "No se encontraron recursos con los filtros aplicados" });
            }

            return Ok(report);
        }

        [HttpGet("metrics")]
        public async Task<ActionResult<List<ResourceReportDto>>> GetMetrics(
            [FromQuery] Guid? eventId,
            [FromQuery] string? resourceType,
            [FromQuery] string? status)
        {
            if (!eventId.HasValue && string.IsNullOrEmpty(resourceType) && string.IsNullOrEmpty(status))
            {
                return BadRequest(new { message = "Debe seleccionar al menos un filtro" });
            }

            var report = await _reportService.GenerateReportAsync(eventId, resourceType, status);

            if (report == null || report.Count == 0)
            {
                return NotFound(new { message = "No se encontraron recursos con los filtros aplicados" });
            }

            return Ok(report);
        }

        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportPdf(
            [FromQuery] Guid? eventId,
            [FromQuery] string? resourceType,
            [FromQuery] string? status)
        {
            var report = await _reportService.GenerateReportAsync(eventId, resourceType, status);

            if (report == null || report.Count == 0)
                return NotFound(new { message = "No se encontraron recursos con los filtros aplicados" });

            // Generar PDF usando QuestPDF
            var stream = new MemoryStream();
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(100); // Nombre
                            columns.ConstantColumn(80);  // Tipo
                            columns.ConstantColumn(60);  // Total
                            columns.ConstantColumn(60);  // Utilizada
                            columns.ConstantColumn(60);  // Disponible
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Nombre");
                            header.Cell().Element(CellStyle).Text("Tipo");
                            header.Cell().Element(CellStyle).Text("Total");
                            header.Cell().Element(CellStyle).Text("Utilizada");
                            header.Cell().Element(CellStyle).Text("Disponible");
                        });

                        // Rows
                        foreach (var item in report)
                        {
                            table.Cell().Element(CellStyle).Text(item.Nombre);
                            table.Cell().Element(CellStyle).Text(item.Tipo);
                            table.Cell().Element(CellStyle).Text(item.CantidadTotal.ToString());
                            table.Cell().Element(CellStyle).Text(item.CantidadUtilizada.ToString());
                            table.Cell().Element(CellStyle).Text(item.CantidadDisponible.ToString());
                        }
                    });
                });
            });

            document.GeneratePdf(stream);
            stream.Position = 0;
            return File(stream, "application/pdf", "reporte.pdf");

            IContainer CellStyle(IContainer container) =>
                container.PaddingVertical(2).PaddingHorizontal(4);
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportExcel(
            [FromQuery] Guid? eventId,
            [FromQuery] string? resourceType,
            [FromQuery] string? status)
        {
            var report = await _reportService.GenerateReportAsync(eventId, resourceType, status);

            if (report == null || report.Count == 0)
                return NotFound(new { message = "No se encontraron recursos con los filtros aplicados" });

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Reporte");

            // Header
            worksheet.Cell(1, 1).Value = "Nombre";
            worksheet.Cell(1, 2).Value = "Tipo";
            worksheet.Cell(1, 3).Value = "Total";
            worksheet.Cell(1, 4).Value = "Utilizada";
            worksheet.Cell(1, 5).Value = "Disponible";

            // Data
            for (int i = 0; i < report.Count; i++)
            {
                var item = report[i];
                worksheet.Cell(i + 2, 1).Value = item.Nombre;
                worksheet.Cell(i + 2, 2).Value = item.Tipo;
                worksheet.Cell(i + 2, 3).Value = item.CantidadTotal;
                worksheet.Cell(i + 2, 4).Value = item.CantidadUtilizada;
                worksheet.Cell(i + 2, 5).Value = item.CantidadDisponible;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporte.xlsx");
        }

        [HttpGet("critical")]
        public async Task<ActionResult<List<ResourceReportDto>>> GetCriticalResources()
        {
            var critical = await _reportService.GetCriticalResourcesAsync();
            if (critical == null || critical.Count == 0)
                return Ok(new { message = "No hay recursos en nivel crítico" });

            return Ok(critical);
        }
    }
}