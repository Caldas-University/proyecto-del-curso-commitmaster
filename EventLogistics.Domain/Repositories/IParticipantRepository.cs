using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories;

public interface IParticipantRepository
{
    Task<Participant?> GetByIdAsync(Guid id);
    Task<Participant?> GetByDocumentAsync(string document);
    Task<Participant?> GetByEmailAsync(string email);
    Task AddAsync(Participant participant);
    Task<List<Participant>> GetAllAsync();
}