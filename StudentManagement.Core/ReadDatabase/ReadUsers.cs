using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySqlConnector;

namespace StudentManagement.Core.ReadDatabase
{
    public class ReadUsers : DatabaseServiceBase
    {
        public ReadUsers(string? connectionString = null) : base(connectionString) { }

        public string? GetUserRoleByCredentials(string username, string password)
        {
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT VaiTro FROM tai_khoan WHERE TenDangNhap = @username AND MatKhau = @password";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        object result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
