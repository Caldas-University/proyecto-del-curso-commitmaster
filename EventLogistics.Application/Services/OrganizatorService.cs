using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

namespace EventLogistics.Application.Services
{
    public class OrganizatorService : IOrganizatorService
    {
        private readonly IOrganizatorRepository _organizatorRepository;

        public OrganizatorService(IOrganizatorRepository organizatorRepository)
        {
            _organizatorRepository = organizatorRepository;
        }        public async Task<OrganizatorDto> CreateAsync(CreateOrganizatorRequest organizatorRequest)
        {
            var organizator = new Organizator(
                organizatorRequest.Name,
                organizatorRequest.Email,
                organizatorRequest.Phone,
                organizatorRequest.Role
            );

            await _organizatorRepository.AddAsync(organizator);

            return new OrganizatorDto
            {
                Id = organizator.Id,
                Name = organizator.Name,
                Email = organizator.Email,
                Phone = organizator.Phone,
                Role = organizator.Role
            };
        }

        public async Task<OrganizatorDto?> GetByIdAsync(Guid id)
        {
            var organizator = await _organizatorRepository.GetByIdAsync(id);
            if (organizator == null) return null;

            return new OrganizatorDto
            {
                Id = organizator.Id,
                Name = organizator.Name,
                Email = organizator.Email,
                Phone = organizator.Phone,
                Role = organizator.Role
            };
        }

        public async Task<List<OrganizatorDto>> GetAllAsync()
        {
            var organizators = await _organizatorRepository.GetAllAsync();
            return organizators.Select(o => new OrganizatorDto
            {
                Id = o.Id,
                Name = o.Name,
                Email = o.Email,
                Phone = o.Phone,
                Role = o.Role
            }).ToList();
        }

        public async Task<List<OrganizatorDto>> GetByRoleAsync(string role)
        {
            var organizators = await _organizatorRepository.GetByRoleAsync(role);
            return organizators.Select(o => new OrganizatorDto
            {
                Id = o.Id,
                Name = o.Name,
                Email = o.Email,
                Phone = o.Phone,
                Role = o.Role
            }).ToList();
        }
    }
}
