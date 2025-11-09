using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN212_Project.Repositories;
using PRN212_Project.Models;
namespace PRN212_Project.Services
{
    public class StudentService
    {
        private StudentRepository _studentRepository = new StudentRepository();

        public User? GetAllStudentInformation(int userId)
        {
            return _studentRepository.getAllStudentInformation(userId);
        }

        public (bool OK, string? Error) UpdateStudentProfile(int userId, string? fullName, string? phone,
            DateOnly? dob, string gender, string? institution, string? gradeLevel,
            string? address)
        {
            if (fullName != null)
            {
                if (fullName.Length < 10) return (false, "Họ và tên quá ngắn");
                if (fullName.Length > 255) return (false, "Họ và tên quá dài");
            }

            if (phone != null && phone.Any(c => !char.IsDigit(c)))
            {
                return (false, "Số điện thoại không hợp lệ");
            }

            if (dob!= null && dob.Value > DateOnly.FromDateTime(DateTime.Today))
                return (false, "Ngày sinh không hợp lệ");

            if (gender != "Nam" && gender != "Nữ")
                return (false, "Giới tính phải là 'Nam' hoặc 'Nữ'");

            if (institution != null)
            {
                if (institution.Length < 10) return (false, "Tên trường quá ngắn");
                if (institution.Length > 255) return (false, "Tên trường quá dài");
            }

            if (gradeLevel != null && gradeLevel.Trim().Length > 50)
                return (false, "Lớp/Trình độ quá dài");

            if (address != null)
            {
                if (address.Length < 10) return (false, "Địa chỉ quá ngắn");
                if (address.Length > 255) return (false, "Địa chỉ quá dài");
            }

            _studentRepository.UpdateStudentProfile(
                userId, fullName, phone, dob, gender, institution, gradeLevel, address);

            return (true, null);
        }

    }
}
