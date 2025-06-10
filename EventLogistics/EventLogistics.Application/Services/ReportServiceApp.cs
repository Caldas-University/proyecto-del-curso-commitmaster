using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Repositories;
using System.Linq;

namespace EventLogistics.Application.Services
{
    public class ReportServiceApp : IReportServiceApp
    {
        private readonly IReportRepository _reportRepository;

        public ReportServiceApp(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<List<ResourceDto>> GenerateReportAsync(int? eventId, string? resourceType, string? status)
        {
            var resources = await _reportRepository.GenerateReportAsync(eventId, resourceType, status);
            
            return resources.Select(r => new ResourceDto
            {
                Id = r.Id,
                Type = r.Type,
                Availability = r.Availability,
                Capacity = r.Capacity,
                Assignments = r.Assignments
            }).ToList();
        }
    }
}
