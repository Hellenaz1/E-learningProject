using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Project.Repositories
{
    public class UserRepository
    {
        private PrnProjectContext _context;
        public UserRepository()
        {
            _context = new PrnProjectContext();
        }

        public bool ExistsByEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
        public bool ExistsByUsername(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User? findUserByUserNameOrEmail(string loginName)
        {
            return _context.Users.Where(u => u.Username == loginName || u.Email == loginName).FirstOrDefault();
        }

        public User? FindById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }
        public void UpdatePassword(int userId, string newHashPassword)
        {
            var user = FindById(userId);
            user.Password = newHashPassword;
            _context.SaveChanges();
        }
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
        public bool DeleteUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
        public bool UpdateUserRole(int userId, string newRole)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            user.Role = newRole;  
            _context.SaveChanges();
            return true;
        }
        public List<User> SearchUsers(string keyword)
        {
            using (var context = new PrnProjectContext())
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return context.Users.ToList(); 

                keyword = keyword.ToLower().Trim();
                return context.Users.Where(u =>
                        u.Username.ToLower().Contains(keyword) ||
                        u.FullName.ToLower().Contains(keyword) ||
                        u.Email.ToLower().Contains(keyword))
                    .ToList();
            }
        }
    }
}
