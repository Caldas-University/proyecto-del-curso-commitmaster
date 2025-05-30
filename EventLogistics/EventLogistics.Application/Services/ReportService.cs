using EventLogistics.Domain.DTOs;
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
        private const int CriticalAvailabilityThreshold = 2; // Puedes ajustar el valor

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<IEnumerable<ResourceAssignment>> GenerateReport(int? eventId, string? resourceType, string? status)
        {
            // Llama al repositorio para obtener los datos filtrados
            return await _reportRepository.GetFilteredAssignments(eventId, resourceType, status);
        }

        public async Task<IEnumerable<ResourceMetricsDto>> GetResourceMetricsAsync()
        {
            return await _reportRepository.GetResourceMetricsAsync();
        }

        public async Task<IEnumerable<ResourceMetricsDto>> GetCriticalResourcesAsync()
        {
            var metrics = await GetResourceMetricsAsync();
            return metrics.Where(m => m.AvailableCount < CriticalAvailabilityThreshold).ToList();
        }
    }
}