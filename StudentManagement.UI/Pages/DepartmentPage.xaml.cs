using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using MySqlConnector;

namespace StudentManagement.UI.Pages
{
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
                    string query = "SELECT MaKhoa, TenKhoa FROM khoa";
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

        // ===== Nút thêm =====
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
                    MessageBox.Show("✅ Thêm Khoa thành công!");
                    LoadDepartments();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi thêm Khoa: " + ex.Message);
            }
        }

        // ===== Nút sửa =====
        private void Update_Department_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một Khoa từ danh sách để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataRowView selectedRow = (DataRowView)DepartmentDataGrid.SelectedItem;
            string oldMaKhoa = selectedRow["MaKhoa"].ToString(); // Mã Khoa cũ trong DB

            string newMaKhoa = txtDepartmentId.Text.Trim(); // Mã Khoa mới người dùng nhập
            string tenKhoa = txtNameDepart.Text.Trim();

            if (string.IsNullOrEmpty(newMaKhoa) || string.IsNullOrEmpty(tenKhoa))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin Khoa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Bắt đầu transaction để đảm bảo an toàn khi thay đổi khóa chính
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1️⃣ Cập nhật các bảng liên quan nếu Mã Khoa thay đổi
                            if (!oldMaKhoa.Equals(newMaKhoa, StringComparison.OrdinalIgnoreCase))
                            {
                                // Cập nhật bảng lop
                                string updateLop = "UPDATE lop SET MaKhoa=@newMaKhoa WHERE MaKhoa=@oldMaKhoa";
                                using (var cmd = new MySqlCommand(updateLop, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@newMaKhoa", newMaKhoa);
                                    cmd.Parameters.AddWithValue("@oldMaKhoa", oldMaKhoa);
                                    cmd.ExecuteNonQuery();
                                }

                                // Cập nhật bảng sinh_vien
                                string updateSV = "UPDATE sinh_vien SET MaKhoa=@newMaKhoa WHERE MaKhoa=@oldMaKhoa";
                                using (var cmd = new MySqlCommand(updateSV, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@newMaKhoa", newMaKhoa);
                                    cmd.Parameters.AddWithValue("@oldMaKhoa", oldMaKhoa);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // 2️⃣ Cập nhật chính bảng khoa
                            string query = "UPDATE khoa SET MaKhoa=@newMaKhoa, TenKhoa=@TenKhoa WHERE MaKhoa=@oldMaKhoa";
                            using (var cmd = new MySqlCommand(query, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@newMaKhoa", newMaKhoa);
                                cmd.Parameters.AddWithValue("@TenKhoa", tenKhoa);
                                cmd.Parameters.AddWithValue("@oldMaKhoa", oldMaKhoa);
                                int rows = cmd.ExecuteNonQuery();

                                if (rows > 0)
                                {
                                    transaction.Commit();
                                    MessageBox.Show("✅ Cập nhật Khoa thành công (bao gồm các bảng liên quan)!");
                                }
                                else
                                {
                                    transaction.Rollback();
                                    MessageBox.Show("❌ Không tìm thấy Khoa để cập nhật!");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("❌ Lỗi khi cập nhật Khoa: " + ex.Message);
                        }
                    }
                }

                LoadDepartments();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi hệ thống: " + ex.Message);
            }
        }

        // ===== Nút xóa =====
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
                        MessageBox.Show("✅ Xóa Khoa thành công!");
                        ClearForm();
                        LoadDepartments();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Lỗi khi xóa Khoa: " + ex.Message);
                }
            }
        }

        // ===== Khi chọn dòng trong DataGrid =====
        private void DepartmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem == null)
            {
                txtDepartmentId.Clear();
                txtNameDepart.Clear();
                txtDepartmentId.IsReadOnly = false;
                return;
            }

            if (DepartmentDataGrid.SelectedItem is DataRowView row)
            {
                txtDepartmentId.Text = row["MaKhoa"]?.ToString() ?? "";
                txtNameDepart.Text = row["TenKhoa"]?.ToString() ?? "";
                //txtDepartmentId.IsReadOnly = true;
            }
        }


        // ===== Nút tìm kiếm =====
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
                MessageBox.Show("❌ Lỗi khi tìm kiếm: " + ex.Message);
            }
        }

        // ===== Hàm tiện ích =====
        private void ClearForm()
        {
            txtDepartmentId.Text = "";
            txtNameDepart.Text = "";
            txtDepartmentId.IsReadOnly = false;
            DepartmentDataGrid.SelectedItem = null;
        }
    }
}
