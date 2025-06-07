using System;
using System.Collections.Generic;

namespace EventLogistics.Application.DTOs
{
    public class ResourceDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool Availability { get; set; }
        public int Capacity { get; set; }
        public List<Guid> Assignments { get; set; } = new List<Guid>();
    }
}