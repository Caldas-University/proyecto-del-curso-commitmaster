namespace EventLogistics.Domain.Entities;

public class Activity
{
    public Guid Id { get; private set; }
    public string Lugar { get; private set; }
    public DateTime Horario { get; private set; }
    public string Estado { get; private set; }
    public Guid EventId { get; private set; }

    private Activity()
    {
        Lugar = string.Empty;
        Estado = string.Empty;
    }

    public Activity(Guid eventId, string lugar, DateTime horario, string estado = "Programada")
    {
        Id = Guid.NewGuid();
        EventId = eventId;
        Lugar = lugar ?? throw new ArgumentNullException(nameof(lugar));
        Horario = horario;
        Estado = estado ?? throw new ArgumentNullException(nameof(estado));
    }
}
