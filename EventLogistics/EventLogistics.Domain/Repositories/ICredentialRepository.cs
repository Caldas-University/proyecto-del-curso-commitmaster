using EventLogistics.Domain.Entities;
using System.Threading.Tasks;

namespace EventLogistics.Domain.Repositories
{
    public interface ICredentialRepository
    {
        // metodo para generar credenciales
        // para un participante, devuelve un byte[] que representa la credencial
        Task<byte[]> GenerateCredentialAsync(int participantId);
        // metodo para obtener el horario personalizado de un participante
        // devuelve un objeto que contiene el horario
        Task<Participant?> GetPersonalizedScheduleAsync(int participantId);
        // CRUD operations
        Task AddAsync(Credential credential);
        Task UpdateAsync(Credential credential);
        Task DeleteAsync(int id);
        // aca se agregan los metodos para obtener credenciales
        //este metodo obtiene una credencial por el id del participante
        Task<Credential> GetByParticipantIdAsync(int participantId);
        // este metodo obtiene todas las credenciales
        Task<IEnumerable<Credential>> GetAllAsync();
        // este metodo obtiene una credencial por el id de la credencial
        Task<Credential> GetByIdAsync(int id);
    }
}