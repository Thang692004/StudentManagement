using MySqlConnector;
using StudentManagement.Core;
using Microsoft.Extensions.Configuration;
using System;

namespace StudentManagement
{
    public class DeleteStudents : DatabaseServiceBase
    {
        public DeleteStudents(string? connectionString = null) : base(connectionString) { }

        public void DeleteStudentByMaSV(string maSV)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string query = "DELETE FROM sinh_vien WHERE MaSV = @MaSV";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSV", maSV);
                    cmd.ExecuteNonQuery(); // Thực thi lệnh DELETE
                }
            }
        }
    }
}