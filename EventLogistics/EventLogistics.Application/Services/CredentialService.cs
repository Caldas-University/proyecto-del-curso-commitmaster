using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;

namespace EventLogistics.Application.Services
{
    public class CredentialService : ICredentialService
    {
        private readonly IRepository<Participant> _participantRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IBadgeGenerator _badgeGenerator;
        // private readonly IRepository<CredentialHistory> _credentialHistoryRepository;

        public CredentialService(
            IRepository<Participant> participantRepository,
            IActivityRepository activityRepository,
            IRepository<Event> eventRepository,
            IBadgeGenerator badgeGenerator
            // IRepository<CredentialHistory> credentialHistoryRepository
        )
        {
            _participantRepository = participantRepository;
            _activityRepository = activityRepository;
            _eventRepository = eventRepository;
            _badgeGenerator = badgeGenerator;
            // _credentialHistoryRepository = credentialHistoryRepository;
        }

        public async Task<byte[]> GenerateCredentialAsync(int participantId)
        {
            var participant = await _participantRepository.GetByIdAsync(participantId)
                ?? throw new ArgumentException($"Participante con ID {participantId} no encontrado");

            var eventEntity = await _eventRepository.GetByIdAsync(participant.EventId)
                ?? throw new ArgumentException($"Evento con ID {participant.EventId} no encontrado");

            if (!participant.IsRegistrationComplete)
                throw new InvalidOperationException("La inscripción del participante no está completa.");

            var badgeData = new BadgeData
            {
                ParticipantName = participant.FullName,
                AccessType = participant.AccessType,
                QRCodeContent = participant.QrCode
            };

            try
            {
                return await _badgeGenerator.GenerateAsync(badgeData);

                // Si decides volver a registrar historial, descomenta esto:
                /*
                await _credentialHistoryRepository.AddAsync(new CredentialHistory
                {
                    ParticipantId = participantId,
                    Action = "CredentialGenerated",
                    ActionTimestamp = DateTime.UtcNow,
                    Details = $"Credencial generada para {participant.FullName}",
                    Result = "Success",
                    CreatedBy = "System",
                    UpdatedBy = "System"
                });
                */
            }
            catch (Exception ex)
            {
                // Si decides guardar errores de historial:
                /*
                await _credentialHistoryRepository.AddAsync(new CredentialHistory
                {
                    ParticipantId = participantId,
                    Action = "CredentialGenerationFailed",
                    ActionTimestamp = DateTime.UtcNow,
                    Details = ex.Message,
                    Result = "Error",
                    CreatedBy = "System",
                    UpdatedBy = "System"
                });
                */
                throw;
            }
        }

        public async Task<IEnumerable<Activity>> GetPersonalizedScheduleAsync(int participantId)
        {
            var activities = await _activityRepository.GetByParticipantIdAsync(participantId);
            return activities?.OrderBy(a => a.StartTime).ToList() ?? new List<Activity>();
        }
    }

    public class BadgeData
    {
        public string ParticipantName { get; set; } = string.Empty;
        public string AccessType { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string QRCodeContent { get; set; } = string.Empty;
    }

    public interface IBadgeGenerator
    {
        Task<byte[]> GenerateAsync(BadgeData badgeData);
    }

    // Solo descomenta esta clase si la vas a usar luego:
    /*
    public class CredentialHistory : BaseEntity
    {
        public int ParticipantId { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime ActionTimestamp { get; set; }
        public string Details { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
    }
    */
}
