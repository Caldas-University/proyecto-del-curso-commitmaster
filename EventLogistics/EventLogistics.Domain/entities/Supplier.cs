
namespace EventLogistics.EventLogistics.Domain.Entities

{
    public class Supplier
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Contacto { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        // Relaciones (opcionalmente nulas, porque puede estar asociado solo a una)
        public int? ActivityId { get; set; }
        public Activity? Activity { get; set; }

        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    }

}