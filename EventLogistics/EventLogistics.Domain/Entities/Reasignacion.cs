namespace EventLogistics.Domain.Entities;

public class Reasignacion
{
    public Guid Id { get; private set; }
    public string Tipo { get; private set; }
    public string Estado { get; private set; }
    public List<Guid> RecursosAfectadosIds { get; private set; }
    public List<Guid> ActividadesAfectadasIds { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    private Reasignacion()
    {
        Tipo = string.Empty;
        Estado = string.Empty;
        RecursosAfectadosIds = new List<Guid>();
        ActividadesAfectadasIds = new List<Guid>();
    }

    public Reasignacion(string tipo, string estado = "Iniciada")
    {
        Id = Guid.NewGuid();
        Tipo = tipo ?? throw new ArgumentNullException(nameof(tipo));
        Estado = estado ?? throw new ArgumentNullException(nameof(estado));
        RecursosAfectadosIds = new List<Guid>();
        ActividadesAfectadasIds = new List<Guid>();
        FechaCreacion = DateTime.UtcNow;
    }
}
