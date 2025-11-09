using PRN212_Project.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PRN212_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StudentHome studentHome;
        private UserService userService = new UserService();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Register(object sender, RoutedEventArgs e)
        {
            var signUp = new SignUpWindow();
            signUp.Show();
            this.Close();
        }

        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            var result = userService.Login(tbLoginName.Text, pwPassword.Password);
            if (!result.OK)
            {
                MessageBox.Show(result.Error, "Lỗi đăng nhập");
                return;
            }
            if(result.User.Role == "student")
            {
                studentHome = new StudentHome(result.User);
                studentHome.Show();
                this.Close();
            }


        }
    }
}