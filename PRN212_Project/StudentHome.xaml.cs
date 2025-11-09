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
    /// Interaction logic for StudentHome.xaml
    /// </summary>
    public partial class StudentHome : Window
    {
        private User _currentStudent;
        private CourseService _courseService;
        public StudentHome(User student)
        {
            InitializeComponent();
            _currentStudent = student;
            _courseService = new CourseService();
            LoadData();
            ReloadGrid();

        }

        public void LoadData()
        {
            string welcomeName = (_currentStudent.FullName != null ? _currentStudent.FullName : _currentStudent.Username);
            tbWelcome.Text = $"Xin chào {welcomeName}";
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

            string? keyword = (!string.IsNullOrWhiteSpace(tbSearchKey.Text)) ? tbSearchKey.Text.Trim() : null;

            string? sortTag = (cbSort.SelectedItem as ComboBoxItem)?.Tag?.ToString();

            if (string.IsNullOrWhiteSpace(sortTag))
            {
                sortTag = null;
            }

            return (categoryId, level, language, keyword, sortTag);
        }


        private void ReloadGrid()
        {
            var result = _courseService.GetEnrolledCourses(_currentStudent.UserId, ReadFilters().keyword, ReadFilters().categoryId,
                         ReadFilters().level, ReadFilters().language, ReadFilters().sortTag);

            //khoi tao anonymous class gan vao data grid
            var display = result.Select(e => new
            {
                e.Course.CourseId,
                Title = e.Course.Title,
                Category = e.Course.Category?.Name,
                Level = e.Course.Level,
                Language = e.Course.Language,
                EnrollAtDisplay = e.EnrolledAt.ToString(),
            }).ToList();

            dgEnrolled.ItemsSource = display;
        }

        private void BtnBrowseCourses_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnUpdateProfile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            ReloadGrid();
        }

        private void BtnDetail_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
