using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories;

public interface IParticipantActivityRepository
{
    Task<bool> AnyAsync(Func<ParticipantActivity, bool> predicate);
    Task<List<ParticipantActivity>> GetByParticipantAndEventAsync(Guid participantId, Guid eventId);
    Task<List<Activity>> GetAvailableActivitiesForEventAsync(Guid participantId, Guid eventId);
}