using PRN212_Project.Models;
using PRN212_Project.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PRN212_Project
{
    /// <summary>
    /// Interaction logic for UserManagementView.xaml
    /// </summary>
    public partial class UserManagementView : UserControl
    {
        private readonly UserService _userService;
        public UserManagementView()
        {
            InitializeComponent();
            _userService = new UserService();
            LoadUsers();
        }
        private void LoadUsers()
        {
            List<User> users = _userService.GetAllUsers();
            dgUsers.ItemsSource = users;
        }
    }
}
