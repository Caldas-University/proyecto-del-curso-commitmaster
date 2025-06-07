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

        public async Task<IEnumerable<Resource>> GenerateReport(int? eventId, string? resourceType, string? status)
        {
            return await _reportRepository.GenerateReportAsync(eventId, resourceType, status);
        }
    }
}