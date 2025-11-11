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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PRN212_Project
{
    /// <summary>
    /// Interaction logic for CourseDetailWindow.xaml
    /// </summary>
    public partial class CourseDetailWindow : Window
    {
        private CourseService _courseService = new CourseService();
        private ReviewService _reviewService = new ReviewService();
        private StudentHome _studentHome;
        private BrowseCourseWindow _browseCourseWindow;
        private int studentId;
        private int courseId;
        private bool canReview;
        private User user;
        private string from;
        public CourseDetailWindow(User u, int courseId, bool canReview, string from)
        {
            InitializeComponent();
            this.studentId = u.UserId;
            this.courseId = courseId;
            this.canReview = canReview;
            this.from = from;
            this.user = u;
            setUpMyReview();
            LoadData();
        }

        public void setUpMyReview()
        {
            bool allowed = canReview || _courseService.ExistEnroll(studentId, courseId);
            tbMyReviewTitle.Visibility = allowed ? Visibility.Visible : Visibility.Collapsed;
            panelMyReview.Visibility = allowed ? Visibility.Visible : Visibility.Collapsed;
            if (!allowed) return;

            var mine = _reviewService.GetReviewByCourseAndStudent(courseId, studentId);
            if (mine == null)
            {
                btnAddReview.Visibility = Visibility.Visible;
                btnUpdateReview.Visibility = Visibility.Collapsed;
                cbMyRating.SelectedIndex = 4; // 5 sao
                tbMyComment.Text = "";
            }
            else
            {
                btnAddReview.Visibility = Visibility.Collapsed;
                btnUpdateReview.Visibility = Visibility.Visible;
                cbMyRating.SelectedIndex = (mine.Rating ?? 5) - 1;
                tbMyComment.Text = mine.Comment ?? "";
            }
        }
        public void LoadData()
        {
            LoadCourse();
            LoadReview();
        }
        public void LoadCourse()
        {
            var course = _courseService.GetCourseById(courseId);

            tbTitle.Text = course.Title ?? "";
            tbCategory.Text = course.Category?.Name ?? "";
            tbLanguage.Text = course.Language ?? "";
            tbLevel.Text = course.Level ?? "";
            tbDescription.Text = course.Description ?? "";
        }

        public void LoadReview()
        {
            var list = _reviewService.GetAllReviewByCourse(courseId);
            var display = list.Select(l => new
            {
                UserName = l.Student.FullName != null ? l.Student.FullName : l.Student.Username,
                Rating = l.Rating,
                Comment = l.Comment,
                CreatedAtDisplay = l.CreatedAt
            });

            dgReviews.ItemsSource = display;
        }

        private void btnSearchFilter_Click(object sender, RoutedEventArgs e)
        {
            int? star = null;
            string keyword = tbSearchComment.Text.Trim();
            var tag = (cbStarFilter.SelectedItem as ComboBoxItem)?.Tag.ToString();
            if (int.TryParse(tag, out int s))
            {
                star = s;
            }
            var list = _reviewService.FilterReviews(courseId, keyword, star);

            var display = list.Select(l => new
            {
                UserName = l.Student.FullName != null ? l.Student.FullName : l.Student.Username,
                Rating = l.Rating,
                Comment = l.Comment,
                CreatedAtDisplay = l.CreatedAt
            });

            dgReviews.ItemsSource = display;
        }

        private void btnAddReview_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem? selectedItem = cbMyRating.SelectedItem as ComboBoxItem;
            string? ratingText = selectedItem.Content == null ? "" : selectedItem.Content.ToString();

            int rating;
            bool ok = int.TryParse(ratingText, out rating);
            if (!ok)
            {
                MessageBox.Show("Giá trị số sao không hợp lệ");
                return;
            }

            var comment = string.IsNullOrWhiteSpace(tbMyComment.Text) ? null : tbMyComment.Text.Trim();

            var rs = _reviewService.AddReview(courseId, studentId, rating, comment);
            if (!rs.OK)
            {
                MessageBox.Show(rs.Error ?? "Không thể cập nhật đánh giá", "Lỗi");
                return;
            }

            LoadReview();
            setUpMyReview();
            MessageBox.Show("Đã cập nhật đánh giá.", "Thông báo");
        }

        private void btnUpdateReview_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem? selectedItem = cbMyRating.SelectedItem as ComboBoxItem;
            string? ratingText = selectedItem.Content == null ? "" : selectedItem.Content.ToString();

            int rating;
            bool ok = int.TryParse(ratingText, out rating);
            if (!ok)
            {
                MessageBox.Show("Giá trị số sao không hợp lệ");
                return;
            }

            var comment = string.IsNullOrWhiteSpace(tbMyComment.Text) ? null : tbMyComment.Text.Trim();

            var rs = _reviewService.UpdateReview(courseId, studentId, rating, comment);
            if (!rs.OK)
            {
                MessageBox.Show(rs.Error ?? "Không thể cập nhật đánh giá", "Lỗi");
                return;
            }

            LoadReview();
            setUpMyReview();
            MessageBox.Show("Đã cập nhật đánh giá.", "Thông báo");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            _studentHome = new StudentHome(user);
            _browseCourseWindow = new BrowseCourseWindow(user);
            if(from == "home")
            {
                _studentHome.Show();
                this.Close();
            }else if(from == "browse")
            {
                _browseCourseWindow.Show();
                this.Close();
            }
        }
    }
}
