using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventLogistics.Domain.Entities
{
    // Base Entity for all entities
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            // Establecer valores predeterminados
            CreatedBy = "System";
            CreatedAt = DateTime.UtcNow;
            UpdatedBy = "System";
        }
        
        [Key]
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}