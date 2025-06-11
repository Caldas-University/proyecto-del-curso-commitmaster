using EventLogistics.Application.DTOs;

namespace EventLogistics.Application.Interfaces
{
    public interface IOrganizatorService
    {
        Task<OrganizatorDto> CreateAsync(CreateOrganizatorRequest organizatorRequest);
        Task<OrganizatorDto?> GetByIdAsync(Guid id);
        Task<List<OrganizatorDto>> GetAllAsync();
        Task<List<OrganizatorDto>> GetByRoleAsync(string role);
    }
}
