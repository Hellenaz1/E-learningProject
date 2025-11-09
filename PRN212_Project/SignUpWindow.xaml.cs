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
using System.Windows.Shapes;

namespace PRN212_Project
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        MainWindow loginWindow;
        private UserService _userService = new UserService();

        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var gender = rbMale.IsChecked == true ? "Nam"
               : rbFemale.IsChecked == true ? "Nữ" : "";
            if (pwPassword.Password != pwRePassword.Password)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp", "Lỗi khi đăng ký");
                return;
            }
            var result = _userService.Register(tbUserName.Text, tbEmail.Text, pwPassword.Password, gender);

            if (result.OK)
            {
                MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập!", "Thông báo");
                var loginWindow = new MainWindow();
                loginWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Error, "Lỗi khi đăng kí");
                return;
            }

            
        }

    }
}
