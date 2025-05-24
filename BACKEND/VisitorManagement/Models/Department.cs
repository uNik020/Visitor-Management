using System;
using System.Collections.Generic;

namespace VisitorManagement.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Host> Hosts { get; set; } = new List<Host>();
}
