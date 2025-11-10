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
using System.Windows.Shapes;
using PRN212_Project.Models;
using PRN212_Project.Services;
namespace PRN212_Project
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private User _currentStudent;
        private UserService userService;
        private StudentHome studentHome;
        private MainWindow loginWindow;
        public ChangePasswordWindow(User student)
        {
            InitializeComponent();
            _currentStudent = student;
            userService = new UserService();
        }

        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = pbPasswordOld.Password;
            string newPassword = pbNewPassword.Password;
            string confirmPassword = pbConfirmPassword.Password;

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu nhập lại không hợp lệ", "Lỗi đổi mật khẩu");
                return;
            }
            var result = userService.ChangePassword(_currentStudent, oldPassword, newPassword);
            if (!result.OK)
            {
                MessageBox.Show(result.Error, "Lỗi đổi mật khẩu");
                return;
            }

            if (result.OK)
            {
                MessageBox.Show("Đổi mật khẩu thành công, vui lòng đăng nhập lại", "Thông báo");
                loginWindow = new MainWindow();
                loginWindow.Show();
                this.Close();
            }

        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            studentHome = new StudentHome(_currentStudent);
            studentHome.Show();
            this.Close();
        }
    }
}
