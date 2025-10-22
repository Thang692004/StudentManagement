using System;
using System.Collections.Generic;
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
using StudentManagement.UI.Functions.StudentsFunc;
using StudentManagement.Core;

namespace StudentManagement.UI.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class StudentsPage : UserControl
    {
        ReadStudents read = new ReadStudents();
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
                System.Diagnostics.Debug.WriteLine($"Số lượng sinh viên lấy được: {students.Count}");
                foreach (var s in students)
                {
                    // Cập nhật sinh viên
                    System.Diagnostics.Debug.WriteLine(
                        $"{s.MaSV} - {s.HoTen} - {s.NgaySinh:dd/MM/yyyy} - {s.GioiTinh} - {s.Email} - {s.SoDienThoai} - {s.DiaChi} - {s.TenLop} - {s.TenKhoa}"
                    );
                }

                StudentsDataGrid.ItemsSource = students;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi load students: {ex.Message}");
                MessageBox.Show("Có lỗi xảy ra khi tải danh sách sinh viên.",
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnAdding_Click(object sender, RoutedEventArgs e)
        {
            // Lấy MainWindow
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new StudentsAddingWindow(); // thay thế StudentsPage
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
            var editwindow = Application.Current.MainWindow as MainWindow;

            editwindow.MainContent.Content = new StudentsEditingWindow(selectedStudent);
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
