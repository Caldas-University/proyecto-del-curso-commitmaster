namespace EventLogistics.Application.DTOs
{
    public class ActivityResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        // Agrega más propiedades si tu entidad Activity las tiene
    }
}