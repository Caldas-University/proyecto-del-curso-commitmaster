using EventLogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventLogistics.Domain.Repositories
{
    /// <summary>
    /// Repositorio para operaciones de persistencia relacionadas con las actividades de los participantes.
    /// </summary>
    public interface IParticipantActivityRepository
    {
        /// <summary>
        /// Verifica si existe alguna relación participante-actividad que cumpla con el predicado.
        /// </summary>
        Task<bool> AnyAsync(Expression<Func<ParticipantActivity, bool>> predicate);

        /// <summary>
        /// Obtiene todas las inscripciones de un participante en un evento.
        /// </summary>
        Task<List<ParticipantActivity>> GetByParticipantAndEventAsync(Guid participantId, Guid eventId);

        /// <summary>
        /// Obtiene la lista de actividades disponibles para un participante en un evento.
        /// </summary>
        Task<List<Activity>> GetAvailableActivitiesForEventAsync(Guid participantId, Guid eventId);

        /// <summary>
        /// Obtiene todas las inscripciones de participantes a actividades.
        /// </summary>
        Task<List<ParticipantActivity>> GetAllAsync();

        /// <summary>
        /// Agrega una nueva inscripción de participante a actividad.
        /// </summary>
        Task AddAsync(ParticipantActivity participantActivity);

        /// <summary>
        /// Elimina la inscripción de un participante a una actividad.
        /// </summary>
        Task DeleteAsync(Guid participantId, Guid activityId);

        /// <summary>
        /// Obtiene una inscripción específica por participante y actividad.
        /// </summary>
        Task<ParticipantActivity?> GetByParticipantAndActivityAsync(Guid participantId, Guid activityId);
    }
}