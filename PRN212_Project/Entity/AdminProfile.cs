using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class AdminProfile
{
    public int UserId { get; set; }

    public string? Position { get; set; }

    public string? Department { get; set; }

    public virtual User User { get; set; } = null!;
}
