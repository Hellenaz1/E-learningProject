using PRN212_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN212_Project.Models;

namespace PRN212_Project.Services
{
    public class UserService
    {
        private UserRepository _userRepo = new UserRepository();
        public (bool OK, string ? Error) Register(string username, string email, string password, string gender)
        {
            if (string.IsNullOrWhiteSpace(username))
                return (false, "Không được để trống tên đăng nhập");

            if (string.IsNullOrWhiteSpace(email))
                return (false, "Không được để trống email");

            if (string.IsNullOrEmpty(password))
                return (false, "Không được để trống mật khẩu");

            if (!IsStrong(password))
                return (false, "Mật khẩu quá yếu. Tối thiểu 6 ký tự, có chữ, số và ký tự đặc biệt");

            if (gender != "Nam" && gender != "Nữ")
                return (false, "Vui lòng chọn giới tính");

            if (_userRepo.ExistsByEmail(email))
                return (false, "Email này đã được sử dụng!");

            if (_userRepo.ExistsByUsername(username))
                return (false, "Tên đăng nhập này đã được sử dụng!");

            var user = new User
            {
                Username = username,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12),
                Gender = gender
            };

            _userRepo.AddUser(user);
            return (true, null);
        }
        private static bool IsStrong(string pass) =>
           pass.Length >= 6 &&
           pass.Any(char.IsLetter) &&
           pass.Any(char.IsDigit) &&
           pass.Any(ch => !char.IsLetterOrDigit(ch));


        public (bool OK, string? Error, User? User ) Login(string loginName, string password)
        {
            if (string.IsNullOrWhiteSpace(loginName))
                return (false, "Vui lòng nhập tên đăng nhập hoặc email", null);
            if (string.IsNullOrEmpty(password))
                return (false, "Vui lòng nhập mật khẩu", null);

            var user = _userRepo.findUserByUserNameOrEmail(loginName);
            if(user == null)
            {
                return (false, "Tài khoản không tồn tại", null);
            }

            var ok = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!ok)
            {
                return (false, "Mật khẩu không chính xác", null);
            }

            return (true, null, user);
        }
    }
}
