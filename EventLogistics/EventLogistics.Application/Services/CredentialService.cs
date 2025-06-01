using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

namespace EventLogistics.Application.Services
{
    public interface ICredentialService
    {
        Task<byte[]> GenerateCredentialAsync(int participantId);
        Task<PersonalizedScheduleDto?> GetPersonalizedScheduleAsync(int participantId);
        Task<BadgeDataDto> GenerateCredentialDataAsync(int participantId);
        Task AddAsync(Credential credential);
        Task UpdateAsync(Credential credential);
        Task DeleteAsync(int id);
        Task<Credential> GetByIdAsync(int id);
        Task<Credential> GetByParticipantIdAsync(int participantId);
        Task<IEnumerable<Credential>> GetAllAsync();
        Task<CredentialResponseDto> GetCredentialResponseAsync(int participantId);
    }

    public class CredentialService : ICredentialService
    {
        private readonly ICredentialRepository _credentialRepository;

        public CredentialService(
            ICredentialRepository credentialRepository
        )
        {
            _credentialRepository = credentialRepository;
        }

        public async Task<byte[]> GenerateCredentialAsync(int participantId)
        {
            var credential = await _credentialRepository.GetByParticipantIdAsync(participantId);
            if (credential == null)
                return Array.Empty<byte>();

            // Aquí deberías usar IBadgeGenerator o lógica propia para generar el PDF
            // return await _badgeGenerator.GenerateAsync(new BadgeData { ... });

            // Por ahora, solo retorna un arreglo vacío para evitar la excepción
            return Array.Empty<byte>();
        }

        public async Task<PersonalizedScheduleDto?> GetPersonalizedScheduleAsync(int participantId)
        {
            var participant = await _credentialRepository.GetPersonalizedScheduleAsync(participantId);

            if (participant == null || participant.ParticipantActivities == null || !participant.ParticipantActivities.Any())
                return null;

            var schedule = new PersonalizedScheduleDto
            {
                ParticipantName = participant.FullName,
                Activities = participant.ParticipantActivities
                    .Where(pa => pa.Activity != null)
                    .Select(pa => new ActivityScheduleDto
                    {
                        ActivityName = pa.Activity.Name,
                        StartTime = pa.Activity.StartTime,
                        EndTime = pa.Activity.EndTime,
                        Location = pa.Activity.Location ?? "No disponible",
                    }).ToList()
            };

            return schedule;
        }

        public Task AddAsync(Credential credential)
        {
            return _credentialRepository.AddAsync(credential);
        }

        public Task UpdateAsync(Credential credential)
        {
            return _credentialRepository.UpdateAsync(credential);
        }

        public Task DeleteAsync(int id)
        {
            return _credentialRepository.DeleteAsync(id);
        }

        public Task<Credential> GetByIdAsync(int id)
        {
            return _credentialRepository.GetByIdAsync(id);
        }

        public async Task<Credential> GetByParticipantIdAsync(int participantId)
        {
            return await _credentialRepository.GetByParticipantIdAsync(participantId);
        }

        public Task<IEnumerable<Credential>> GetAllAsync()
        {
            return _credentialRepository.GetAllAsync();
        }

        public async Task<BadgeDataDto> GenerateCredentialDataAsync(int participantId)
        {
            var credential = await _credentialRepository.GetByParticipantIdAsync(participantId);
            if (credential == null)
                return null;

            var participant = credential.Participant;
            var eventInfo = participant?.Event;

            return new BadgeDataDto
            {
                ParticipantName = participant?.FullName ?? "Desconocido",
                AccessType = credential.AccessType,
                EventName = eventInfo?.Place ?? "Evento no disponible",
                QrCode = participant?.QrCode ?? "QR no disponible",
                IssuedAt = credential.IssuedAt,
                Printed = credential.Printed
            };
        }

        public async Task<CredentialResponseDto> GetCredentialResponseAsync(int participantId)
        {
            var credential = await _credentialRepository.GetByParticipantIdAsync(participantId);
            if (credential == null)
                return null;

            var participant = credential.Participant;
            var schedule = await GetPersonalizedScheduleAsync(participantId);
            var pdf = await GenerateCredentialAsync(participantId);

            return CredentialMapper.ToResponseDto(participant, pdf, schedule);
        }
    }
}
