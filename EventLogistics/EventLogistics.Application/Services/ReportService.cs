using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLogistics.Application.Services
{
    public class ReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<IEnumerable<Resource>> GenerateReport(int? eventId, string? resourceType, string? status)
        {
            return await _reportRepository.GenerateReportAsync(eventId, resourceType, status);
        }

        public async Task<IEnumerable<ResourceMetricsDto>> GetResourceMetrics(int? eventId, string? resourceType, string? status)
        {
            var resources = await _reportRepository.GenerateReportAsync(eventId, resourceType, status);

            // Procesa las métricas
            var metrics = resources.Select(r => new ResourceMetricsDto
            {
                Id = r.Id, // Usa el Id como identificador
                Type = r.Type,
                Total = r.Capacity,
                Utilized = r.Capacity - (r.Availability ? 1 : 0), // Ajusta según tu lógica real
                Available = r.Availability ? 1 : 0,
                Events = r.Assignments?.Count ?? 0,
                Activities = 0, // Si tienes relación con actividades, cámbialo aquí
                TotalUsage = r.Capacity, // O ajusta según tu lógica
                Availability = r.Availability
            });

            return metrics;
        }
    }
}