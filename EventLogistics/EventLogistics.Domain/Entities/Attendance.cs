using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventLogistics.Domain.Entities;

public class Attendance : BaseEntity
{
    [Required]
    public int ParticipantId { get; set; }

    [ForeignKey("ParticipantId")]
    public virtual Participant Participant { get; set; }

    [Required]
    public int EventId { get; set; }

    [ForeignKey("EventId")]
    public virtual Event Event { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    public DateTime CheckInTime { get; set; }

    [Required]
    [MaxLength(20)]
    public string? Method { get; set; }

    public string? RegisteredBy { get; set; }
}
