using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace StudentManagement.UI.Pages
{
    /// <summary>
    /// Interaction logic for DepartmentPage.xaml
    /// </summary>
    public partial class DepartmentPage : UserControl
    {
        private readonly string connectionString;

        public DepartmentPage()
        {
            InitializeComponent();

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();
            connectionString = config.GetConnectionString("Default") ?? string.Empty;

            LoadDepartments();
        }

        // Cuộn ngang trong form
        private void svForm_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var sv = (ScrollViewer)sender;
            double newOffset = sv.HorizontalOffset - e.Delta;
            if (newOffset < 0) newOffset = 0;
            if (newOffset > sv.ScrollableWidth) newOffset = sv.ScrollableWidth;
            sv.ScrollToHorizontalOffset(newOffset);
            e.Handled = true;
        }

        // Load dữ liệu lên DataGrid
        private void LoadDepartments()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MaKhoa, TenKhoa FROM khoa";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    DepartmentDataGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách khoa: " + ex.Message);
            }
        }

        // Thêm Khoa
        private void Add_Department_Click(object sender, RoutedEventArgs e)
        {
            string maKhoa = txtDepartmentId.Text.Trim();
            string tenKhoa = txtNameDepart.Text.Trim();

            if (string.IsNullOrEmpty(maKhoa) || string.IsNullOrEmpty(tenKhoa))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin Khoa.", "Thông báo");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO khoa (MaKhoa, TenKhoa) VALUES (@MaKhoa, @TenKhoa)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                    cmd.Parameters.AddWithValue("@TenKhoa", tenKhoa);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm Khoa thành công!");
                    ClearForm();
                    LoadDepartments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm Khoa: " + ex.Message);
            }
        }

        // Sửa Khoa
        private void Update_Department_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Khoa cần sửa.");
                return;
            }

            string maKhoa = txtDepartmentId.Text.Trim();
            string tenKhoa = txtNameDepart.Text.Trim();

            if (string.IsNullOrEmpty(maKhoa) || string.IsNullOrEmpty(tenKhoa))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin Khoa.", "Thông báo");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE khoa SET TenKhoa=@TenKhoa WHERE MaKhoa=@MaKhoa";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                    cmd.Parameters.AddWithValue("@TenKhoa", tenKhoa);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật Khoa thành công!");
                        ClearForm();
                        LoadDepartments();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy Khoa để cập nhật.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật Khoa: " + ex.Message);
            }
        }

        // Xóa Khoa
        private void Delete_Department_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Khoa cần xóa.");
                return;
            }

            DataRowView row = (DataRowView)DepartmentDataGrid.SelectedItem;
            string maKhoa = row["MaKhoa"].ToString();

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra xem Khoa còn Lớp không
                    string checkQuery = "SELECT COUNT(*) FROM lop WHERE MaKhoa=@MaKhoa";
                    using (var checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                        long classCount = (long)checkCmd.ExecuteScalar();

                        if (classCount > 0)
                        {
                            MessageBox.Show($"Khoa này còn {classCount} lớp. Không thể xóa Khoa còn lớp.",
                                            "Lỗi xóa Khoa", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    // Xác nhận xóa
                    if (MessageBox.Show($"Bạn có chắc muốn xóa Khoa {maKhoa}?", "Xác nhận", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                        return;

                    // Thực hiện xóa Khoa
                    string deleteQuery = "DELETE FROM khoa WHERE MaKhoa=@MaKhoa";
                    using (var deleteCmd = new MySqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                        int rows = deleteCmd.ExecuteNonQuery();
                        MessageBox.Show(rows > 0 ? "Xóa Khoa thành công!" : "Không tìm thấy Khoa.");
                    }

                    ClearForm();
                    LoadDepartments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa Khoa: " + ex.Message);
            }
        }


        // Khi chọn một dòng trên DataGrid
        private void DepartmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem != null)
            {
                DataRowView row = (DataRowView)DepartmentDataGrid.SelectedItem;
                txtDepartmentId.Text = row["MaKhoa"].ToString();
                txtNameDepart.Text = row["TenKhoa"].ToString();
            }
        }

        // Clear form
        private void ClearForm()
        {
            txtDepartmentId.Text = "";
            txtNameDepart.Text = "";
            DepartmentDataGrid.SelectedItem = null;
        }

        // Search Khoa
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string queryText = SearchBox.Text.Trim();
            if (string.IsNullOrEmpty(queryText))
            {
                LoadDepartments();
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MaKhoa, TenKhoa FROM khoa WHERE MaKhoa LIKE @text OR TenKhoa LIKE @text";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@text", "%" + queryText + "%");
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    DepartmentDataGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
            }
        }
    }
}
