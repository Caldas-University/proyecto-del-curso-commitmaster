namespace EventLogistics.Domain.Entities;

public class Resource : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Availability { get; set; } = true;
    public int Capacity { get; set; }
    public List<Guid> Assignments { get; set; } = new List<Guid>();

    // Propiedades adicionales que podrían estar en migraciones
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public List<string> Tags { get; set; } = new List<string>();

    // Navigation properties
    public virtual ICollection<ResourceAssignment> ResourceAssignments { get; set; } = new List<ResourceAssignment>();
    public virtual ICollection<ReassignmentRule> ReassignmentRules { get; set; } = new List<ReassignmentRule>();

    public Resource()
    {
        Name = string.Empty;
        Type = string.Empty;
        Assignments = new List<Guid>();
        Tags = new List<string>();
    }

    public Resource(string name, string type, int capacity, bool availability = true)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Capacity = capacity;
        Availability = availability;
        Assignments = new List<Guid>();
        Tags = new List<string>();
    }

    public void UpdateAvailability(bool isAvailable)
    {
        // Aquí puedes incluir la lógica de validación necesaria antes de cambiar la disponibilidad
        // También puedes generar eventos de dominio si es necesario
        Availability = isAvailable;
    }
}
