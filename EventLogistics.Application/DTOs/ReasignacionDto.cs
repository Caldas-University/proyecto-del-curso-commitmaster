namespace EventLogistics.Application.DTOs;

public class ReasignacionDto
{
    public Guid Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public List<Guid> RecursosAfectadosIds { get; set; } = new();
    public List<Guid> ActividadesAfectadasIds { get; set; } = new();
}
