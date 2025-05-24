using System;
using System.Collections.Generic;

namespace VisitorManagement.Models;

public class Admin
{
    public int AdminId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }    // Newly added
    public string Email { get; set; }          // Newly added
    public DateTime CreatedAt { get; set; }
}

