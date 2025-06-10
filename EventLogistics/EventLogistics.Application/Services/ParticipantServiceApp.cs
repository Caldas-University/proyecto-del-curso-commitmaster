using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using EventLogistics.Application.Mappers;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

namespace EventLogistics.Application.Services;

public class ParticipantServiceApp : IParticipantServiceApp
{
    private readonly IParticipantRepository _participantRepository;

    public ParticipantServiceApp(IParticipantRepository participantRepository)
    {
        _participantRepository = participantRepository;
    }

    public async Task<ParticipantDto?> GetByIdAsync(Guid id)
    {
        var participant = await _participantRepository.GetByIdAsync(id);
        return participant != null ? ParticipantMapper.ToDto(participant) : null;
    }

    public async Task<ParticipantDto?> GetByDocumentAsync(string document)
    {
        var participant = await _participantRepository.GetByDocumentAsync(document);
        return participant != null ? ParticipantMapper.ToDto(participant) : null;
    }

    public async Task<ParticipantDto?> GetByEmailAsync(string email)
    {
        var participant = await _participantRepository.GetByEmailAsync(email);
        return participant != null ? ParticipantMapper.ToDto(participant) : null;
    }

    public async Task<List<ParticipantDto>> GetAllAsync()
    {
        var participants = await _participantRepository.GetAllAsync();
        return participants.Select(ParticipantMapper.ToDto).ToList();
    }

    public async Task<ParticipantDto> CreateAsync(ParticipantDto participantDto)
    {
        var participant = ParticipantMapper.ToEntity(participantDto);
        await _participantRepository.AddAsync(participant);
        return ParticipantMapper.ToDto(participant);
    }

    public async Task<ParticipantDto> UpdateAsync(ParticipantDto participantDto)
    {
        var existingParticipant = await _participantRepository.GetByIdAsync(participantDto.Id);
        if (existingParticipant == null)
            throw new InvalidOperationException("Participant not found");

        // Actualizar propiedades
        // Nota: Necesitarías añadir métodos de actualización en la entidad Participant
        // O crear una nueva instancia con los nuevos datos
        
        return participantDto;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var participant = await _participantRepository.GetByIdAsync(id);
            if (participant == null)
                return false;

            // Note: Necesitarías implementar Delete en el repositorio
            // await _participantRepository.DeleteAsync(id);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
