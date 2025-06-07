using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Repositories;
using EventLogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLogistics.Application.Services
{
    public class ResourceServiceApp : IResourceServiceApp
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceServiceApp(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<ResourceDto?> CheckAvailabilityAsync(Guid resourceId)
        {
            var resource = await _resourceRepository.GetByIdAsync(resourceId);
            if (resource == null) return null;

            return new ResourceDto
            {
                Id = resource.Id,
                Type = resource.Type,
                Availability = resource.Availability,
                Capacity = resource.Capacity,
                Assignments = resource.Assignments
            };
        }

        public async Task<ResourceDto?> UpdateResourceStatusAsync(Guid resourceId, string status)
        {
            var resource = await _resourceRepository.GetByIdAsync(resourceId);
            if (resource == null) return null;

            bool isAvailable = status.ToLower() == "disponible";
            resource.UpdateAvailability(isAvailable);
            await _resourceRepository.UpdateAsync(resource);

            return new ResourceDto
            {
                Id = resource.Id,
                Type = resource.Type,
                Availability = resource.Availability,
                Capacity = resource.Capacity,
                Assignments = resource.Assignments
            };
        }

        public async Task<bool> AssignResourceAsync(Guid resourceId, Guid eventId)
        {
            return await _resourceRepository.AssignResourceAsync(resourceId, eventId);
        }

        public async Task<bool> ReassignResourcesAsync(List<Guid> resourceIds, List<Guid> newEventIds)
        {
            // Implementación básica de reasignación
            try
            {
                for (int i = 0; i < resourceIds.Count && i < newEventIds.Count; i++)
                {
                    await _resourceRepository.AssignResourceAsync(resourceIds[i], newEventIds[i]);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ResourceDto>> GetAvailableResourcesAsync()
        {
            var resources = await _resourceRepository.GetAvailableResourcesAsync();
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