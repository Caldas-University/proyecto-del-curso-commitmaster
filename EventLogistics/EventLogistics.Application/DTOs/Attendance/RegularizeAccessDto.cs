namespace EventLogistics.Application.DTOs.Attendance
{
    public class RegularizeAccessDto
    {
        public int ParticipantId { get; set; }
        public Dictionary<string, string> DataToUpdate { get; set; } = new Dictionary<string, string>(); // Puedes ajustar el tipo según tus necesidades
    }
}