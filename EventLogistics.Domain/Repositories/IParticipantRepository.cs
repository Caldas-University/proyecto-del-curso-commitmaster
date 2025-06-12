using EventLogistics.Domain.Entities;

namespace EventLogistics.Domain.Repositories;

/// <summary>
/// Repositorio para operaciones de persistencia de participantes.
/// </summary>
public interface IParticipantRepository
{
    /// <summary>
    /// Obtiene un participante por su Id.
    /// </summary>
    Task<Participant?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtiene un participante por su documento de identidad.
    /// </summary>
    Task<Participant?> GetByDocumentAsync(string document);

    /// <summary>
    /// Obtiene un participante por su correo electrónico.
    /// </summary>
    Task<Participant?> GetByEmailAsync(string email);

    /// <summary>
    /// Agrega un nuevo participante.
    /// </summary>
    Task AddAsync(Participant participant);

    /// <summary>
    /// Obtiene todos los participantes.
    /// </summary>
    Task<List<Participant>> GetAllAsync();

    /// <summary>
    /// Actualiza los datos de un participante.
    /// </summary>
    Task UpdateAsync(Participant participant);

    /// <summary>
    /// Elimina un participante por su Id.
    /// </summary>
    Task DeleteAsync(Guid id);
}