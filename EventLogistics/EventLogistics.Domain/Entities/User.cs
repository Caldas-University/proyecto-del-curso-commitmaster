using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventLogistics.Domain.Entities
{
    // User entity as shown in the diagram
    public class User : BaseEntity
    {
        public User()
        {
            // Inicializar colecciones para evitar problemas de null reference
            Notifications = new List<Notification>();
        }

        public string Role { get; set; }
        public string Contact { get; set; }
        public string Preferences { get; set; } // JSON for preferences
        
        // Navigation properties
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}