public class ParticipantUpdateDto
{
    public string? FullName { get; set; }
    public string? AccessType { get; set; }
    public string? QrCode { get; set; }
    public string? Document { get; set; }
    public int EventId { get; set; }
    public bool IsRegistrationComplete { get; set; }
}