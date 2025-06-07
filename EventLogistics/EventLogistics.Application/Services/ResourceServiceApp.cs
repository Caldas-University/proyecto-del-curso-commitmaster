namespace EventLogistics.Application.Services;

using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ResourceServiceApp : IResourceServiceApp
{
    private readonly IResourceRepository _resourceRepository;

    public ResourceServiceApp(IResourceRepository resourceRepository)
    {
        _resourceRepository = resourceRepository;
    }

    public async Task<ResourceDto> CheckAvailabilityAsync(Guid resourceId)
    {
        var resource = await _resourceRepository.GetByIdAsync(resourceId);
        if (resource == null)
        {
            return null;
        }

        return new ResourceDto
        {
            Id = resource.Id,
            Type = resource.Type,
            Availability = resource.Availability,
            Capacity = resource.Capacity,
        };
    }

    public async Task<ResourceDto> UpdateResourceStatusAsync(Guid resourceId, string status)
    {
        var resource = await _resourceRepository.GetByIdAsync(resourceId);
        if (resource == null)
        {
            return null;
        }

        // En lugar de asignar directamente a la propiedad, deberíamos tener un método en la entidad Resource
        // que nos permita actualizar su disponibilidad
        bool isAvailable = status.ToLower() == "disponible" || status.ToLower() == "available";
        
        // Asumiendo que existe un método como este en la entidad Resource
        resource.UpdateAvailability(isAvailable);
        
        await _resourceRepository.UpdateAsync(resource);

        return new ResourceDto
        {
            Id = resource.Id,
            Type = resource.Type,
            Availability = resource.Availability,
            Capacity = resource.Capacity,
            Status = resource.Availability ? "Disponible" : "No disponible" // Derivamos Status de Availability
        };
    }

    public async Task<bool> AssignResourceAsync(Guid resourceId, Guid eventId)
    {
        var result = await _resourceRepository.AssignResourceAsync(resourceId, eventId);
        return result;
    }
}