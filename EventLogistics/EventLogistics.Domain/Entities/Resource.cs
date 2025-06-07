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

    public void UpdateAvailability(bool isAvailable)
    {
        // Aquí puedes incluir la lógica de validación necesaria antes de cambiar la disponibilidad
        // También puedes generar eventos de dominio si es necesario

        // Si tienes un campo privado _availability, podrías actualizarlo así:
        // _availability = isAvailable;

        // Si estás usando un enfoque de propiedad de solo lectura con un campo respaldado:
        this.Availability = isAvailable; // (si tienes un modificador de acceso private set)
    }
}
