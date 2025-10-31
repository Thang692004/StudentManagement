using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace StudentManagement.UI.Pages
{
    public partial class AccountsPage : UserControl
    {
        private readonly string connectionString;

        public AccountsPage()
        {
            InitializeComponent();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();
            connectionString = config.GetConnectionString("Default")
                ?? throw new Exception("Không tìm thấy chuỗi kết nối 'Default' trong appsettings.json.");
            LoadAccounts();
        }

        // Cuộn ngang trong ScrollViewer
        private void svForm_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var sv = (ScrollViewer)sender;
            double newOffset = sv.HorizontalOffset - e.Delta;

            if (newOffset < 0) newOffset = 0;
            if (newOffset > sv.ScrollableWidth) newOffset = sv.ScrollableWidth;

            sv.ScrollToHorizontalOffset(newOffset);
            e.Handled = true;
        }

        // ==================== LOAD DỮ LIỆU ====================
        private void LoadAccounts(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT MaSV, TenDangNhap AS Username, MatKhau AS Password, VaiTro FROM tai_khoan";
                    if (!string.IsNullOrEmpty(keyword))
                        sql += " WHERE MaSV LIKE @kw OR TenDangNhap LIKE @kw OR VaiTro LIKE @kw";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    if (!string.IsNullOrEmpty(keyword))
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    StudentsDataGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách tài khoản:\n" + ex.Message,
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ==================== THÊM ====================
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string maSV = txtStudentId.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = (cbRole.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? cbRole.Text?.Trim();

            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã SV, Username, Mật khẩu và VaiTrò!",
                                "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra MaSV có tồn tại trong bảng sinh_vien không
                    string checkSql = "SELECT COUNT(*) FROM sinh_vien WHERE MaSV=@MaSV";
                    MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@MaSV", maSV);
                    long exists = (long)checkCmd.ExecuteScalar();

                    if (exists == 0)
                    {
                        MessageBox.Show($"Mã sinh viên '{  maSV}' không tồn tại trong bảng sinh_vien!",
                                        "Lỗi khóa ngoại", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Thêm tài khoản (bao gồm VaiTro)
                    string sql = "INSERT INTO tai_khoan (MaSV, TenDangNhap, MatKhau, VaiTro) VALUES (@MaSV, @TenDangNhap, @MatKhau, @VaiTro)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaSV", maSV);
                    cmd.Parameters.AddWithValue("@TenDangNhap", username);
                    cmd.Parameters.AddWithValue("@MatKhau", password);
                    cmd.Parameters.AddWithValue("@VaiTro", role);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Thêm tài khoản thành công!", "Thành công",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadAccounts();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm tài khoản:\n" + ex.Message,
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ==================== SỬA ====================
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa!", "Chưa chọn",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string maSV = txtStudentId.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = (cbRole.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? cbRole.Text?.Trim();

            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã SV, Username và Mật khẩu để sửa!",
                                "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Vui lòng chọn VaiTrò để sửa!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE tai_khoan SET MatKhau=@MatKhau, VaiTro=@VaiTro WHERE MaSV=@MaSV AND TenDangNhap=@TenDangNhap";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaSV", maSV);
                    cmd.Parameters.AddWithValue("@TenDangNhap", username);
                    cmd.Parameters.AddWithValue("@MatKhau", password);
                    cmd.Parameters.AddWithValue("@VaiTro", role);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        MessageBox.Show("Cập nhật tài khoản thành công!", "Thành công",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Không tìm thấy tài khoản cần sửa!",
                                        "Không có kết quả", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadAccounts();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật tài khoản:\n" + ex.Message,
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ==================== XÓA ====================
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Chưa chọn",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string maSV = txtStudentId.Text.Trim();
            string username = txtUsername.Text.Trim();

            if (MessageBox.Show($"Bạn có chắc muốn xóa tài khoản '{username}' của sinh viên '{maSV}'?",
                                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM tai_khoan WHERE MaSV=@MaSV AND TenDangNhap=@TenDangNhap";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaSV", maSV);
                    cmd.Parameters.AddWithValue("@TenDangNhap", username);
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        MessageBox.Show("Đã xóa tài khoản thành công!", "Thành công",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Không tìm thấy tài khoản để xóa!",
                                        "Không có kết quả", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadAccounts();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa tài khoản:\n" + ex.Message,
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ==================== TÌM KIẾM ====================
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchBox.Text.Trim();
            LoadAccounts(keyword);
        }

        // ==================== CLICK TRONG DATAGRID ====================
        private void StudentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentsDataGrid.SelectedItem == null) return;
            DataRowView row = (DataRowView)StudentsDataGrid.SelectedItem;

            txtStudentId.Text = row["MaSV"].ToString();
            txtUsername.Text = row["Username"].ToString();
            txtPassword.Text = row["Password"].ToString();
            // Gán VaiTro nếu có
            if (row.Row.Table.Columns.Contains("VaiTro"))
            {
                var v = row["VaiTro"]?.ToString();
                // chọn item tương ứng trong ComboBox
                if (!string.IsNullOrEmpty(v))
                {
                    foreach (ComboBoxItem it in cbRole.Items)
                    {
                        if ((it.Content as string) == v)
                        {
                            cbRole.SelectedItem = it;
                            break;
                        }
                    }
                }
                else
                {
                    cbRole.SelectedIndex = -1;
                }
            }
        }

        // ==================== XÓA FORM ====================
        private void ClearFields()
        {
            txtStudentId.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            if (cbRole != null)
                cbRole.SelectedIndex = -1;
        }
    }
}
