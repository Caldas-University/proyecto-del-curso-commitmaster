using EventLogistics.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Application.Interfaces
{
    public interface IParticipantService
    {
        Task<ParticipantDto?> GetByIdAsync(Guid id);
        Task<ParticipantDto?> GetByDocumentAsync(string document);
        Task<ParticipantDto?> GetByEmailAsync(string email);
        Task<List<ParticipantDto>> GetAllAsync();
        Task<ParticipantDto> CreateAsync(ParticipantDto dto);
        Task<ParticipantDto?> UpdateAsync(Guid id, ParticipantDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}