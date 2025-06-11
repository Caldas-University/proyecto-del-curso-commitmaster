using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLogistics.Application.Services
{
    public class ReportServiceApp : IReportServiceApp
    {
        private readonly IReportRepository _reportRepository;
        private const int UMBRAL_CRITICO = 2; // Puedes ajustar este valor o cargarlo de configuración

        public ReportServiceApp(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<List<ResourceReportDto>> GenerateReportAsync(Guid? eventId, string? resourceType, string? status)
        {
            var resources = await _reportRepository.GenerateReportAsync(eventId, resourceType, status);
            var report = new List<ResourceReportDto>();

            foreach (var resource in resources)
            {
                var eventos = resource.ResourceAssignments?
                    .Where(ra => ra.Event != null)
                    .Select(ra => ra.Event.Name)
                    .Distinct()
                    .ToList() ?? new List<string>();

                var actividades = resource.ResourceAssignments?
                    .Where(ra => ra.Activity != null)
                    .Select(ra => ra.Activity.Name)
                    .Distinct()
                    .ToList() ?? new List<string>();

                report.Add(new ResourceReportDto
                {
                    Id = resource.Id,
                    Nombre = resource.Name,
                    Tipo = resource.Type,
                    CantidadTotal = resource.Capacity,
                    CantidadUtilizada = resource.ResourceAssignments?.Count ?? 0,
                    CantidadDisponible = resource.Capacity - (resource.ResourceAssignments?.Count ?? 0),
                    Eventos = eventos,
                    Actividades = actividades,
                    UsoTotal = resource.ResourceAssignments?.Count ?? 0,
                    Disponible = resource.Availability
                });
            }

            return report;
        }

        public async Task<List<ResourceReportDto>> GetCriticalResourcesAsync()
        {
            var resources = await _reportRepository.GenerateReportAsync(null, null, null);
            var critical = new List<ResourceReportDto>();
            const int UMBRAL_CRITICO = 2;

            foreach (var resource in resources)
            {
                int disponibles = resource.Capacity - (resource.ResourceAssignments?.Count ?? 0);
                if (disponibles <= UMBRAL_CRITICO)
                {
                    critical.Add(new ResourceReportDto
                    {
                        Id = resource.Id,
                        Nombre = resource.Name,
                        Tipo = resource.Type,
                        CantidadTotal = resource.Capacity,
                        CantidadUtilizada = resource.ResourceAssignments?.Count ?? 0,
                        CantidadDisponible = disponibles,
                        Eventos = resource.ResourceAssignments?
                            .Where(ra => ra.Event != null)
                            .Select(ra => ra.Event.Name)
                            .Distinct()
                            .ToList() ?? new List<string>(),
                        Actividades = resource.ResourceAssignments?
                            .Where(ra => ra.Activity != null)
                            .Select(ra => ra.Activity.Name)
                            .Distinct()
                            .ToList() ?? new List<string>(),
                        UsoTotal = resource.ResourceAssignments?.Count ?? 0,
                        Disponible = resource.Availability
                    });
                }
            }
            return critical;
        }
    }
}
