using System.Collections.Generic;
using System.Threading.Tasks;
using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories
{
    public interface ICredentialService
    {
        Task<byte[]> GenerateCredentialAsync(int participantId);
        Task<IEnumerable<Activity>> GetPersonalizedScheduleAsync(int participantId);
    }
}
