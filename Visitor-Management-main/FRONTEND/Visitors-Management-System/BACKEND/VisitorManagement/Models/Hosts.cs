using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VisitorManagement.Models;

public partial class Hosts
{
    public int HostId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    [JsonIgnore]
    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
