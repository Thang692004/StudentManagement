using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySqlConnector;

namespace StudentManagement.UI.Pages
{
    /// <summary>
    /// Interaction logic for DepartmentPage.xaml
    /// </summary>
    public partial class DepartmentPage : UserControl
    {
        private string connectionString = "server=127.0.0.1;database=quanlysinhvien;uid=root;pwd=;"; 

        public DepartmentPage()
        {
            InitializeComponent();
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
                    string query = "SELECT MaKhoa, TenKhoa FROM khoa"; // Truy vấn bảng "khoa"
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
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

        private void Add_Department_Click(object sender, RoutedEventArgs e)
        {
            string maKhoa = txtDepartmentId.Text.Trim();
            string tenKhoa = txtNameDepart.Text.Trim();

            if (string.IsNullOrEmpty(maKhoa) || string.IsNullOrEmpty(tenKhoa))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin Khoa.");
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
                    LoadDepartments(); // Tải lại dữ liệu
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
                MessageBox.Show("Vui lòng chọn một Khoa từ danh sách để sửa.", "Chưa chọn Khoa", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string maKhoa = txtDepartmentId.Text.Trim();
            string tenKhoa = txtNameDepart.Text.Trim();

            if (string.IsNullOrEmpty(tenKhoa))
            {
                MessageBox.Show("Tên Khoa không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                MessageBox.Show("Lỗi khi cập nhật Khoa: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

            if (MessageBox.Show($"Bạn có chắc muốn xóa Khoa {maKhoa}?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM khoa WHERE MaKhoa=@MaKhoa";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa Khoa thành công!");
                        ClearForm();
                        LoadDepartments();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa Khoa: " + ex.Message);
                }
            }
        }

        // Khi chọn một dòng trên DataGrid
        private void DepartmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem is DataRowView row)
            {
                txtDepartmentId.Text = row["MaKhoa"].ToString();
                txtNameDepart.Text = row["TenKhoa"].ToString();

                // KHÓA ô Mã Khoa lại khi người dùng chọn một dòng để sửa
                txtDepartmentId.IsReadOnly = true;
            }
        }

        // Clear form
        private void ClearForm()
        {
            txtDepartmentId.Text = "";
            txtNameDepart.Text = "";
            DepartmentDataGrid.SelectedItem = null;
            txtDepartmentId.IsReadOnly = false;
            txtDepartmentId.Focus();
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
