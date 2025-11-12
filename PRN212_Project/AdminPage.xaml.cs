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
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Window
    {
        private readonly UserService _userService;

        public AdminPage()
        {
            InitializeComponent();
            _userService = new UserService();
            LoadUsers();
        }

        private void LoadUsers()
        {
            dgUsers.ItemsSource = null;
            var users = _userService.GetAllUsers();
            dgUsers.ItemsSource = users;
        }



        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
           
            if (sender is not Button button || button.Tag is not int userId)
                return;

            
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa tài khoản này không?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                
                var (ok, error) = _userService.DeleteUser(userId);

                if (ok)
                {
                    MessageBox.Show("Xóa người dùng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show(error ?? "Đã xảy ra lỗi khi xóa người dùng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            
            var button = sender as Button;
            if (button == null)
                return;

           
            int userId = (int)button.Tag;

            
            var editWindow = new EditUserWindow(userId);
            this.Close();
            editWindow.ShowDialog();
            
        }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            var users = _userService.SearchUsers(keyword);
            dgUsers.ItemsSource = users;
        }
        private void BtnCourses_Click(object sender, RoutedEventArgs e)
        {
          
            var courseWindow = new CourseManagementWindow();
            courseWindow.Show();

            
            this.Close();
        }
    }
}

