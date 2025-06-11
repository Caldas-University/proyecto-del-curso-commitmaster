namespace EventLogistics.Application.DTOs
{
    public class ResourceReportDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int CantidadTotal { get; set; }
        public int CantidadUtilizada { get; set; }
        public int CantidadDisponible { get; set; }
        public List<string> Eventos { get; set; }
        public List<string> Actividades { get; set; }
        public int UsoTotal { get; set; }
        public bool Disponible { get; set; }
    }
}