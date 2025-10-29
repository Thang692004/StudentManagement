// Các using statement giữ nguyên...
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.UI.Pages
{
    public partial class ClassPage : UserControl
    {
        // SỬA: Đảm bảo chuỗi kết nối dùng đúng mật khẩu (để trống)
        private string connectionString = "server=127.0.0.1;database=quanlysinhvien;uid=root;pwd=;";

        public ClassPage()
        {
            InitializeComponent();
            LoadDepartments();
            LoadClasses();
        }

        // Xử lý cuộn ngang ScrollViewer - Giữ nguyên
        private void svForm_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var sv = (ScrollViewer)sender;
            double newOffset = sv.HorizontalOffset - e.Delta;
            if (newOffset < 0) newOffset = 0;
            if (newOffset > sv.ScrollableWidth) newOffset = sv.ScrollableWidth;
            sv.ScrollToHorizontalOffset(newOffset);
            e.Handled = true;
        }

        // Load danh sách khoa vào ComboBox - Giữ nguyên
        private void LoadDepartments()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MaKhoa, TenKhoa FROM khoa";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            txtDepartID.Items.Clear();
                            while (reader.Read())
                            {
                                ComboBoxItem item = new ComboBoxItem
                                {
                                    Content = reader["MaKhoa"].ToString(),
                                    Tag = reader["TenKhoa"].ToString()
                                };
                                txtDepartID.Items.Add(item);
                            }
                            if (txtDepartID.Items.Count > 0)
                                txtDepartID.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Load danh sách lớp vào DataGrid
        private void LoadClasses()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // SỬA: Đổi tên bảng từ "lop" thành "classes"
                    string query = "SELECT l.class_code AS MaLop, l.class_name AS TenLop, l.faculty AS MaKhoa FROM classes l";
                    using (var adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        ClassDataGrid.ItemsSource = dt.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Thêm lớp
        private void Add_Class_Click(object sender, RoutedEventArgs e)
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
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // SỬA: Đổi tên bảng từ "lop" thành "classes" và các cột tương ứng
                    string query = "INSERT INTO classes (class_code, class_name, faculty) VALUES (@id, @name, @depart)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", classId);
                        cmd.Parameters.AddWithValue("@name", className);
                        cmd.Parameters.AddWithValue("@depart", departId);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Thêm lớp thành công!");
                LoadClasses();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Sửa lớp
        private void Update_Class_Click(object sender, RoutedEventArgs e)
        {
            string classId = txtClassId.Text.Trim();
            string className = txtNameClass.Text.Trim();
            if (txtDepartID.SelectedItem == null) return;
            string departId = ((ComboBoxItem)txtDepartID.SelectedItem).Content.ToString();

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // SỬA: Đổi tên bảng từ "lop" thành "classes" và các cột tương ứng
                    string query = "UPDATE classes SET class_name=@name, faculty=@depart WHERE class_code=@id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", classId);
                        cmd.Parameters.AddWithValue("@name", className);
                        cmd.Parameters.AddWithValue("@depart", departId);
                        int rows = cmd.ExecuteNonQuery();
                        MessageBox.Show(rows > 0 ? "Cập nhật thành công!" : "Không tìm thấy lớp.");
                    }
                }
                LoadClasses();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Xóa lớp
        private void Delete_Class_Click(object sender, RoutedEventArgs e)
        {
            string classId = txtClassId.Text.Trim();

            if (string.IsNullOrEmpty(classId))
            {
                MessageBox.Show("Vui lòng nhập mã lớp cần xóa!");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // SỬA: Đổi tên bảng từ "lop" thành "classes" và các cột tương ứng
                    string query = "DELETE FROM classes WHERE class_code=@id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", classId);
                        int rows = cmd.ExecuteNonQuery();
                        MessageBox.Show(rows > 0 ? "Xóa thành công!" : "Không tìm thấy lớp.");
                    }
                }
                LoadClasses();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Tìm kiếm
        // SỬA: Đổi tên hàm từ "Search_Class_Click" thành "Search_Click"
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string queryText = SearchBox.Text.Trim();
            if (string.IsNullOrEmpty(queryText)) { LoadClasses(); return; }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // SỬA: Đổi tên bảng từ "lop" thành "classes" và các cột tương ứng
                    string query = "SELECT class_code AS MaLop, class_name AS TenLop, faculty AS MaKhoa " +
                                     "FROM classes " +
                                     "WHERE class_code LIKE @text OR class_name LIKE @text";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@text", "%" + queryText + "%");
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            ClassDataGrid.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Chọn dòng trong DataGrid để tự động điền TextBox
        private void ClassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassDataGrid.SelectedItem == null) return;
            DataRowView row = ClassDataGrid.SelectedItem as DataRowView;
            if (row == null) return;

            txtClassId.Text = row["MaLop"].ToString();
            txtNameClass.Text = row["TenLop"].ToString();

            // SỬA: Sửa logic chọn Khoa trong ComboBox
            string maKhoaFromGrid = row["MaKhoa"].ToString();
            foreach (ComboBoxItem item in txtDepartID.Items)
            {
                // So sánh Mã Khoa với Mã Khoa
                if (item.Content.ToString() == maKhoaFromGrid)
                {
                    txtDepartID.SelectedItem = item;
                    break;
                }
            }
        }
    }
}