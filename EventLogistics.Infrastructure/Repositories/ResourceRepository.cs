using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventLogistics.Infrastructure.Repositories
{    public class ResourceRepository : IResourceRepository
    {
        private readonly EventLogisticsDbContext _context;

        public ResourceRepository(EventLogisticsDbContext context)
        {
            _context = context;
        }

        public async Task<Resource?> GetByIdAsync(Guid id)
        {
            return await _context.Resources.FindAsync(id);
        }

        public async Task<IEnumerable<Resource>> GetAllAsync()
        {
            return await _context.Resources.ToListAsync();
        }

        public async Task<IEnumerable<Resource>> GetAvailableResourcesAsync()
        {
            return await _context.Resources.Where(r => r.Availability).ToListAsync();
        }

        public async Task<Resource> AddAsync(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();
            return resource;
        }

        public async Task UpdateAsync(Resource resource)
        {
            _context.Resources.Update(resource);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var resource = await GetByIdAsync(id);
            if (resource != null)
            {
                _context.Resources.Remove(resource);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> AssignResourceAsync(Guid resourceId, Guid eventId)
        {
            var resource = await GetByIdAsync(resourceId);
            if (resource != null && resource.Availability)
            {
                // Crear un nuevo ResourceAssignment
                var assignmentId = Guid.NewGuid();
                var assignment = new ResourceAssignment
                {
                    Id = assignmentId,
                    ResourceId = resourceId,
                    EventId = eventId,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow.AddHours(1),
                    Status = "Assigned",
                    AssignmentDate = DateTime.UtcNow,
                    Quantity = 1
                };
                
                _context.ResourceAssignments.Add(assignment);
                resource.Assignments.Add(assignmentId);
                await UpdateAsync(resource);
                return true;
            }
            return false;
        }

        public async Task<bool> CheckAvailabilityAsync(string resourceType, int quantity, DateTime date)
        {
            // Corregir: Resource no tiene FechaInicio ni FechaFin, usar solo Type y Availability
            var resources = await _context.Resources
                .Where(r => r.Type == resourceType && r.Availability)
                .ToListAsync();

            return resources.Any(r => r.Capacity >= quantity);
        }

        public async Task<bool> ReserveResourceAsync(Guid resourceId, int quantity)
        {
            var resource = await _context.Resources.FindAsync(resourceId);
            if (resource == null || resource.Capacity < quantity)
                return false;

            resource.Capacity -= quantity;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateResourceAvailability(Guid resourceId, DateTime start, DateTime end)
        {
            return await _context.Resources
                .Where(r => r.Id == resourceId)
                .AnyAsync(r => r.Availability && 
                              !_context.ResourceAssignments.Any(a => 
                                  a.ResourceId == resourceId &&
                                  a.Status != "Cancelled" &&
                                  a.StartTime < end && 
                                  a.EndTime > start));
        }
    }
}
