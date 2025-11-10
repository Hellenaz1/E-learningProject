using Microsoft.IdentityModel.Tokens;
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
    /// Interaction logic for UpdateStudentProfile.xaml
    /// </summary>
    public partial class UpdateStudentProfile : Window
    {
        private User _currentStudent;
        private StudentHome _studentHome;
        private StudentService _studentService;
        public UpdateStudentProfile(User student)
        {
            InitializeComponent();
            _currentStudent = student;
            _studentService = new StudentService();
            LoadInformation();
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            _studentHome = new StudentHome(_currentStudent);
            _studentHome.Show();
            this.Close();
        }

        private void LoadInformation()
        {
            tbUsername.Text = _currentStudent.Username;
            tbEmail.Text = _currentStudent.Email;
            tbRole.Text = _currentStudent.Role == "student" ? "Học sinh" : "Admin";

            User? u = _studentService.GetAllStudentInformation(_currentStudent.UserId);

            tbFullName.Text = u.FullName ?? "";
            tbPhone.Text = u.Phone ?? "";

            dpDob.SelectedDate = u.DateOfBirth?.ToDateTime(TimeOnly.MinValue);

            if (u.Gender == "Nữ")
                cbGender.SelectedItem = cbFemale;
            else
                cbGender.SelectedItem = cbMale;

            tbInstitution.Text = u.StudentProfile.Institution ?? "";
            tbGradeLevel.Text = u.StudentProfile.GradeLevel ?? "";
            tbAddress.Text = u.StudentProfile.Address ?? "";
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            var gender = (cbGender.SelectedItem as ComboBoxItem)?.Content.ToString();

            DateOnly? dob = dpDob.SelectedDate.HasValue?
                DateOnly.FromDateTime(dpDob.SelectedDate.Value): (DateOnly?)null;

            var result = _studentService.UpdateStudentProfile(
                _currentStudent.UserId, EmptyToNull(tbFullName.Text),
                 EmptyToNull(tbPhone.Text), dob, gender, 
                 EmptyToNull(tbInstitution.Text), EmptyToNull(tbGradeLevel.Text),
                 EmptyToNull(tbAddress.Text)
            );

            if (!result.OK)
            {
                MessageBox.Show(result.Error, "Cập nhật thất bại");
                return;
            }
            MessageBox.Show("Cập nhật hồ sơ thành công.", "Thông báo");

            LoadInformation();
        }

        private string? EmptyToNull(String s) => string.IsNullOrWhiteSpace(s) ? null : s;


    }
}
