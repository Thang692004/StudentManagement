// File: DeleteStudents.cs (Bạn cần tự tạo file này)
using MySql.Data.MySqlClient;
using StudentManagement.Core;
using System;

namespace StudentManagement
{
    public class DeleteStudents
    {
        private string connStr = "Server=localhost;Port=3306;Database=quanlysinhvien;Uid=root;Pwd=thang692004;";

        public void DeleteStudentByMaSV(string maSV)
        {
            using (var conn = new MySqlConnection(connStr))
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