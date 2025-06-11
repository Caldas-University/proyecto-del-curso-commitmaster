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

    public ParticipantActivity(Guid participantId, Guid activityId, Activity activity)
    {
        Id = Guid.NewGuid();
        ParticipantId = participantId;
        ActivityId = activityId;
        Activity = activity;
        RegistrationDate = DateTime.UtcNow;
    }
}