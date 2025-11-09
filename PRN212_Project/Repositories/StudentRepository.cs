using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
namespace PRN212_Project.Repositories
{
    internal class StudentRepository
    {
        private PrnProjectContext _context = new PrnProjectContext();
        public User? getAllStudentInformation(int studentId)
        => _context.Users.Include(u => u.StudentProfile)
            .Where(u => u.UserId == studentId).FirstOrDefault();

        public void UpdateStudentProfile(int userId, string? fullName, string? phone,
            DateOnly? dob, string gender, string? institution, string? gradeLevel,
            string? address)
        {
            var user = _context.Users.
                Include(u => u.StudentProfile)
               .FirstOrDefault(u => u.UserId == userId);

            user.FullName = string.IsNullOrWhiteSpace(fullName) ? null : fullName.Trim();
            user.Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
            user.DateOfBirth = dob;
            user.Gender = gender;

            //student
            user.StudentProfile.Institution = string.IsNullOrWhiteSpace(institution) ? null : institution.Trim();
            user.StudentProfile.GradeLevel = string.IsNullOrWhiteSpace(gradeLevel) ? null : gradeLevel.Trim();
            user.StudentProfile.Address = string.IsNullOrWhiteSpace(address) ? null : address.Trim();
           
            _context.SaveChanges();
        }
    }
}
