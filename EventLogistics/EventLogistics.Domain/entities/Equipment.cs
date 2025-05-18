
namespace EventLogistics.EventLogistics.Domain.Entities

{

    public class Equipment
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ubicacion { get; set; }
        public bool Estado { get; set; } // true = disponible, false = no disponible

        // Relaciones (opcionalmente nulas, porque puede estar asociado solo a una)
        public int? ActivityId { get; set; }
        public Activity? Activity { get; set; }

        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    }

}