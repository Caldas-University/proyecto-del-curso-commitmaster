using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    // Resource entity as shown in the diagram
    public class Resource : BaseEntity
    {
        [Column("TipoEquipo")]  // Mapear a la BD
        public required string Type { get; set; }

        [Column("Cantidad")]
        public int Capacity { get; set; }
        public bool Availability { get; set; } = true;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

        // Mantener relaciones existentes
        public virtual required ICollection<ResourceAssignment> Assignments { get; set; }
        public virtual required ICollection<ReassignmentRule> ReassignmentRules { get; set; }
    }
}