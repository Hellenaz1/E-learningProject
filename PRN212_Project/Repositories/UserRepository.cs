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

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}
