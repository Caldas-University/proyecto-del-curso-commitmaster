
namespace EventLogistics.EventLogistics.Domain.Entities

{

    public class Activity
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    }
}