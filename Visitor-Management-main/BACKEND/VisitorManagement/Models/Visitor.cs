using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitorManagement.Models;

public partial class Visitor
{
    public int VisitorId { get; set; }

    // Basic Info
    public string FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    // Company & Visit Purpose
    public string? CompanyName { get; set; }
    public string? Purpose { get; set; }
    public string? Comment { get; set; }

    // Identification
    public string? IdProofType { get; set; }
    public string? IdProofNumber { get; set; }
    public string? LicensePlateNumber { get; set; }

    // QR & Access
    public string? PassCode { get; set; }             // Auto-generated per visitor
    public string? QrCodeData { get; set; }

    // Media
    [Column(TypeName = "nvarchar(max)")]
    public string? PhotoUrl { get; set; }

    // Registration Info
    public bool? IsPreRegistered { get; set; }

    public DateTime? ExpectedVisitDateTime { get; set; } // ADDED here for pre-registration

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships
    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
    public ICollection<Companion> Companions { get; set; } = new List<Companion>();
}
