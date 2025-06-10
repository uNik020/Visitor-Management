using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VisitorManagement.Models;

public partial class Hosts
{
    public int HostId { get; set; }

    // Personal Info
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }

    // Additional Info
    public int? DesignationId { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? About { get; set; }

    // Department
    public int? DepartmentId { get; set; }
    public virtual Department? Department { get; set; }
    public virtual Designation? Designation { get; set; }

    [JsonIgnore]
    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
