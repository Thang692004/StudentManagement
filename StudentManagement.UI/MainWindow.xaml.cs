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

    private void BtnTeachers_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new TeachersPage();
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
}