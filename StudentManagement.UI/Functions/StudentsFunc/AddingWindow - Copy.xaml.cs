using StudentManagement.Core;
using StudentManagement.UI.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection; // Cần cho hàm SelectComboBoxItem (Dù bạn chưa dùng trong AddingWindow)

namespace StudentManagement.UI.Functions.StudentsFunc
{
    public partial class StudentsAddingWindow : UserControl
    {
        private ReadClasses cRead = new ReadClasses();
        private List<Class> classAll;

        public StudentsAddingWindow()
        {
            InitializeComponent();
            // Đảm bảo txtFaculty và txtClass tồn tại trong XAML
            classAll = cRead.GetClasses();
            LoadDataFaculty();
        }

        private void LoadDataFaculty()
        {
            try
            {
                ReadFaculties fRead = new ReadFaculties();
                var fList = fRead.GetFaculties();

                txtFaculty.ItemsSource = fList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách Khoa: {ex.Message}", "Lỗi Cơ sở dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --- NÚT HỦY ---
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new StudentsPage();
            }
        }

        // --- NÚT LƯU ---
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Cần đảm bảo Checkbox trong XAML có x:Name="chkTrangThai"
            bool isStudying = chkTrangThai.IsChecked ?? false;

            Student newStudent = new Student
            {
                MaSV = txtStudentId.Text,
                HoTen = txtName.Text,
                NgaySinh = dpDateOfBirth.SelectedDate ?? DateTime.MinValue,
                GioiTinh = (cbGender.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Email = txtEmail.Text,
                SoDienThoai = txtSdt.Text,
                MaLop = txtClass.SelectedValue?.ToString(),
                MaKhoa = txtFaculty.SelectedValue?.ToString(),
                DiaChi = txtAdress.Text,
                // GÁN GIÁ TRỊ TRẠNG THÁI
                Check_TrangThai = isStudying
            };

            try
            {
                // Gọi dịch vụ INSERT
                InsertStudents service = new InsertStudents();
                // Đảm bảo InsertStudents.AddStudent đã được chỉnh sửa để lưu Check_TrangThai
                service.AddStudent(newStudent);

                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm Database: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Quay lại StudentsPage
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new StudentsPage();
            }
        }

        // --- HÀM XỬ LÝ LỌC LỚP ---

        private void OnFacultyChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFacultyCode = null;

            if (txtFaculty.SelectedValue != null)
            {
                selectedFacultyCode = txtFaculty.SelectedValue.ToString();
            }
            LoadDataClass(selectedFacultyCode);
        }

        private void LoadDataClass(string maKhoa)
        {
            try
            {
                txtClass.ItemsSource = null;
                txtClass.SelectedIndex = -1;

                if (string.IsNullOrEmpty(maKhoa) || classAll == null)
                {
                    return;
                }

                // Cần đảm bảo Class model có thuộc tính FacultyCode
                var filteredClasses = classAll.Where(c => c.FacultyCode == maKhoa).ToList();
                txtClass.ItemsSource = filteredClasses;
            }
            catch
            {
                txtClass.ItemsSource = null;
                txtClass.SelectedIndex = -1;
            }

        }

        private void CheckBox_StatusChanged(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox chk)
            {
                // Cập nhật màu chữ dựa trên trạng thái (UI)
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
            }
        }

    }
}