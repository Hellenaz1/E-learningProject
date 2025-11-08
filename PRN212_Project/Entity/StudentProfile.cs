using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class StudentProfile
{
    public int UserId { get; set; }

    public string? Institution { get; set; }

    public string? GradeLevel { get; set; }

    public string? Address { get; set; }

    public virtual User User { get; set; } = null!;
}
