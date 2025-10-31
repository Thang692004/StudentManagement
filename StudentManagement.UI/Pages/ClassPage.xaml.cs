using MySqlConnector;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace StudentManagement.UI.Pages
{
    public partial class ClassPage : UserControl
    {
        private readonly string connectionString;

        public ClassPage()
        {
            InitializeComponent();

            // Nạp cấu hình từ appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();
            connectionString = config.GetConnectionString("Default")
                ?? throw new Exception("Không tìm thấy chuỗi kết nối 'Default' trong appsettings.json.");

            // Gọi các hàm tải dữ liệu
            _ = LoadDepartmentsAsync();
            _ = LoadClassesAsync();
        }

        // Cuộn ngang trong ScrollViewer
        private void svForm_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer sv)
            {
                double newOffset = sv.HorizontalOffset - e.Delta;
                sv.ScrollToHorizontalOffset(Math.Clamp(newOffset, 0, sv.ScrollableWidth));
                e.Handled = true;
            }
        }


        private async Task LoadDepartmentsAsync()
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                const string query = "SELECT MaKhoa, TenKhoa FROM khoa";
                using var cmd = new MySqlCommand(query, conn);
                using var reader = await cmd.ExecuteReaderAsync();

                txtDepartID.Items.Clear();
                while (await reader.ReadAsync())
                {
                    var item = new ComboBoxItem
                    {
                        Content = reader["MaKhoa"].ToString(),
                        Tag = reader["TenKhoa"].ToString()
                    };
                    txtDepartID.Items.Add(item);
                }

                if (txtDepartID.Items.Count > 0)
                    txtDepartID.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách khoa: {ex.Message}");
            }
        }

        private async Task LoadClassesAsync()
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                const string query = """
                    SELECT l.MaLop, l.TenLop, k.TenKhoa
                    FROM lop l
                    INNER JOIN khoa k ON l.MaKhoa = k.MaKhoa;
                """;

                using var adapter = new MySqlDataAdapter(query, conn);
                var dt = new DataTable();
                adapter.Fill(dt);
                ClassDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách lớp: {ex.Message}");
            }
        }


        private async void Add_Class_Click(object sender, RoutedEventArgs e)
        {
            string classId = txtClassId.Text.Trim();
            string className = txtNameClass.Text.Trim();
            if (txtDepartID.SelectedItem == null) return;
            string departId = ((ComboBoxItem)txtDepartID.SelectedItem).Content.ToString();

            if (string.IsNullOrEmpty(classId) || string.IsNullOrEmpty(className))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                const string query = "INSERT INTO lop (MaLop, TenLop, MaKhoa) VALUES (@id, @name, @depart)";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", classId);
                cmd.Parameters.AddWithValue("@name", className);
                cmd.Parameters.AddWithValue("@depart", departId);
                await cmd.ExecuteNonQueryAsync();

                MessageBox.Show("Thêm lớp thành công!");
                await LoadClassesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm lớp: {ex.Message}");
            }
        }

        private async void Update_Class_Click(object sender, RoutedEventArgs e)
        {
            string classId = txtClassId.Text.Trim();
            string className = txtNameClass.Text.Trim();
            if (txtDepartID.SelectedItem == null) return;
            string departId = ((ComboBoxItem)txtDepartID.SelectedItem).Content.ToString();

            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                const string query = "UPDATE lop SET TenLop=@name, MaKhoa=@depart WHERE MaLop=@id";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", classId);
                cmd.Parameters.AddWithValue("@name", className);
                cmd.Parameters.AddWithValue("@depart", departId);
                int rows = await cmd.ExecuteNonQueryAsync();

                MessageBox.Show(rows > 0 ? "Cập nhật thành công!" : "Không tìm thấy lớp.");
                await LoadClassesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật lớp: {ex.Message}");
            }
        }

        private async void Delete_Class_Click(object sender, RoutedEventArgs e)
        {
            string classId = txtClassId.Text.Trim();
            if (string.IsNullOrEmpty(classId))
            {
                MessageBox.Show("Vui lòng nhập mã lớp cần xóa!");
                return;
            }

            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                // Kiểm tra xem lớp còn sinh viên không
                const string checkQuery = "SELECT COUNT(*) FROM sinh_vien WHERE MaLop=@id";
                using var checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@id", classId);
                long studentCount = (long)await checkCmd.ExecuteScalarAsync();

                if (studentCount > 0)
                {
                    MessageBox.Show($"Lớp này còn {studentCount} sinh viên. Không thể xóa lớp còn sinh viên.",
                                    "Lỗi xóa lớp", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Xác nhận xóa
                if (MessageBox.Show($"Bạn có chắc muốn xóa lớp {classId}?", "Xác nhận", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    return;

                // Xóa lớp
                const string deleteQuery = "DELETE FROM lop WHERE MaLop=@id";
                using var deleteCmd = new MySqlCommand(deleteQuery, conn);
                deleteCmd.Parameters.AddWithValue("@id", classId);
                int rows = await deleteCmd.ExecuteNonQueryAsync();

                MessageBox.Show(rows > 0 ? "Xóa lớp thành công!" : "Không tìm thấy lớp.");
                await LoadClassesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa lớp: {ex.Message}");
            }
        }



        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string queryText = SearchBox.Text.Trim();
            if (string.IsNullOrEmpty(queryText))
            {
                await LoadClassesAsync();
                return;
            }

            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                const string query = """
                    SELECT l.MaLop, l.TenLop, k.TenKhoa
                    FROM lop l
                    INNER JOIN khoa k ON l.MaKhoa = k.MaKhoa
                    WHERE l.MaLop LIKE @text OR l.TenLop LIKE @text;
                """;

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@text", $"%{queryText}%");

                using var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                ClassDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}");
            }
        }


        private void ClassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassDataGrid.SelectedItem is not DataRowView row) return;

            txtClassId.Text = row["MaLop"].ToString();
            txtNameClass.Text = row["TenLop"].ToString();

            foreach (ComboBoxItem item in txtDepartID.Items)
            {
                if (item.Tag?.ToString() == row["TenKhoa"].ToString())
                {
                    txtDepartID.SelectedItem = item;
                    break;
                }
            }
        }
    }
}
