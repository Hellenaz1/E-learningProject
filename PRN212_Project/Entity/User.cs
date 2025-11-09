using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateOnly CreatedAt { get; set; }

    public virtual AdminProfile? AdminProfile { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual StudentProfile? StudentProfile { get; set; }
}
