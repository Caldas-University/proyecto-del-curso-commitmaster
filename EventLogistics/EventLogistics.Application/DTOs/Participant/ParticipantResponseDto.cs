namespace EventLogistics.Application.DTOs
{
    public class ParticipantResponseDto
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public string AccessType { get; set; }
        public bool RegistrationComplete { get; set; }

        public ParticipantResponseDto()
        {
            FullName = string.Empty;
            AccessType = string.Empty;
        }
    }
}