namespace EventLogistics.Application.DTOs
{
    public class CredentialCreateDto
    {
        public int ParticipantId { get; set; }
        public string AccessType { get; set; } = string.Empty;
        public DateTime IssuedAt { get; set; }
        public bool Printed { get; set; } = false;
        public string? BadgeData { get; set; }
    }
}