namespace EventLogistics.Domain.Entities;

public class Event
{
    public Guid Id { get; private set; }
    public string Place { get; private set; }
    public DateTime Schedule { get; private set; }
    public List<Guid> Resources { get; private set; }
    public string Status { get; private set; }

    private Event()
    {
<<<<<<< HEAD
        Place = string.Empty;
        Resources = new List<Guid>();
        Status = string.Empty;
=======
        public required string Name { get; set; }
        public string Place { get; set; }
        public DateTime Schedule { get; set; }
        public string Status { get; set; }
        
        // Navigation properties
        public virtual ICollection<ResourceAssignment> Resources { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
>>>>>>> sebas
    }

    public Event(string place, DateTime schedule, string status = "Activo")
    {
        Id = Guid.NewGuid();
        Place = place ?? throw new ArgumentNullException(nameof(place));
        Schedule = schedule;
        Resources = new List<Guid>();
        Status = status ?? throw new ArgumentNullException(nameof(status));
    }
}
