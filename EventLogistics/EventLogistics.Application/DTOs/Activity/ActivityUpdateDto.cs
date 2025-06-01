namespace EventLogistics.Application.DTOs
{
    public class ActivityUpdateDto
    {
        public string ActivityName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}