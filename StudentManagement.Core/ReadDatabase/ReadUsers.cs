using MySqlConnector;
using System;
using System.Windows;

namespace StudentManagement.Core.ReadDatabase
{
    public class ReadUsers
    {
        private string connectionString = "Server=127.0.0.1;Database=quanlysinhvien;Uid=root;Pwd=thang692004;";

        /// <summary>
        /// Kiểm tra tài khoản đăng nhập và trả về vai trò (Admin / Student) nếu hợp lệ
        /// </summary>
        public string? GetUserRoleByCredentials(string username, string password)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Đổi tên bảng và cột để khớp với SQL thật
                    string query = @"
                        SELECT VaiTro 
                        FROM tai_khoan 
                        WHERE TenDangNhap = @username 
                          AND MatKhau = @password 
                          AND TrangThai = 1
                        LIMIT 1;
                    ";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        object result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Bạn có thể log lỗi chi tiết hơn ở đây
                MessageBox.Show("Lỗi khi đọc dữ liệu tài khoản: " + ex.Message);
                return null;
            }
        }
    }
}
