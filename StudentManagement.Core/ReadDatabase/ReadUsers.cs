using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySqlConnector;

namespace StudentManagement.Core.ReadDatabase
{
    public class ReadUsers
    {
        private string connectionString = "Server=127.0.0.1;Database=quanlysinhvien;Uid=root;Pwd=;";

        public string? GetUserRoleByCredentials(string username, string password)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT role FROM users WHERE username = @username AND password = @password";
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
