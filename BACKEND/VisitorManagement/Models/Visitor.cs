using System;
using System.Collections.Generic;

namespace VisitorManagement.Models;

public partial class Visitor
{
    public int VisitorId { get; set; }

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }
    public string? Address { get; set; }

    public string? Purpose { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
