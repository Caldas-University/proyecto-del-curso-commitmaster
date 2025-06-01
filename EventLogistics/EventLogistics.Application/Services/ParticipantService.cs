using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventLogistics.Domain.Repositories;
using EventLogistics.Application.DTOs;
using EventLogistics.Application.Mappers;

public interface IParticipantService
{
    Task<IEnumerable<ParticipantResponseDto>> GetAllAsync();
    Task<ParticipantResponseDto?> GetByIdAsync(int id);
}

public class ParticipantService : IParticipantService
{
    private readonly IParticipantRepository _participantRepository;

    public ParticipantService(IParticipantRepository participantRepository)
    {
        _participantRepository = participantRepository;
    }

    public async Task<IEnumerable<ParticipantResponseDto>> GetAllAsync()
    {
        var participants = await _participantRepository.GetAllAsync();
        return participants.Select(ParticipantMapper.ToResponseDto);
    }

    public async Task<ParticipantResponseDto?> GetByIdAsync(int id)
    {
        var participant = await _participantRepository.GetByIdAsync(id);
        return participant == null ? null : ParticipantMapper.ToResponseDto(participant);
    }
}