using System;
using System.Text.Json.Serialization;

namespace VisitorManagement.Models;

public partial class Visit
{
    public int VisitId { get; set; }

    public int VisitorId { get; set; }
    public int HostId { get; set; }

    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }

    public string? VisitStatus { get; set; }           // e.g., Scheduled, CheckedIn, Completed, Cancelled

    public bool IsApproved { get; set; } = false;      // Optional for pre-approvals
    public string? ApprovalComment { get; set; }

    public string? GatePassNumber { get; set; }        // Optional pass number
    public string? QrCodeData { get; set; }            // Unique to this visit if needed

    // Navigation
    public virtual Hosts Host { get; set; } = null!;

    [JsonIgnore]
    public virtual Visitor Visitor { get; set; } = null!;
}
