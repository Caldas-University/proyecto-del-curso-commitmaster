namespace EventLogistics.Application.DTOs
{
    public class CriticalResourceAlertDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public int Available { get; set; }
        public int Total { get; set; }
        public string Message { get; set; }
    }
}