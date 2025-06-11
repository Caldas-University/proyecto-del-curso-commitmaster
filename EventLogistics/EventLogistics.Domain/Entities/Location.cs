using System;
using System.Collections.Generic;
using System.Linq;

namespace EventLogistics.Domain.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Status { get; set; } = "Disponible"; // Disponible, Ocupado, Mantenimiento
        
        // Navigation properties
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        
        public Location()
        {
            Name = string.Empty;
            Address = string.Empty;
            Status = "Disponible";
        }

        public Location(string name, string address, string status = "Disponible")
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Status = status ?? throw new ArgumentNullException(nameof(status));
        }
        
        public void UpdateStatus(string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
                throw new ArgumentException("El estado no puede ser nulo o vacío", nameof(newStatus));
                
            var validStatuses = new[] { "Disponible", "Ocupado", "Mantenimiento" };
            if (!validStatuses.Contains(newStatus))
                throw new ArgumentException("Estado no válido. Los estados válidos son: Disponible, Ocupado, Mantenimiento", nameof(newStatus));
                
            Status = newStatus;
        }
        
        public bool IsAvailable()
        {
            return Status == "Disponible";
        }
    }
}
