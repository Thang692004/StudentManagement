using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace StudentManagement.UI.Pages.UserPages
{
    /// <summary>
    /// Interaction logic for UserClassesPage.xaml
    /// </summary>
    public partial class UserDepartmentsPage : UserControl
    {
        private readonly string connectionString;
        private readonly string? _maKhoaFilter;

        public UserDepartmentsPage(string? maKhoaFilter = null)
        {
            InitializeComponent();
            _maKhoaFilter = maKhoaFilter;
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();
            connectionString = config.GetConnectionString("Default")
                ?? throw new Exception("Không tìm thấy chuỗi kết nối 'Default' trong appsettings.json.");

            // Gọi các hàm tải dữ liệu
            _ = LoadDepartmentsAsync();
        }
        private async Task LoadDepartmentsAsync()
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                string query;
                if (!string.IsNullOrWhiteSpace(_maKhoaFilter))
                {
                    query = @"SELECT MaKhoa, TenKhoa FROM khoa WHERE MaKhoa = @maKhoa;";
                    using var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maKhoa", _maKhoaFilter);
                    using var adapter = new MySqlDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    DepartmentDataGrid.ItemsSource = dt.DefaultView;
                }
                else
                {
                    query = @"SELECT MaKhoa, TenKhoa FROM khoa;";
                    using var adapter = new MySqlDataAdapter(query, conn);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    DepartmentDataGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách khoa: {ex.Message}");
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchBox.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                await LoadDepartmentsAsync();
                return;
            }

            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                const string query = @"
            SELECT MaKhoa, TenKhoa
            FROM khoa
            WHERE MaKhoa LIKE @keyword OR TenKhoa LIKE @keyword;
        ";

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");

                using var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                DepartmentDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}");
            }
        }

        private void DepartmentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentDataGrid.SelectedItem is not DataRowView row) return;
            string maKhoa = row["MaKhoa"].ToString();
            string tenKhoa = row["TenKhoa"].ToString();

        }

    }
}
