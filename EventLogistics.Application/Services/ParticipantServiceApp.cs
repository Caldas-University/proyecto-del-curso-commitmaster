using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Application.Services
{
    public class ParticipantServiceApp : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;

        public ParticipantServiceApp(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task<ParticipantDto?> GetByIdAsync(Guid id)
        {
            var participant = await _participantRepository.GetByIdAsync(id);
            if (participant == null) return null;
            return ToDto(participant);
        }

        public async Task<ParticipantDto?> GetByDocumentAsync(string document)
        {
            var participant = await _participantRepository.GetByDocumentAsync(document);
            if (participant == null) return null;
            return ToDto(participant);
        }

        public async Task<ParticipantDto?> GetByEmailAsync(string email)
        {
            var participant = await _participantRepository.GetByEmailAsync(email);
            if (participant == null) return null;
            return ToDto(participant);
        }

        public async Task<List<ParticipantDto>> GetAllAsync()
        {
            var participants = await _participantRepository.GetAllAsync();
            var dtos = new List<ParticipantDto>();
            foreach (var p in participants)
            {
                dtos.Add(ToDto(p));
            }
            return dtos;
        }

        public async Task<ParticipantDto> CreateAsync(ParticipantDto dto)
        {
            var participant = new Participant(dto.Name, dto.Document, dto.Email, dto.AccessType);
            await _participantRepository.AddAsync(participant);
            dto.Id = participant.Id;
            return dto;
        }

        public async Task<ParticipantDto?> UpdateAsync(Guid id, ParticipantDto dto)
        {
            var participant = await _participantRepository.GetByIdAsync(id);
            if (participant == null) return null;

            participant.Update(dto.Name, dto.Document, dto.Email, dto.AccessType);
            await _participantRepository.UpdateAsync(participant);

            // Devuelve un nuevo DTO actualizado
            return new ParticipantDto
            {
                Id = participant.Id,
                Name = participant.Name,
                Document = participant.Document,
                Email = participant.Email,
                AccessType = participant.AccessType
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var participant = await _participantRepository.GetByIdAsync(id);
            if (participant == null) return false;
            await _participantRepository.DeleteAsync(id);
            return true;
        }

        private static ParticipantDto ToDto(Participant participant)
        {
            return new ParticipantDto
            {
                Id = participant.Id,
                Name = participant.Name,
                Document = participant.Document,
                Email = participant.Email,
                AccessType = participant.AccessType
            };
        }
    }
}