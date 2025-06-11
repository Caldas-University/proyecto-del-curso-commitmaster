namespace EventLogistics.Application.DTOs
{
    public class ResourceSuggestionDto
    {
        public Guid ResourceId { get; set; }
        public string ResourceType { get; set; } = string.Empty;
        public string ResourceName { get; set; } = string.Empty;
        public int AvailableQuantity { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableUntil { get; set; }
        public string Reason { get; set; } = string.Empty;
        public double CompatibilityScore { get; set; }
    }
}
