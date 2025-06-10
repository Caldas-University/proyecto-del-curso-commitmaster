namespace EventLogistics.Domain.Entities
{
    public class Organizator
    {
        public int OrganizatorId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Role { get; set; }
        public Organizator()
        {
            Activities = new List<Activity>(); // Inicialización
        }
        public required ICollection<Activity> Activities { get; set; }
    }
}