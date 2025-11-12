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
    /// Interaction logic for CourseManagementWindow.xaml
    /// </summary>
    public partial class CourseManagementWindow : Window
    {
        private readonly CourseService _service;
        public CourseManagementWindow()
        {
            InitializeComponent();
            _service = new CourseService();
            LoadCourses();
        }
        private void LoadCourses()
        {
            List<Course> courses = _service.GetAllCourses();
            dgCourses.ItemsSource = courses;
        }
        private void AddCourse_Click(object sender, RoutedEventArgs e)
        {
            AddCourseWindow addWindow = new AddCourseWindow();
            addWindow.Show();
            this.Close();
        }
        private void EditCourse_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            int courseId = (int)button.Tag;
            var editWindow = new AddCourseWindow(courseId);
            editWindow.Show();
            this.Close();

           
        }
        private void DeleteCourse_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            int courseId = (int)button.Tag;

            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa khóa học này không?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                var (ok, error) = _service.DeleteCourse(courseId);
                if (ok)
                {
                    MessageBox.Show("Xóa khóa học thành công!", "Thông báo");
                    LoadCourses(); // load lại danh sách sau khi xóa
                }
                else
                {
                    MessageBox.Show(error ?? "Không thể xóa khóa học!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            
            var userWindow = new AdminPage();
            userWindow.Show();
            this.Close();
        }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                // Nếu ô tìm trống -> hiển thị lại tất cả
                LoadCourses();
                return;
            }

            var result = _service.GetAllCourses()
                .Where(c => c.Title.ToLower().Contains(keyword))
                .ToList();

            dgCourses.ItemsSource = result;

            if (result.Count == 0)
            {
                MessageBox.Show("Không tìm thấy khóa học nào phù hợp!", "Kết quả tìm kiếm", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


    }
}
