using StudentManagement.Core;
using StudentManagement.UI.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection; // Cần cho hàm SelectComboBoxItem
using System.Windows;
using System.Windows.Controls;
using StudentManagement;

namespace StudentManagement.UI.Functions.StudentsFunc
{
    public partial class StudentsEditingWindow : UserControl
    {
        private Student editingStudent;
        private ReadClasses cRead = new ReadClasses();
        private ReadFaculties fRead = new ReadFaculties();

        private List<Class> classAll;    // Toàn bộ danh sách Lớp (để lọc)
        private List<Faculty> facultyAll; // Toàn bộ danh sách Khoa (để chọn)

        // Constructor mặc định (gọi InitializeComponent)
        public StudentsEditingWindow()
        {
            InitializeComponent();
            LoadStaticData(); // Tải dữ liệu tĩnh ngay khi khởi tạo
        }

        // Constructor nhận student
        public StudentsEditingWindow(Student student) : this()
        {
            editingStudent = student;
            LoadStudentData(student);
        }

        // --- CÁC HÀM TẢI DỮ LIỆU TĨNH VÀ CỦA SINH VIÊN ---

        private void LoadStaticData()
        {
            try
            {
                // Tải tất cả Khoa và Lớp vào bộ nhớ (Chỉ 1 lần)
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

        private void LoadStudentData(Student student)
        {
            if (student == null) return;

            // Gán các trường dữ liệu
            txtStudentId.Text = student.MaSV;
            txtName.Text = student.HoTen;
            dpDateOfBirth.SelectedDate = student.NgaySinh;
            cbGender.Text = student.GioiTinh;
            txtEmail.Text = student.Email;
            txtSdt.Text = student.SoDienThoai;
            // Gán các giá trị khác...

            // 1. Gán danh sách Khoa và chọn Khoa hiện tại
            txtFaculty.ItemsSource = facultyAll;
            SelectComboBoxItem(txtFaculty, student.MaKhoa, "FacultyCode");

            // 2. Tải danh sách Lớp phù hợp với Khoa đã chọn và chọn Lớp hiện tại
            LoadDataClass(student.MaKhoa, student.MaLop);
        }

        // --- HÀM LỌC LỚP VÀ CẬP NHẬT COMBOBOX ---

        private void LoadDataClass(string maKhoa, string maLopToSelect = null)
        {
            // 1. RESET ComboBox Lớp
            txtClass.ItemsSource = null;
            txtClass.SelectedIndex = -1;

            if (string.IsNullOrEmpty(maKhoa) || classAll == null)
            {
                return;
            }

            try
            {
                // 2. Lọc tất cả các lớp thuộc về MaKhoa được chỉ định
                var filteredClasses = classAll
                    .Where(c => c.FacultyCode == maKhoa)
                    .ToList();

                // 3. Gán danh sách đã lọc
                txtClass.ItemsSource = filteredClasses;

                // 4. Chọn Lớp hiện tại nếu maLopToSelect được cung cấp
                if (!string.IsNullOrEmpty(maLopToSelect))
                {
                    SelectComboBoxItem(txtClass, maLopToSelect, "ClassCode");
                }
            }
            catch
            {
                // Xử lý lỗi nếu có
                txtClass.ItemsSource = null;
                txtClass.SelectedIndex = -1;
            }
        }

        // --- SỰ KIỆN KHI KHOA THAY ĐỔI (Dùng để Lọc Lớp) ---

        private void OnFacultyChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFacultyCode = null;

            // Kiểm tra xem có giá trị hợp lệ nào được chọn không
            if (txtFaculty.SelectedValue != null)
            {
                selectedFacultyCode = txtFaculty.SelectedValue.ToString();
            }

            // Gọi LoadDataClass để lọc. 
            // KHÔNG truyền mã lớp cũ (maLopToSelect = null) để danh sách Lớp mới không tự chọn.
            LoadDataClass(selectedFacultyCode, null);
        }

        // --- HÀM TIỆN ÍCH CHO VIỆC CHỌN COMBOBOX ---

        private void SelectComboBoxItem(ComboBox cmb, string selectedValue, string valuePath)
        {
            if (cmb.ItemsSource == null || string.IsNullOrEmpty(selectedValue)) return;

            // Tìm đối tượng trong ItemsSource có thuộc tính valuePath bằng selectedValue
            var selectedItem = cmb.Items.OfType<object>()
                .FirstOrDefault(item =>
                {
                    // Lấy giá trị của thuộc tính (FacultyCode/ClassCode) qua Reflection
                    var prop = item.GetType().GetProperty(valuePath);
                    // Đảm bảo thuộc tính tồn tại và giá trị khớp
                    return prop != null && prop.GetValue(item)?.ToString() == selectedValue;
                });

            if (selectedItem != null)
            {
                cmb.SelectedItem = selectedItem;
            }
        }

        // --- CÁC HÀM XỬ LÝ NÚT (SỰ KIỆN) ---

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (editingStudent == null) return;

            // Lấy Mã Lớp và Mã Khoa đã chọn từ ComboBox
            string selectedMaLop = txtClass.SelectedValue?.ToString();
            string selectedMaKhoa = txtFaculty.SelectedValue?.ToString();

            // 1. GÁN GIÁ TRỊ VÀO ĐỐI TƯỢNG (Đã hoàn thành)
            editingStudent.HoTen = txtName.Text;
            editingStudent.NgaySinh = dpDateOfBirth.SelectedDate ?? DateTime.MinValue;
            editingStudent.GioiTinh = (cbGender.SelectedItem as ComboBoxItem)?.Content.ToString() ?? cbGender.Text;
            editingStudent.Email = txtEmail.Text;
            editingStudent.SoDienThoai = txtSdt.Text;
            editingStudent.MaLop = selectedMaLop;
            editingStudent.MaKhoa = selectedMaKhoa;

            // 2. PHẦN BỔ SUNG: GỌI HÀM CẬP NHẬT VÀO DATABASE
            try
            {
                UpdateStudents service = new UpdateStudents();
                service.UpdateStudent(editingStudent);

                MessageBox.Show("Cập nhật sinh viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật Database: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Dừng lại nếu lỗi
            }

            // 3. Quay lại StudentsPage (Đã hoàn thành)
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

        private void txtClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Logic tùy chỉnh khi Lớp thay đổi (nếu cần)
        }
    }
}