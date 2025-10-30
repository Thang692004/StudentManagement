using System;
using System.Windows;
using StudentManagement.Core;

namespace StudentManagement.UI
{
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
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            try
            {
                string? userRole = _authService.AuthenticateUser(username, password);

                // Không có tài khoản hợp lệ
                if (string.IsNullOrEmpty(userRole))
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!",
                        "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // ✅ CHỈ CHO PHÉP ĐĂNG NHẬP KHI ROLE = ADMIN
                if (!userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Tài khoản này không có quyền truy cập hệ thống!",
                        "Từ chối truy cập", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }

                // Nếu là Admin → mở giao diện chính
                MainWindow mainWindow = new MainWindow(userRole);
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.\n\nChi tiết: " + ex.Message,
                                "Lỗi kết nối", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
