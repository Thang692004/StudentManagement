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
    public partial class UserStudentsPage : UserControl, StudentManagement.UI.ISearchableContent
    {
        private readonly ReadStudents _readService;
        private ObservableCollection<Student> _allStudents = new ObservableCollection<Student>();
        private ICollectionView? _studentView;
        private readonly string? _maKhoaFilter;

        public UserStudentsPage(string? maKhoaFilter = null)
        {
            InitializeComponent();

            _maKhoaFilter = maKhoaFilter;
            _readService = new ReadStudents();

            LoadStudents();
        }
        private void LoadStudents()
        {
            try
            {
                List<Student> students = _readService.GetStudents();

                if (!string.IsNullOrWhiteSpace(_maKhoaFilter))
                {
                    students = students.Where(s => string.Equals(s.MaKhoa, _maKhoaFilter, StringComparison.OrdinalIgnoreCase)).ToList();
                }

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
        public void PerformSearch(string searchText)
        {
            // Kiểm tra null
            if (_studentView == null) return;

            string query = searchText?.ToLower().Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(query))
            {
                // Xóa bộ lọc
                _studentView.Filter = null;
            }
            else
            {
                _studentView.Filter = item =>
                {
                    var student = item as Student;
                    if (student == null) return false;

                    return (student.MaSV ?? "").ToLower().Contains(query) ||
                           (student.HoTen ?? "").ToLower().Contains(query) ||
                           (student.DiaChi ?? "").ToLower().Contains(query) ||
                           (student.Email ?? "").ToLower().Contains(query) ||
                           (student.SoDienThoai ?? "").ToLower().Contains(query) ||
                           ((student.TenLop ?? "").ToLower().Contains(query)) ||
                           ((student.TenKhoa ?? "").ToLower().Contains(query));
                };
            }

            _studentView.Refresh();

            if (_studentView.IsEmpty && !string.IsNullOrWhiteSpace(query))
            {
                StudentsDataGrid.Visibility = Visibility.Collapsed;
                NoResultsText.Visibility = Visibility.Visible;
            }
            else
            {
                StudentsDataGrid.Visibility = Visibility.Visible;
                NoResultsText.Visibility = Visibility.Collapsed;
            }
        }
    }
}
