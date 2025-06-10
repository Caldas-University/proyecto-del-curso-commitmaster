<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
=======
namespace EventLogistics.Domain.Entities;
>>>>>>> d2804b8fb33a385d35d1dddc2534720d8e145fe9

public class Event
{
<<<<<<< HEAD
    // Event entity as shown in the diagram
    public class Event : BaseEntity
    
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]

        public DateTime EndTime { get; set; }

        public string Description { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public bool Status { get; set; }
        public virtual ICollection<ResourceAssignment> Resources { get; set; } = new List<ResourceAssignment>();
        // Navigation properties
=======
    public Guid Id { get; private set; }
    public string Place { get; private set; }
    public DateTime Schedule { get; private set; }
    public List<Guid> Resources { get; private set; }
    public string Status { get; private set; }

    private Event()
    {
<<<<<<< HEAD
        Place = string.Empty;
        Resources = new List<Guid>();
        Status = string.Empty;
=======
        public required string Name { get; set; }
        public string Place { get; set; }
        public DateTime Schedule { get; set; }
        public string Status { get; set; }
        
        // Navigation properties
        public virtual ICollection<ResourceAssignment> Resources { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
>>>>>>> sebas
>>>>>>> d2804b8fb33a385d35d1dddc2534720d8e145fe9
    }

    public Event(string place, DateTime schedule, string status = "Activo")
    {
        Id = Guid.NewGuid();
        Place = place ?? throw new ArgumentNullException(nameof(place));
        Schedule = schedule;
        Resources = new List<Guid>();
        Status = status ?? throw new ArgumentNullException(nameof(status));
    }
}
