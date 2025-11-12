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
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private readonly UserService _userService;
        private readonly int _userId;

        
        public EditUserWindow(int userId)
        {
            InitializeComponent();
            _userService = new UserService();
            _userId = userId;
            LoadUserData();
        }
        private void LoadUserData()
        {
            using (var context = new PrnProjectContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == _userId);
                if (user == null)
                {
                    MessageBox.Show("Không tìm thấy người dùng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                    return;
                }

                // 🔹 Gán thông tin vào các textbox (chỉ đọc)
                txtUsername.Text = user.Username;
                txtFullName.Text = user.FullName;
                txtEmail.Text = user.Email;
                txtPhone.Text = user.Phone;

                dpBirthDate.SelectedDate = user.DateOfBirth?.ToDateTime(TimeOnly.MinValue);


                // 🔹 Hiển thị danh sách vai trò (do DB chỉ có cột 'role')
                cbRole.SelectedItem = null; // reset chọn trước
                foreach (ComboBoxItem item in cbRole.Items)
                {
                    if (item.Content.ToString().Equals(user.Role, StringComparison.OrdinalIgnoreCase))
                    {
                        cbRole.SelectedItem = item;
                        break;
                    }
                }
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = cbRole.SelectedItem as ComboBoxItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string newRole = selectedItem.Content.ToString();
            var (ok, error) = _userService.UpdateUserRole(_userId, newRole);

            if (ok)
            {
                MessageBox.Show("Cập nhật vai trò thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                AdminPage adminPage = new AdminPage();
                adminPage.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(error ?? "Có lỗi xảy ra khi cập nhật.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            AdminPage adminPage = new AdminPage();
            adminPage.Show();
            this.Close();
        }
    }
}
