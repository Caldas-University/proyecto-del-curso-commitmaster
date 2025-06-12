namespace EventLogistics.Domain.Entities;

public class Participant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Document { get; private set; }
    public string Email { get; private set; }
    public string AccessType { get; private set; } // Ej: Asistente, Ponente, VIP

    private Participant() { }

    public Participant(string name, string document, string email, string accessType)
    {
        Id = Guid.NewGuid();
        Name = name;
        Document = document;
        Email = email;
        AccessType = accessType;
    }

    /// <summary>
    /// Constructor para reconstrucción desde persistencia o mapeo.
    /// </summary>
    public Participant(Guid id, string name, string document, string email, string accessType)
    {
        Id = id;
        Name = name;
        Document = document;
        Email = email;
        AccessType = accessType;
    }

    public void Update(string name, string document, string email, string accessType)
    {
        Name = name;
        Document = document;
        Email = email;
        AccessType = accessType;
    }
}