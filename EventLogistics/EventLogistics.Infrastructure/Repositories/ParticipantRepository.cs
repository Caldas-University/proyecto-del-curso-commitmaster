using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventLogistics.Infrastructure.Repositories
{
    public class ParticipantRepository : Repository<Participant>, IParticipantRepository
    {
        public ParticipantRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Participant?> GetByQrCodeAsync(string qrCode)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.QrCode == qrCode);
        }

        public async Task<Participant?> GetByDocumentAsync(string document)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Document == document);
        }

        public async Task<IEnumerable<Participant>> GetByAccessTypeAsync(string accessType)
        {
            return await _dbSet
                .Where(p => p.AccessType == accessType)
                .ToListAsync();
        }
    }
}
