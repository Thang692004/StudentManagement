// Các using statement
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
        private string connectionString = "server=127.0.0.1;database=quanlysinhvien;uid=root;pwd=;";

        public ClassPage()
        {
            InitializeComponent();
            LoadDepartments();
            LoadClasses();
        }

        // ===== Scroll ngang =====
        private void svForm_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var sv = (ScrollViewer)sender;
            double newOffset = sv.HorizontalOffset - e.Delta;
            if (newOffset < 0) newOffset = 0;
            if (newOffset > sv.ScrollableWidth) newOffset = sv.ScrollableWidth;
            sv.ScrollToHorizontalOffset(newOffset);
            e.Handled = true;
        }

        // ===== Nạp danh sách Khoa vào ComboBox =====
        private void LoadDepartments()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MaKhoa, TenKhoa FROM khoa";
                    using (var cmd = new MySqlCommand(query, conn))
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải Khoa: " + ex.Message);
            }
        }

        // ===== Nạp danh sách Lớp vào DataGrid =====
        private void LoadClasses()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MaLop, TenLop, MaKhoa FROM lop";
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
                MessageBox.Show("Lỗi tải danh sách lớp: " + ex.Message);
            }
        }

        // ====== Nút Thêm ======
        private void Add_Class_Click(object sender, RoutedEventArgs e)
        {
            string classId = txtClassId.Text.Trim();
            string className = txtNameClass.Text.Trim();
            if (txtDepartID.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Khoa!");
                return;
            }
            string departId = ((ComboBoxItem)txtDepartID.SelectedItem).Content.ToString();

            if (string.IsNullOrEmpty(classId) || string.IsNullOrEmpty(className))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin lớp!");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO lop (MaLop, TenLop, MaKhoa) VALUES (@id, @name, @depart)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", classId);
                        cmd.Parameters.AddWithValue("@name", className);
                        cmd.Parameters.AddWithValue("@depart", departId);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("✅ Thêm lớp thành công!");
                ClearForm();
                LoadClasses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi thêm lớp: " + ex.Message);
            }
        }

        // ====== Nút Sửa ======
        private void Update_Class_Click(object sender, RoutedEventArgs e)
        {
            string classId = txtClassId.Text.Trim();
            string className = txtNameClass.Text.Trim();
            if (txtDepartID.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Khoa!");
                return;
            }
            string departId = ((ComboBoxItem)txtDepartID.SelectedItem).Content.ToString();

            if (string.IsNullOrEmpty(classId))
            {
                MessageBox.Show("Vui lòng chọn lớp cần sửa!");
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE lop SET TenLop=@name, MaKhoa=@depart WHERE MaLop=@id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", classId);
                        cmd.Parameters.AddWithValue("@name", className);
                        cmd.Parameters.AddWithValue("@depart", departId);
                        int rows = cmd.ExecuteNonQuery();
                        MessageBox.Show(rows > 0 ? "✅ Cập nhật thành công!" : "❌ Không tìm thấy lớp cần sửa.");
                    }
                }
                ClearForm();
                LoadClasses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi cập nhật: " + ex.Message);
            }
        }

        // ====== Nút Xóa ======
        private void Delete_Class_Click(object sender, RoutedEventArgs e)
        {
            string classId = txtClassId.Text.Trim();
            if (string.IsNullOrEmpty(classId))
            {
                MessageBox.Show("Vui lòng nhập mã lớp cần xóa!");
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa lớp {classId}?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM lop WHERE MaLop=@id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", classId);
                        int rows = cmd.ExecuteNonQuery();
                        MessageBox.Show(rows > 0 ? "✅ Xóa thành công!" : "❌ Không tìm thấy lớp cần xóa.");
                    }
                }
                ClearForm();
                LoadClasses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi xóa lớp: " + ex.Message);
            }
        }

        // ====== Nút Tìm kiếm ======
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string queryText = SearchBox.Text.Trim();
            if (string.IsNullOrEmpty(queryText))
            {
                LoadClasses();
                return;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MaLop, TenLop, MaKhoa FROM lop " +
                                   "WHERE MaLop LIKE @text OR TenLop LIKE @text";
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
                MessageBox.Show("❌ Lỗi tìm kiếm: " + ex.Message);
            }
        }

        // ===== Khi chọn dòng trong DataGrid =====
        private void ClassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassDataGrid.SelectedItem == null) return;
            DataRowView row = ClassDataGrid.SelectedItem as DataRowView;
            if (row == null) return;

            txtClassId.Text = row["MaLop"].ToString();
            txtNameClass.Text = row["TenLop"].ToString();
            string maKhoa = row["MaKhoa"].ToString();

            foreach (ComboBoxItem item in txtDepartID.Items)
            {
                if (item.Content.ToString() == maKhoa)
                {
                    txtDepartID.SelectedItem = item;
                    break;
                }
            }
        }

        // ===== Hàm tiện ích =====
        private void ClearForm()
        {
            txtClassId.Clear();
            txtNameClass.Clear();
            SearchBox.Clear();
            if (txtDepartID.Items.Count > 0)
                txtDepartID.SelectedIndex = 0;
        }
    }
}
