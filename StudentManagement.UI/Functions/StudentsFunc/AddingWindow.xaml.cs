using StudentManagement.Core;
using StudentManagement.UI.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StudentManagement;

namespace StudentManagement.UI.Functions.StudentsFunc
{
    public partial class StudentsAddingWindow : UserControl
    {
        private ReadClasses cRead = new ReadClasses();
        private List<Class> classAll;
        public StudentsAddingWindow()
        {
            InitializeComponent();
            classAll = cRead.GetClasses();

            LoadDataFaculty();
            
        }

        private void LoadDataFaculty()
        {
            try
            {
             
                ReadFaculties fRead = new ReadFaculties();
                var fList = fRead.GetFaculties(); // Lấy danh sách khoa

                txtFaculty.ItemsSource = fList;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách Khoa: {ex.Message}", "Lỗi Cơ sở dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new StudentsPage();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Student newStudent = new Student
            {
                MaSV = txtStudentId.Text,
                HoTen = txtName.Text,
                NgaySinh = dpDateOfBirth.SelectedDate ?? DateTime.MinValue,
                GioiTinh = (cbGender.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Email = txtEmail.Text,
                SoDienThoai = txtSdt.Text,
                MaLop = txtClass.SelectedValue?.ToString(),   // Lấy mã lớp
                MaKhoa = txtFaculty.SelectedValue?.ToString() // Lấy mã khoa
                                                              // Giả sử bạn có thể lấy các thuộc tính này từ các controls tương ứng
            };

            try
            {
                // 2. Gọi dịch vụ INSERT
                InsertStudents service = new InsertStudents();
                service.AddStudent(newStudent);

                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm Database: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 3. Quay lại StudentsPage
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContent.Content = new StudentManagement.UI.Pages.StudentsPage();
            }
        }

        private void txtStudentId_TextChanged(object sender, TextChangedEventArgs e) { }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) { }

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
                // Reset các gía trị trong combox của 'Class'
                txtClass.ItemsSource = null;
                txtClass.SelectedIndex = -1;

                if (string.IsNullOrEmpty(maKhoa) || classAll == null) 
                {
                    return; // Dừng lại sau khi đã reset
                }

                var filteredClasses = classAll.Where(c => c.FacultyCode == maKhoa).ToList();
                txtClass.ItemsSource = filteredClasses;
            }
            catch
            {
                txtClass.ItemsSource = null;
                txtClass.SelectedIndex = -1;
            }
            
        }

    }
}