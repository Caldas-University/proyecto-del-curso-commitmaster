namespace EventLogistics.Application.Interfaces;

using EventLogistics.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IResourceServiceApp
{
    Task <ResourceDto> CheckAvailabilityAsync(Guid resourceId);
    Task<ResourceDto> UpdateResourceStatusAsync(Guid resourceId, string status);
    Task<bool> AssignResourceAsync(Guid resourceId, Guid eventId);
}