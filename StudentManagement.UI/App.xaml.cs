using System.Configuration;
using System.Data;
using System.Windows;

namespace StudentManagement.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Tạo và hiển thị cửa sổ đăng nhập khi ứng dụng khởi động
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}

