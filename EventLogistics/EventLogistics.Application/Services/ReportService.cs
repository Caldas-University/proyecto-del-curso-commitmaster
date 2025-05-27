using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using System.Collections.Generic;
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

        public async Task<IEnumerable<ResourceAssignment>> GenerateReport(int? eventId, string? resourceType, string? status)
        {
            // Llama al repositorio para obtener los datos filtrados
            return await _reportRepository.GetFilteredAssignments(eventId, resourceType, status);
        }
    }
}