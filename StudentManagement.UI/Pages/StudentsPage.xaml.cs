using StudentManagement.Core;
using StudentManagement.UI.Functions.StudentsFunc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Extensions.Configuration;

namespace StudentManagement.UI.Pages
{
    public partial class StudentsPage : UserControl, ISearchableContent
    {
        private readonly ReadStudents _readService;
        private ObservableCollection<Student> _allStudents;
        private ICollectionView _studentView;
        private readonly string _connectionString;
        private readonly DatabaseServiceBase _dbService;

        public StudentsPage()
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

        public void PerformSearch(string searchText)
        {
            if (_studentView == null) return;

            string query = searchText.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                _studentView.Filter = null;
            }
            else
            {
                _studentView.Filter = item =>
                {
                    var student = item as Student;
                    if (student == null) return false;

                    return student.MaSV.ToLower().Contains(query) ||
                           student.HoTen.ToLower().Contains(query) ||
                           student.DiaChi.ToLower().Contains(query) ||
                           student.Email.ToLower().Contains(query) ||
                           student.SoDienThoai.ToLower().Contains(query) ||
                           (student.TenLop ?? "").ToLower().Contains(query) ||
                           (student.TenKhoa ?? "").ToLower().Contains(query);
                };
            }

            _studentView.Refresh();

            // Hiển thị "Không có kết quả" nếu cần
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

        private void BtnAdding_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainContent.Content = new StudentsAddingWindow();
            }
        }

        private void Repair_button(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để sửa.",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (StudentsDataGrid.SelectedItems.Count > 1)
            {
                MessageBox.Show("Vui lòng chỉ chọn một sinh viên để sửa.",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var selectedStudent = StudentsDataGrid.SelectedItem as Student;
            if (Application.Current.MainWindow is MainWindow main)
            {
                main.MainContent.Content = new StudentsEditingWindow(selectedStudent);
            }
        }

        private void Delete_button(object sender, RoutedEventArgs e)
        {
            var selectedStudents = StudentsDataGrid.SelectedItems.OfType<Student>().ToList();

            if (selectedStudents.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sinh viên để xóa.",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string confirmationMessage = selectedStudents.Count == 1
                ? $"Bạn có chắc chắn muốn xóa sinh viên: {selectedStudents[0].HoTen} ({selectedStudents[0].MaSV})?"
                : $"Bạn có chắc chắn muốn xóa {selectedStudents.Count} sinh viên đã chọn?";

            MessageBoxResult result = MessageBox.Show(
                confirmationMessage,
                "Xác nhận Xóa Dữ liệu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var deleteService = new DeleteStudents(_connectionString);
                int successCount = 0, errorCount = 0;

                foreach (var student in selectedStudents)
                {
                    try
                    {
                        deleteService.DeleteStudentByMaSV(student.MaSV);
                        successCount++;
                    }
                    catch
                    {
                        errorCount++;
                    }
                }

                if (successCount > 0)
                    MessageBox.Show($"Đã xóa thành công {successCount} sinh viên.",
                                    "Hoàn tất", MessageBoxButton.OK, MessageBoxImage.Information);

                if (errorCount > 0)
                    MessageBox.Show($"Cảnh báo: Có {errorCount} sinh viên không thể xóa do lỗi cơ sở dữ liệu.",
                                    "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
