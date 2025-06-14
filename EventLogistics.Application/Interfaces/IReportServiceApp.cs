using EventLogistics.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Application.Interfaces
{
    public interface IReportServiceApp
    {
        Task<List<ResourceReportDto>> GenerateReportAsync(Guid? eventId, string? resourceType, string? status);
        Task<List<ResourceReportDto>> GetCriticalResourcesAsync();
    }
}
