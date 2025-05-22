using EventLogistics.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Domain.Repositories
{
    public interface IParticipantRepository : IRepository<Participant>
    {
        Task<Participant> GetByDocumentAsync(string document);
        Task<Participant> GetByQrCodeAsync(string qrCode);
        Task<IEnumerable<Participant>> GetByAccessTypeAsync(string accessType);
    }
}
