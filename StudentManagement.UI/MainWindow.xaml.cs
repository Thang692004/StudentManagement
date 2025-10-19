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

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void BtnDashboard_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new DashboardPage();
    }

    private void BtnStudents_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new StudentsPage();
    }

    private void BtnScores_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ScoresPage();
    }

    private void BtnClasses_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ClassPage();
    }
    private void BtnDepartments_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new DepartmentPage();
    }
    private void BtnSubjects_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new SubjectPage();
    }
    private void BtnAccounts_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new AccountsPage();
    }
    private void Profile_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new ProfilePage();
    }
    private void Search_Click(object sender, RoutedEventArgs e)
    {
        string query = SearchBox.Text.Trim();
        if (!string.IsNullOrEmpty(query))
        {
            MessageBox.Show($"Tìm kiếm: {query}", "Search");
            // TODO: gọi logic filter dữ liệu ở MainContent
        }
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
}