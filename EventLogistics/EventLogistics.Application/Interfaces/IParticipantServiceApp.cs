using EventLogistics.Application.DTOs;

namespace EventLogistics.Application.Interfaces;

public interface IParticipantServiceApp
{
    Task<ParticipantDto?> GetByIdAsync(Guid id);
    Task<ParticipantDto?> GetByDocumentAsync(string document);
    Task<ParticipantDto?> GetByEmailAsync(string email);
    Task<List<ParticipantDto>> GetAllAsync();
    Task<ParticipantDto> CreateAsync(CreateParticipantRequest participantRequest);
    Task<ParticipantDto> UpdateAsync(ParticipantDto participantDto);
    Task<bool> DeleteAsync(Guid id);
}
