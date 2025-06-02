namespace EventLogistics.Domain.Entities;

public class Resource
{
    public Guid Id { get; private set; }
    public string Type { get; private set; }
    public bool Availability { get; private set; }
    public int Capacity { get; private set; }
    public List<Guid> Assignments { get; private set; }

    private Resource()
    {
        Type = string.Empty;
        Assignments = new List<Guid>();
    }

    public Resource(string type, int capacity, bool availability = true)
    {
        Id = Guid.NewGuid();
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Capacity = capacity;
        Availability = availability;
        Assignments = new List<Guid>();
    }
}
