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
using StudentManagement.Core;

namespace StudentManagement.UI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly AuthenticationService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            try // Bắt đầu khối try để bắt lỗi
            {
                string? userRole = _authService.AuthenticateUser(username, password);

                if (userRole != null)
                {
                    MainWindow mainWindow = new MainWindow(userRole);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi Đăng Nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex) // Bắt lỗi kết nối DB được "ném" ra từ tầng Core
            {
                // Hiển thị lỗi kết nối chi tiết cho người dùng
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu. Vui lòng kiểm tra lại.\n\nChi tiết: " + ex.Message, "Lỗi Kết Nối", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
