namespace EventLogistics.Application.DTOs
{
    public class AttendanceResponseDto
    {
        public int ParticipantId { get; set; }
        public string? FullName { get; set; }
        public string? AccessType { get; set; }
        public DateTime CheckInTime { get; set; }
        public bool RegistrationComplete { get; set; }
        public string? Message { get; set; }
    }
}
