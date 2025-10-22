using StudentManagement;
using StudentManagement.Core;
using StudentManagement.UI.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StudentManagement.UI.Functions.StudentsFunc
{
    public partial class StudentsEditingWindow : UserControl
    {
        private Student editingStudent;
        private ReadClasses cRead = new ReadClasses();
        private ReadFaculties fRead = new ReadFaculties();

        private List<Class> classAll;
        private List<Faculty> facultyAll;

        // Constructor mặc định
        public StudentsEditingWindow()
        {
            InitializeComponent();
            LoadStaticData();
        }

        // Constructor nhận student
        public StudentsEditingWindow(Student student) : this()
        {
            editingStudent = student;
            LoadStudentData(student);
        }

        // --- HÀM TẢI DỮ LIỆU TĨNH ---
        private void LoadStaticData()
        {
            try
            {
                facultyAll = fRead.GetFaculties();
                classAll = cRead.GetClasses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu tĩnh: {ex.Message}", "Lỗi Cơ sở dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                facultyAll = new List<Faculty>();
                classAll = new List<Class>();
            }
        }

        // --- HÀM GÁN DỮ LIỆU SINH VIÊN (Sử dụng Gán trực tiếp) ---
        private void LoadStudentData(Student student)
        {
            if (student == null) return;

            // GÁN GIÁ TRỊ TỪ STUDENT VÀO TẤT CẢ CÁC CONTROLS
            txtStudentId.Text = student.MaSV;
            txtName.Text = student.HoTen;
            dpDateOfBirth.SelectedDate = student.NgaySinh;
            cbGender.Text = student.GioiTinh;
            txtEmail.Text = student.Email;
            txtSdt.Text = student.SoDienThoai;
            txtAdress.Text = student.DiaChi;

            // 1. Gán danh sách Khoa và chọn Khoa hiện tại
            txtFaculty.ItemsSource = facultyAll;
            txtFaculty.SelectedValue = student.MaKhoa;

            // 2. Tải danh sách Lớp phù hợp và chọn Lớp hiện tại
            LoadDataClass(student.MaKhoa, student.MaLop);

            // 3. Khởi tạo trạng thái Checkbox
            if (chkTrangThai != null)
            {
                // Gán giá trị IsChecked từ database
                chkTrangThai.IsChecked = student.Check_TrangThai;

                // BUỘC KÍCH HOẠT SỰ KIỆN UI:
                // Tùy thuộc vào giá trị, chúng ta kích hoạt sự kiện tương ứng để cập nhật màu và chữ.
                if (student.Check_TrangThai)
                {
                    // Nếu Đang học (True): Kích hoạt sự kiện Checked
                    chkTrangThai.RaiseEvent(new RoutedEventArgs(CheckBox.CheckedEvent));
                }
                else
                {
                    // Nếu Nghỉ học (False): Kích hoạt sự kiện Unchecked
                    chkTrangThai.RaiseEvent(new RoutedEventArgs(CheckBox.UncheckedEvent));
                }
            }
        }

        // --- HÀM LỌC LỚP VÀ CẬP NHẬT COMBOBOX ---
        private void LoadDataClass(string maKhoa, string maLopToSelect = null)
        {
            txtClass.ItemsSource = null;
            txtClass.SelectedIndex = -1;

            if (string.IsNullOrEmpty(maKhoa) || classAll == null)
            {
                return;
            }

            try
            {
                // Lọc các lớp theo FacultyCode (tên thuộc tính trong Class model)
                var filteredClasses = classAll
                    .Where(c => c.FacultyCode == maKhoa)
                    .ToList();

                txtClass.ItemsSource = filteredClasses;

                // Chọn Lớp hiện tại
                if (!string.IsNullOrEmpty(maLopToSelect))
                {
                    txtClass.SelectedValue = maLopToSelect;
                }
            }
            catch (Exception)
            {
                txtClass.ItemsSource = null;
                txtClass.SelectedIndex = -1;
            }
        }

        // --- SỰ KIỆN KHI KHOA THAY ĐỔI (Dùng để Lọc Lớp) ---
        private void OnFacultyChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedMaKhoa = null;

            if (txtFaculty.SelectedValue != null)
            {
                selectedMaKhoa = txtFaculty.SelectedValue.ToString();
            }

            LoadDataClass(selectedMaKhoa, null);
        }

        // --- HÀM XỬ LÝ UI CHECKBOX (Đổi màu/chữ) ---
        private void CheckBox_StatusChanged(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox chk)
            {
                // 1. Cập nhật màu chữ và nội dung (UI)
                if (chk.IsChecked == true)
                {
                    chk.Content = "Đang học";
                    chk.Foreground = Brushes.Green;
                }
                else
                {
                    chk.Content = "Nghỉ học";
                    chk.Foreground = Brushes.Red;
                }

                // 2. Cập nhật Model ngay lập tức (phục vụ cho Save_Click)
                if (editingStudent != null)
                {
                    editingStudent.Check_TrangThai = chk.IsChecked ?? false;
                }
            }
        }

        // --- NÚT LƯU ---
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (editingStudent == null) return;

            // THU THẬP TẤT CẢ GIÁ TRỊ VÀO ĐỐI TƯỢNG (Thủ công)
            editingStudent.MaSV = txtStudentId.Text;
            editingStudent.HoTen = txtName.Text;
            editingStudent.NgaySinh = dpDateOfBirth.SelectedDate ?? DateTime.MinValue;
            editingStudent.GioiTinh = cbGender.Text;
            editingStudent.Email = txtEmail.Text;
            editingStudent.SoDienThoai = txtSdt.Text;
            editingStudent.DiaChi = txtAdress.Text;

            // Lấy mã từ SelectedValue của ComboBox
            editingStudent.MaKhoa = txtFaculty.SelectedValue?.ToString();
            editingStudent.MaLop = txtClass.SelectedValue?.ToString();

            // Check_TrangThai đã được cập nhật bởi CheckBox_StatusChanged

            // GỌI HÀM CẬP NHẬT VÀO DATABASE
            try
            {
                UpdateStudents service = new UpdateStudents();
                service.UpdateStudent(editingStudent);

                MessageBox.Show("Cập nhật sinh viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật Database: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Quay lại StudentsPage
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new StudentManagement.UI.Pages.StudentsPage();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new StudentManagement.UI.Pages.StudentsPage();
            }
        }
    }
}