using System;
using System.ComponentModel.DataAnnotations.Schema;
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

    public bool IsApproved { get; set; } = false; 
    public string? ApprovalComment { get; set; }

    public string? GatePassNumber { get; set; }        // Optional pass number

    [Column(TypeName = "nvarchar(max)")]
    public string? QrCodeData { get; set; }            // Unique to this visit if needed

    // Navigation
    [JsonIgnore]
    public virtual Hosts Host { get; set; } = null!;

    [JsonIgnore]
    public virtual Visitor Visitor { get; set; } = null!;
}
