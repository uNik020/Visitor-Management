using System;
using System.Collections.Generic;

namespace VisitorManagement.Models;

public partial class Visit
{
    public int VisitId { get; set; }

    public int VisitorId { get; set; }

    public int HostId { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public string? VisitStatus { get; set; }

    public virtual Host Host { get; set; } = null!;

    public virtual Visitor Visitor { get; set; } = null!;
}
