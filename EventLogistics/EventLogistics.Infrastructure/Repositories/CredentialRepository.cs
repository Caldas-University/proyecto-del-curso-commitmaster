using EventLogistics.Domain.Entities;
using EventLogistics.Domain.Repositories;
using EventLogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventLogistics.Infrastructure.Repositories
{
    public class CredentialRepository : Repository<Credential>, ICredentialRepository
    {
        public CredentialRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Credential> GetByParticipantIdAsync(int participantId)
        {
            return await _context.Credentials
                .Include(c => c.Participant)
                    .ThenInclude(p => p.Event) // <-- Esto agrega el evento del participante
                .FirstOrDefaultAsync(c => c.ParticipantId == participantId);
        }

        public async Task<IEnumerable<Credential>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Credential> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Id == id);
        }

        // Si tu clase base Repository<Credential> ya implementa AddAsync, UpdateAsync, DeleteAsync, puedes omitirlos aquí.
        // Si necesitas sobreescribirlos, asegúrate de que la firma coincida con la interfaz/base.

        public async Task AddAsync(Credential entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Credential entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        // Métodos de lógica de negocio que no corresponden al repositorio:
        public Task<byte[]> GenerateCredentialAsync(int participantId)
        {
            throw new System.NotImplementedException("Implementar en el servicio de aplicación.");
        }

        public async Task<Participant?> GetPersonalizedScheduleAsync(int participantId)
        {
            var participant = await _context.Participants
                .Include(p => p.ParticipantActivities)
                    .ThenInclude(pa => pa.Activity)
                .FirstOrDefaultAsync(p => p.Id == participantId);
            return participant;
        }
    }
}