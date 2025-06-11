namespace EventLogistics.Application.DTOs
{
    public class TimeSuggestionDto
    {
        public DateTime SuggestedStartTime { get; set; }
        public DateTime SuggestedEndTime { get; set; }
        public string Reason { get; set; } = string.Empty;
        public double CompatibilityScore { get; set; }
        public bool IsOptimal { get; set; }
        public List<Guid> AvailableResourceIds { get; set; } = new();
    }
}
