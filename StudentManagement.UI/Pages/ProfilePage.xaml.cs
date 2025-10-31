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
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace StudentManagement.UI.Pages
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : UserControl
    {
        private readonly string? _maSV;
        public ProfilePage(string? maSV = null)
        {
            InitializeComponent();
            _maSV = maSV;

            if (!string.IsNullOrWhiteSpace(_maSV))
            {
                _ = LoadProfileAsync(_maSV);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Allow editing for student info (except MSV, username, class/faculty/status when present)
            SetReadOnly(false);
            // enable password editing
            pwdBox.IsEnabled = true;
            BtnEdit.Visibility = Visibility.Collapsed;
            BtnSave.Visibility = Visibility.Visible;
        }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Persist only the fields that were editable (class, faculty, status)
            try
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
                var connStr = config.GetConnectionString("Default");
                if (string.IsNullOrWhiteSpace(connStr)) throw new Exception("Chuỗi kết nối không hợp lệ.");

                using var conn = new MySqlConnection(connStr);
                conn.Open();

                var updates = new List<string>();
                using var cmd = new MySqlCommand();
                cmd.Connection = conn;

                if (cbFaculty.Visibility == Visibility.Visible && cbFaculty.SelectedValue != null)
                {
                    cmd.Parameters.AddWithValue("@MaKhoa", cbFaculty.SelectedValue.ToString());
                    updates.Add("MaKhoa = @MaKhoa");
                }
                if (cbClass.Visibility == Visibility.Visible && cbClass.SelectedValue != null)
                {
                    cmd.Parameters.AddWithValue("@MaLop", cbClass.SelectedValue.ToString());
                    updates.Add("MaLop = @MaLop");
                }
                if (cbStatus.Visibility == Visibility.Visible && cbStatus.SelectedItem is ComboBoxItem statusItem)
                {
                    var tag = statusItem.Tag?.ToString() ?? "1";
                    cmd.Parameters.AddWithValue("@TrangThai", Convert.ToInt32(tag));
                    updates.Add("TrangThai = @TrangThai");
                }

                if (updates.Count > 0)
                {
                    cmd.CommandText = $"UPDATE sinh_vien SET {string.Join(", ", updates)} WHERE MaSV = @MaSV";
                    cmd.Parameters.AddWithValue("@MaSV", _maSV);
                    cmd.ExecuteNonQuery();
                }

                // Also persist editable student fields (HoTen, NgaySinh, GioiTinh)
                var studentUpdates = new List<string>();
                if (!string.IsNullOrWhiteSpace(txtName.Text))
                {
                    studentUpdates.Add("HoTen = @HoTen");
                    cmd.Parameters.AddWithValue("@HoTen", txtName.Text.Trim());
                }
                if (!string.IsNullOrWhiteSpace(txtDOB.Text))
                {
                    if (DateTime.TryParse(txtDOB.Text.Trim(), out var dt))
                    {
                        studentUpdates.Add("NgaySinh = @NgaySinh");
                        cmd.Parameters.AddWithValue("@NgaySinh", dt);
                    }
                }
                if (!string.IsNullOrWhiteSpace(txtGender.Text))
                {
                    studentUpdates.Add("GioiTinh = @GioiTinh");
                    cmd.Parameters.AddWithValue("@GioiTinh", txtGender.Text.Trim());
                }

                if (studentUpdates.Count > 0)
                {
                    cmd.CommandText = $"UPDATE sinh_vien SET {string.Join(", ", studentUpdates)} WHERE MaSV = @MaSV";
                    // Ensure MaSV param exists
                    if (!cmd.Parameters.Contains("@MaSV")) cmd.Parameters.AddWithValue("@MaSV", _maSV);
                    cmd.ExecuteNonQuery();
                }

                // Update password in tai_khoan if changed (and pwdBox not empty)
                if (!string.IsNullOrWhiteSpace(pwdBox.Password))
                {
                    using var cmd2 = new MySqlCommand("UPDATE tai_khoan SET MatKhau = @MatKhau WHERE MaSV = @MaSV", conn);
                    cmd2.Parameters.AddWithValue("@MatKhau", pwdBox.Password);
                    cmd2.Parameters.AddWithValue("@MaSV", _maSV);
                    cmd2.ExecuteNonQuery();
                }

                // reload profile to reflect changes
                if (!string.IsNullOrWhiteSpace(_maSV))
                {
                    await LoadProfileAsync(_maSV!);
                }

                SetReadOnly(true);
                BtnSave.Visibility = Visibility.Collapsed;
                BtnEdit.Visibility = Visibility.Visible;
                MessageBox.Show("Thông tin đã được lưu thành công!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetReadOnly(bool state)
        {
            // Fields that CAN be edited by student: Name, DOB, Gender, Password
            txtName.IsReadOnly = state;
            txtDOB.IsReadOnly = state;
            txtGender.IsReadOnly = state;

            // Locked fields (always non-editable): MSV (txtMSV), Username (txtUsername), Class/TextBox and Faculty/TextBox when present, Status Text
            txtUsername.IsReadOnly = true;
            txtClass.IsReadOnly = true;
            txtFaculty.IsReadOnly = true;

            // ComboBoxes are shown only when the value is missing; allow editing them when entering edit mode
            cbFaculty.IsEnabled = !state && cbFaculty.Visibility == Visibility.Visible;
            cbClass.IsEnabled = !state && cbClass.Visibility == Visibility.Visible;
            cbStatus.IsEnabled = !state && cbStatus.Visibility == Visibility.Visible;

            // Password enabled only in edit mode
            pwdBox.IsEnabled = !state;
        }

        private void CbFaculty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFaculty.SelectedValue != null)
            {
                _ = LoadClassesForFacultyAsync(cbFaculty.SelectedValue.ToString()!);
            }
        }

        private async Task LoadFacultiesAsync()
        {
            try
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
                var connStr = config.GetConnectionString("Default");
                using var conn = new MySqlConnection(connStr);
                conn.Open();
                using var cmd = new MySqlCommand("SELECT MaKhoa, TenKhoa FROM khoa", conn);
                using var rdr = cmd.ExecuteReader();
                var list = new List<(string MaKhoa, string TenKhoa)>();
                while (rdr.Read())
                {
                    list.Add((rdr["MaKhoa"]?.ToString() ?? string.Empty, rdr["TenKhoa"]?.ToString() ?? string.Empty));
                }
                cbFaculty.ItemsSource = list;
                cbFaculty.DisplayMemberPath = "TenKhoa";
                cbFaculty.SelectedValuePath = "MaKhoa";
            }
            catch { }
        }

        private async Task LoadClassesForFacultyAsync(string maKhoa)
        {
            try
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
                var connStr = config.GetConnectionString("Default");
                using var conn = new MySqlConnection(connStr);
                conn.Open();
                using var cmd = new MySqlCommand("SELECT MaLop, TenLop FROM lop WHERE MaKhoa = @maKhoa", conn);
                cmd.Parameters.AddWithValue("@maKhoa", maKhoa);
                using var rdr = cmd.ExecuteReader();
                var list = new List<(string MaLop, string TenLop)>();
                while (rdr.Read())
                {
                    list.Add((rdr["MaLop"]?.ToString() ?? string.Empty, rdr["TenLop"]?.ToString() ?? string.Empty));
                }
                cbClass.ItemsSource = list;
                cbClass.DisplayMemberPath = "TenLop";
                cbClass.SelectedValuePath = "MaLop";
            }
            catch { }
        }

        private async Task LoadProfileAsync(string maSV)
        {
            try
            {
                var reader = new StudentManagement.ReadStudents();
                var student = reader.GetStudentByMaSV(maSV);
                if (student != null)
                {
                    tbMSV.Text = $"MSV: {student.MaSV}";
                    txtMSV.Text = student.MaSV;
                    txtStatus.Text = student.TrangThai ?? "-";
                    txtName.Text = student.HoTen ?? string.Empty;
                    txtDOB.Text = student.NgaySinh?.ToString("yyyy-MM-dd") ?? string.Empty;
                    txtGender.Text = student.GioiTinh ?? string.Empty;
                    // class/faculty
                    if (!string.IsNullOrWhiteSpace(student.TenLop))
                    {
                        txtClass.Text = student.TenLop;
                        cbClass.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        txtClass.Text = string.Empty;
                        cbClass.Visibility = Visibility.Visible;
                    }

                    if (!string.IsNullOrWhiteSpace(student.TenKhoa))
                    {
                        txtFaculty.Text = student.TenKhoa;
                        cbFaculty.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        txtFaculty.Text = string.Empty;
                        cbFaculty.Visibility = Visibility.Visible;
                    }
                    // Permission (vai tro) from tai_khoan
                    // Load account info (username + password)
                    try
                    {
                        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
                        var connStr = config.GetConnectionString("Default");
                        if (!string.IsNullOrWhiteSpace(connStr))
                        {
                            using var conn = new MySqlConnection(connStr);
                            conn.Open();
                            using var cmd = new MySqlCommand("SELECT TenDangNhap, MatKhau FROM tai_khoan WHERE MaSV = @maSV LIMIT 1", conn);
                            cmd.Parameters.AddWithValue("@maSV", maSV);
                            using var rdr = cmd.ExecuteReader();
                            if (rdr.Read())
                            {
                                txtUsername.Text = rdr["TenDangNhap"]?.ToString() ?? string.Empty;
                                pwdBox.Password = rdr["MatKhau"]?.ToString() ?? string.Empty;
                            }
                        }
                    }
                    catch { }

                    // If faculty or class combo boxes are visible, load options
                    if (cbFaculty.Visibility == Visibility.Visible)
                    {
                        await LoadFacultiesAsync();
                    }
                    if (cbClass.Visibility == Visibility.Visible && !string.IsNullOrWhiteSpace(student.MaKhoa))
                    {
                        await LoadClassesForFacultyAsync(student.MaKhoa!);
                    }

                    // If status missing, show dropdown so user can set it; otherwise keep text
                    if (string.IsNullOrWhiteSpace(student.TrangThai))
                    {
                        txtStatus.Text = "-";
                        cbStatus.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        cbStatus.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadProfileAsync error: {ex.Message}");
            }
        }
    }
}
