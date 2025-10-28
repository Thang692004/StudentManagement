using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using StudentManagement.UI.Pages;
using System.Windows;
using System.Windows.Controls;


namespace StudentManagement.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Biến private để lưu vai trò của người dùng hiện tại
        private readonly string _currentUserRole;

        public MainWindow(string userRole)
        {
            InitializeComponent();
            _currentUserRole = userRole; // Lưu lại vai trò
            this.Loaded += MainWindow_Loaded; // Gán sự kiện Loaded để xử lý sau khi UI đã tải xong
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ApplyPermissions();
            // Đặt DashboardPage làm trang mặc định khi mở lên
            MainContent.Content = new DashboardPage();
        }

        // HÀM MỚI: Chứa logic phân quyền chính.
        private void ApplyPermissions()
        {
            // Cập nhật TextBlock trên giao diện để hiển thị đúng vai trò
            UserRoleText.Text = _currentUserRole;

            if (_currentUserRole == "Student")
            {
                // Nếu người dùng là "Student", ẩn các nút quản lý
                BtnStudents.Visibility = Visibility.Collapsed;
                BtnClasses.Visibility = Visibility.Collapsed;
                BtnDepart.Visibility = Visibility.Collapsed;
                BtnAccounts.Visibility = Visibility.Collapsed;
            }
            // Nếu là "Admin", không cần làm gì cả vì mặc định các nút đã hiển thị
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DashboardPage();
        }

        private void BtnStudents_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new StudentsPage();
            if (MainContent.Content is ISearchableContent searchableContent)
            {
                // Gọi PerformSearch() với nội dung hiện tại của SearchBox.
                searchableContent.PerformSearch(SearchBox.Text);
            }
        }

        private void BtnClasses_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ClassPage();
        }
        private void BtnDepartments_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DepartmentPage();
        }

        private void BtnAccounts_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AccountsPage();
        }
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProfilePage();
        }

        // Khi ToggleButton được check
        private void ProfileToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (ProfileToggle.ContextMenu != null)
            {
                ProfileToggle.ContextMenu.PlacementTarget = ProfileToggle;
                ProfileToggle.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                ProfileToggle.ContextMenu.IsOpen = true;
            }
        }

        // Khi ContextMenu đóng thì reset lại toggle
        private void ProfileMenu_Closed(object sender, RoutedEventArgs e)
        {
            ProfileToggle.IsChecked = false;
        }

        // Chức năng tìm kiếm
        private void SearchBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.Trim();
            if (MainContent.Content is ISearchableContent searchableContent)
            {
                // GỌI: Thực hiện tìm kiếm trên User Control đó
                searchableContent.PerformSearch(query);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}