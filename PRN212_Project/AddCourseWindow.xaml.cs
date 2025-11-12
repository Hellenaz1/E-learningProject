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
    /// Interaction logic for AddCourseWindow.xaml
    /// </summary>
    public partial class AddCourseWindow : Window
    {
        private readonly CourseService _courseService;
        private int? _editCourseId = null;
        public AddCourseWindow()
        {
            InitializeComponent();
            _courseService = new CourseService();
            LoadCategories();
            this.Title = "Tạo khóa học mới";
            btnSave.Content = "Tạo khóa học";
        }

        public AddCourseWindow(int courseId) : this()
        {
            _editCourseId = courseId;
            this.Title = "Chỉnh sửa khóa học";
            btnSave.Content = "Lưu thay đổi";
            LoadCourseData(courseId);
        }
        private void LoadCategories()
        {
            var categories = _courseService.GetAllCategories();
            cbCategory.ItemsSource = categories;
            cbCategory.DisplayMemberPath = "Name";
            cbCategory.SelectedValuePath = "CategoryId";
        }
       

        private void LoadCourseData(int courseId)
        {
            using (var context = new PrnProjectContext())
            {
                var course = context.Courses.FirstOrDefault(c => c.CourseId == courseId);
                if (course == null)
                {
                    MessageBox.Show("Không tìm thấy khóa học!", "Lỗi");
                    this.Close();
                    return;
                }

                txtTitle.Text = course.Title;
                txtDescription.Text = course.Description;
                cbCategory.SelectedValue = course.CategoryId;

                SelectComboBoxItem(cbLanguage, course.Language);
                SelectComboBoxItem(cbLevel, course.Level);
            }
        }

        private void SelectComboBoxItem(ComboBox comboBox, string value)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content.ToString().Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void CreateCourse_Click(object sender, RoutedEventArgs e)
        {
            var title = txtTitle.Text.Trim();
            var description = txtDescription.Text.Trim();
            var language = (cbLanguage.SelectedItem as ComboBoxItem)?.Content.ToString();
            var level = (cbLevel.SelectedItem as ComboBoxItem)?.Content.ToString();
            var categoryId = (int?)cbCategory.SelectedValue;

            if (string.IsNullOrEmpty(title) || categoryId == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo");
                return;
            }

            if (_editCourseId.HasValue)
            {
                var course = new Course
                {
                    CourseId = _editCourseId.Value,
                    Title = title,
                    Description = description,
                    Language = language,
                    Level = level,
                    CategoryId = categoryId.Value
                };

                var (ok, error) = _courseService.UpdateCourse(course);
                if (ok)
                {
                    MessageBox.Show("Cập nhật khóa học thành công!", "Thông báo");
                    CourseManagementWindow coursemanage = new CourseManagementWindow();
                    coursemanage.Show();
                    this.Close();
                }
                else
                    MessageBox.Show(error ?? "Lỗi khi cập nhật khóa học!", "Lỗi");
            }
            else
            {
                var course = new Course
                {
                    Title = title,
                    Description = description,
                    Language = language,
                    Level = level,
                    CategoryId = categoryId.Value,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now)
                };

                var (ok, error) = _courseService.AddCourse(course);
                if (ok)
                {
                    MessageBox.Show("Thêm khóa học thành công!", "Thông báo");
                    CourseManagementWindow coursemanage = new CourseManagementWindow();
                    coursemanage.Show();
                    this.Close();
                }
                else
                    MessageBox.Show(error ?? "Lỗi khi thêm khóa học!", "Lỗi");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CourseManagementWindow coursemanage = new CourseManagementWindow();
            coursemanage.Show();
            this.Close();
        }



    }
}
