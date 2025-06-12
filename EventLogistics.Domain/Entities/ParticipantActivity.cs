namespace EventLogistics.Domain.Entities;

public class ParticipantActivity
{
    public Guid Id { get; private set; }
    public Guid ParticipantId { get; private set; }
    public Guid ActivityId { get; private set; }
    public DateTime RegistrationDate { get; private set; }

    // Relaciones de navegación (opcional)
    public Participant? Participant { get; private set; }
    public Activity Activity { get; private set; }

    private ParticipantActivity() { }

    /// <summary>
    /// Constructor para registrar una nueva inscripción a una actividad.
    /// </summary>
    public ParticipantActivity(Guid participantId, Guid activityId, Activity activity, string? qrCode = null)
    {
        Id = Guid.NewGuid();
        ParticipantId = participantId;
        ActivityId = activityId;
        Activity = activity;
        
        RegistrationDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Constructor para reconstrucción desde persistencia o mapeo.
    /// </summary>
    public ParticipantActivity(Guid id, Guid participantId, Guid activityId, DateTime registrationDate, Activity activity)
    {
        Id = id;
        ParticipantId = participantId;
        ActivityId = activityId;
        RegistrationDate = registrationDate;
        Activity = activity;
    }
}