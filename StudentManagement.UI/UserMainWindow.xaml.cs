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

namespace StudentManagement.UI;

using System.Windows.Controls.Primitives;
using StudentManagement.UI.Pages;
using StudentManagement.UI.Pages.UserPages;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class UserMainWindow : Window
{
    private string? _currentUsername;
    private string? _currentMaSV;
    private string? _currentMaKhoa;

    public UserMainWindow()
    {
        InitializeComponent();
    }

    public UserMainWindow(string username, string? maSV) : this()
    {
        _currentUsername = username;
        _currentMaSV = maSV;
        // Delay heavy lookup until loaded
        this.Loaded += UserMainWindow_Loaded;
    }

    private void UserMainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(_currentMaSV))
            {
                var reader = new StudentManagement.ReadStudents();
                var student = reader.GetStudentByMaSV(_currentMaSV);
                _currentMaKhoa = student?.MaKhoa;
            }
        }
        catch
        {
            // ignore; pages will handle missing data
        }

        // Default to user students page for student users
        MainContent.Content = new UserStudentsPage(_currentMaKhoa);
    }
    private void BtnDashboard_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new DashboardPage();
    }

    private void BtnUserStudents_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new UserStudentsPage(_currentMaKhoa);
        if (MainContent.Content is ISearchableContent searchableContent)
        {
            // Gọi PerformSearch() với nội dung hiện tại của SearchBox.
            searchableContent.PerformSearch(SearchBox.Text);
        }
    }

    private void BtnUserClasses_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new UserClassesPage(_currentMaKhoa);
    }
    private void BtnUserDepartments_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new UserDepartmentsPage(_currentMaKhoa);
    }

    private void Profile_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ProfilePage(_currentMaSV);
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
    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        // Mở lại cửa sổ Login
        LoginWindow loginWindow = new LoginWindow();
        loginWindow.Show();

        // Đóng MainWindow hiện tại
        ProfileToggle.IsChecked = false;
        this.Close();
    }

}