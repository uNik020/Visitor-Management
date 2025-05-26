using System;
using System.ComponentModel.DataAnnotations.Schema;

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

    // Keep HostId as foreign key
    public int HostId { get; set; }

    // Navigation property (EF Core will auto-link if your DbContext is configured)
    public virtual Hosts? Host { get; set; }

    public string? Status { get; set; }  // e.g., "Inside", "Checked Out"
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
}
