using EventLogistics.Domain.Entities;

namespace EventLogistics.Application.Interfaces;
public interface IParticipantActivityService
{
    // ...otros métodos...
    Task<List<Activity>> GetAvailableActivitiesForEventAsync(Guid participantId, Guid eventId);
}