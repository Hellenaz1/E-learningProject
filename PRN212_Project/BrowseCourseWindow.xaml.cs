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
    /// Interaction logic for BrowseCourseWindow.xaml
    /// </summary>
    public partial class BrowseCourseWindow : Window
    {
        private User _currentStudent;
        private StudentHome _studentHome;
        private CourseDetailWindow _courseDetailWindow;
        private CourseService _courseService = new CourseService();

        public BrowseCourseWindow(User student)
        {
            InitializeComponent();
            _currentStudent = student;
            LoadData();
            ReloadGrid();
        }
        private void LoadData()
        {
            LoadCategory();
            LoadLevel();
            LoadLanguage();
        }
        private void LoadCategory()
        {
            var list = _courseService.GetAllCategories();
            //tu day cac phan tu khac sang +1 index
            list.Insert(0, new Category { CategoryId = 0, Name = "Tất cả" });

            cbCategory.ItemsSource = list;
            cbCategory.DisplayMemberPath = "Name";
            cbCategory.SelectedValuePath = "CategoryId";
            cbCategory.SelectedValue = 0;
        }

        private void LoadLevel()
        {
            var list = _courseService.GetAllLevel();
            list.Insert(0, "Tất cả");

            cbLevel.ItemsSource = list;
            cbLevel.SelectedIndex = 0;
        }
        private void LoadLanguage()
        {
            var list = _courseService.GetAllLanguages();
            list.Insert(0, "Tất cả");

            cbLanguage.ItemsSource = list;
            cbLanguage.SelectedIndex = 0;
        }

        private (int? categoryId, string? level, string? language, string? keyword, string? sortTag) ReadFilters()
        {
            int? categoryId = null;
            if (cbCategory.SelectedValue is int v && v > 0)
                categoryId = v;

            string? level = (cbLevel.SelectedItem as string) == "Tất cả" ? null : (cbLevel.SelectedItem as string);

            string? language = (cbLanguage.SelectedItem as string) == "Tất cả" ? null : (cbLanguage.SelectedItem as string);

            string? keyword = (!string.IsNullOrWhiteSpace(tbKeyword.Text)) ? tbKeyword.Text.Trim() : null;

            string? sortTag = (cbSort.SelectedItem as ComboBoxItem)?.Tag?.ToString();

            if (string.IsNullOrWhiteSpace(sortTag))
            {
                sortTag = null;
            }

            return (categoryId, level, language, keyword, sortTag);
        }

        private void ReloadGrid()
        {
            var result = _courseService.BrowseCourse(ReadFilters().keyword, ReadFilters().categoryId,
                         ReadFilters().level, ReadFilters().language, ReadFilters().sortTag, _currentStudent.UserId);

            //khoi tao anonymous class gan vao data grid
            var display = result.Select(c => new
            {
                c.Course.CourseId,
                Title = c.Course.Title,
                Category = c.Course.Category?.Name,
                Level = c.Course.Level,
                Language = c.Course.Language,
                CreatedAtDisplay = c.Course.CreatedAt,
                EnrollVisibility = c.IsEnrolled ? Visibility.Collapsed : Visibility.Visible,
                EnrolledVisibility = c.IsEnrolled ? Visibility.Visible : Visibility.Collapsed
            }).ToList();

            dgCourse.ItemsSource = display;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ReloadGrid();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            _studentHome = new StudentHome(_currentStudent);
            _studentHome.Show();
            this.Close();
        }

        private void ButtonDetail_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Tag is int courseId)
            {
                bool canReview = _courseService.ExistEnroll(_currentStudent.UserId, courseId);
                _courseDetailWindow = new CourseDetailWindow(_currentStudent, courseId, canReview, "browse");
                _courseDetailWindow.Show();
                this.Close();
            }
        }

        private void ButtonEnroll_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int courseId)
            {
                var result = _courseService.EnrollCourse(_currentStudent.UserId, courseId);
                if (!result.OK)
                {
                    MessageBox.Show(result.Error, "Lỗi");
                    return;
                }

                MessageBox.Show("Ghi danh thành công!", "Thông báo");                       

                ReloadGrid();
            }
        }
    }
}
