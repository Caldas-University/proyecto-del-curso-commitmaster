namespace EventLogistics.Application.DTOs
{
    public class ResourceMetricsDto
    {
        // public string Name { get; set; } // Elimina o comenta esta línea
        public Guid Id { get; set; } // Si quieres mostrar el Id
        public string Type { get; set; }
        public int Total { get; set; }
        public int Utilized { get; set; }
        public int Available { get; set; }
        public int Events { get; set; }
        public int Activities { get; set; }
        public int TotalUsage { get; set; }
        public bool Availability { get; set; }
    }
}