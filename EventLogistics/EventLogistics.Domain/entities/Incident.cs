
namespace EventLogistics.EventLogistics.Domain.Entities

{

    public class Incident
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public string Severidad { get; set; }
        public DateTime FechaHora { get; set; }
        public string Ubicacion { get; set; }

        // Relaciones (opcionalmente nulas, porque puede estar asociado solo a una)
        public int? ActivityId { get; set; }
        public Activity? Activity { get; set; }

        public int? SpaceId { get; set; }
        public Space? Space { get; set; }

        public int? EquipmentId { get; set; }
        public Equipment? Equipment { get; set; }

        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
    }

}