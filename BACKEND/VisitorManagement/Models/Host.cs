using System;
using System.Collections.Generic;

namespace VisitorManagement.Models;

public partial class Host
{
    public int HostId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
