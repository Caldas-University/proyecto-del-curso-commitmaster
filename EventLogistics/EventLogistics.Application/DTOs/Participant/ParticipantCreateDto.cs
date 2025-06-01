namespace EventLogistics.Application.DTOs
{
    public class ParticipantCreateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string AccessType { get; set; } = string.Empty;
        public string QrCode { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public int EventId { get; set; }
        // Agrega solo los campos simples necesarios
    }
}