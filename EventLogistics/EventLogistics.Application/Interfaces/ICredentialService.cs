using EventLogistics.Application.DTOs;
using EventLogistics.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Application.Interfaces
{
    using EventLogistics.Domain.Entities;
    using System.Threading.Tasks;

    public interface ICredentialService
    {
        Task<byte[]> GenerateCredentialAsync(int participantId);
        Task<PersonalizedScheduleDto?> GetPersonalizedScheduleAsync(int participantId);
        Task<BadgeDataDto> GenerateCredentialDataAsync(int participantId);
    }
}
