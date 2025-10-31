using StudentManagement.Core;
using StudentManagement.UI.Functions.StudentsFunc;
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

namespace StudentManagement.UI.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class StudentsPage : UserControl, ISearchableContent
    {
        ReadStudents read = new ReadStudents();
        private ObservableCollection<Student> _allStudents;
        private ICollectionView _studentView;
        public StudentsPage()
        {
            InitializeComponent();
            LoadStudents();
        }
        private void LoadStudents()
        {
            try
            {
                List<Student> students = read.GetStudents();
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

            string query = searchText.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                // CHỈ XÓA BỘ LỌC
                _studentView.Filter = null;
            }
            else
            {
                // THIẾT LẬP BỘ LỌC
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
            if (_studentView.IsEmpty && !string.IsNullOrWhiteSpace(query))
            {
                // Không có kết quả VÀ người dùng đã gõ thứ gì đó
                StudentsDataGrid.Visibility = Visibility.Collapsed;
                NoResultsText.Visibility = Visibility.Visible;
            }
            else
            {
                // Có kết quả (hoặc chuỗi tìm kiếm rỗng, nên hiển thị DataGrid)
                StudentsDataGrid.Visibility = Visibility.Visible;
                NoResultsText.Visibility = Visibility.Collapsed;
            }
        }
        private void BtnAdding_Click(object sender, RoutedEventArgs e)
        {
            // Cố gắng lấy MainWindow một cách tin cậy: ưu tiên cửa sổ chứa UserControl
            var mainWindow = Window.GetWindow(this) as MainWindow
                             ?? Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new StudentsAddingWindow(); // thay thế StudentsPage
            }
            else
            {
                // Thông báo thay vì im lặng, dễ dàng phát hiện lỗi khi Application.Current.MainWindow không được set
                MessageBox.Show("Không thể chuyển trang: không tìm thấy cửa sổ chính.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

            // 2. Chọn nhiều dòng
            if (StudentsDataGrid.SelectedItems.Count > 1)
            {
                MessageBox.Show("Vui lòng chỉ chọn một sinh viên để sửa.",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // 3. Chọn đúng 1 sinh viên
            var selectedStudent = StudentsDataGrid.SelectedItem as Student;

            var mainWindow = Window.GetWindow(this) as MainWindow
                             ?? Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new StudentsEditingWindow(selectedStudent);
            }
            else
            {
                MessageBox.Show("Không thể chuyển trang để sửa: không tìm thấy cửa sổ chính.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_button(object sender, RoutedEventArgs e)
        {
            var selectedStudents = StudentsDataGrid.SelectedItems.OfType<Student>().ToList();

            // --- BƯỚC 1: KIỂM TRA CHỌN (Bạn phải chọn) ---
            if (selectedStudents.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sinh viên để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- BƯỚC 2: XÁC NHẬN XÓA ---
            string confirmationMessage;
            if (selectedStudents.Count == 1)
            {
                // Nếu chỉ chọn 1 sinh viên, hiển thị tên của họ
                confirmationMessage = $"Bạn có chắc chắn muốn xóa sinh viên: {selectedStudents[0].HoTen} ({selectedStudents[0].MaSV})?";
            }
            else
            {
                // Nếu chọn nhiều sinh viên
                confirmationMessage = $"Bạn có chắc chắn muốn xóa {selectedStudents.Count} sinh viên đã chọn?";
            }

            // Hiển thị hộp thoại xác nhận
            MessageBoxResult result = MessageBox.Show(
                confirmationMessage,
                "Xác nhận Xóa Dữ liệu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // --- BƯỚC 3: THỰC HIỆN XÓA NẾU NGƯỜI DÙNG CHỌN 'CÓ' (Yes) ---
            if (result == MessageBoxResult.Yes)
            {
                DeleteStudents service = new DeleteStudents();
                int successCount = 0;
                int errorCount = 0;

                foreach (var student in selectedStudents)
                {
                    try
                    {
                        // Gọi dịch vụ xóa từng sinh viên một
                        service.DeleteStudentByMaSV(student.MaSV);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        // Tùy chọn: Ghi log hoặc hiển thị chi tiết lỗi cho sinh viên bị lỗi
                    }
                }

                // --- BƯỚC 4: THÔNG BÁO KẾT QUẢ VÀ TẢI LẠI DỮ LIỆU ---
                if (successCount > 0)
                {
                    MessageBox.Show($"Đã xóa thành công {successCount} sinh viên.", "Hoàn tất", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (errorCount > 0)
                {
                    MessageBox.Show($"Cảnh báo: Có {errorCount} sinh viên không thể xóa do lỗi cơ sở dữ liệu.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
        }
    
    }
}
