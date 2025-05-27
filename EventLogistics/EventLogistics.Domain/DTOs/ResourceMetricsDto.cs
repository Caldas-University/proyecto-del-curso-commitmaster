namespace EventLogistics.Domain.DTOs
{
    public class ResourceMetricsDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int TotalCount { get; set; }
        public int UsedCount { get; set; }
        public int AvailableCount { get; set; }
        public int EventsCount { get; set; }
        public int ActivitiesCount { get; set; }
        public int TotalUsage { get; set; }
        public bool Availability { get; set; }
    }
}