using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using StudentManagement.Core;

namespace StudentManagement.UI.Pages.UserPages
{
    /// <summary>
    /// Interaction logic for UserStudentsPage.xaml
    /// </summary>
    public partial class UserStudentsPage : UserControl
    {
        private readonly ReadStudents _readService;
        private ObservableCollection<Student> _allStudents;
        private ICollectionView _studentView;
        private readonly string _connectionString;
        private readonly DatabaseServiceBase _dbService;
        public UserStudentsPage()
        {
            InitializeComponent();

            // Truyền connection string cho service
            _readService = new ReadStudents(_connectionString);

            LoadStudents();
        }
        private void LoadStudents()
        {
            try
            {
                List<Student> students = _readService.GetStudents();
                _allStudents = new ObservableCollection<Student>(students);
                _studentView = CollectionViewSource.GetDefaultView(_allStudents);
                StudentsDataGrid.ItemsSource = _studentView;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi load students: {ex.Message}");
                MessageBox.Show("Có lỗi xảy ra khi tải danh sách sinh viên.",
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
