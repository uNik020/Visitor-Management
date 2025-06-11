using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VisitorManagement.Models;

public partial class Visit
{
    public int VisitId { get; set; }

    public int VisitorId { get; set; }

    public int HostId { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public string? VisitStatus { get; set; }

    public virtual Hosts Host { get; set; } = null!;
    
    [JsonIgnore]
    public virtual Visitor Visitor { get; set; } = null!;
}
