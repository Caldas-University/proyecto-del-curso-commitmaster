namespace EventLogistics.Application.DTOs
{
    public class CredentialResponseDto
    {
        public string ParticipantName { get; set; } = string.Empty;
        public string AccessType { get; set; } = string.Empty;
        public byte[] CredentialPdf { get; set; } = Array.Empty<byte>();
        //agregamos schuedule personalizado

        
        public PersonalizedScheduleDto Schedule { get; set; }
    }
}


