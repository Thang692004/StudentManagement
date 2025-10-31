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
    public partial class UserClassesPage : UserControl
    {
        private readonly string connectionString;
        public UserClassesPage()
        {
            InitializeComponent();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();
            connectionString = config.GetConnectionString("Default")
                ?? throw new Exception("Không tìm thấy chuỗi kết nối 'Default' trong appsettings.json.");

            // Gọi các hàm tải dữ liệu
            _ = LoadClassesAsync();
        }
        private async Task LoadClassesAsync()
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                const string query = @"
                    SELECT l.MaLop, l.TenLop, k.TenKhoa
                    FROM lop l
                    INNER JOIN khoa k ON l.MaKhoa = k.MaKhoa;
                ";

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
        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchBox.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                await LoadClassesAsync();
                return;
            }

            try
            {
                using var conn = new MySqlConnection(connectionString);
                await conn.OpenAsync();

                const string query = @"
                    SELECT l.MaLop, l.TenLop, k.TenKhoa
                    FROM lop l
                    INNER JOIN khoa k ON l.MaKhoa = k.MaKhoa
                    WHERE l.MaLop LIKE @keyword OR l.TenLop LIKE @keyword;
                ";

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");

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
            string maLop = row["MaLop"].ToString();
            string tenLop = row["TenLop"].ToString();
            string tenKhoa = row["TenKhoa"].ToString();

        }

    }
}
