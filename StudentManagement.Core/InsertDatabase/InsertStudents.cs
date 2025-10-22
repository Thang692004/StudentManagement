
using MySql.Data.MySqlClient;
using StudentManagement.Core;
using System;

namespace StudentManagement
{
    public class InsertStudents
    {
        private string connStr = "Server=localhost;Port=3306;Database=quanlysinhvien;Uid=root;Pwd=thang692004;";

        public void AddStudent(Student student)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                // Đã thêm cột TrangThai vào danh sách cột và VALUES
                string query = @"INSERT INTO sinh_vien (MaSV, HoTen, NgaySinh, GioiTinh, Email, SoDienThoai, MaLop, MaKhoa, TrangThai) 
                                 VALUES (@MaSV, @HoTen, @NgaySinh, @GioiTinh, @Email, @SoDienThoai, @MaLop, @MaKhoa, @TrangThai)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    // Thêm các tham số cho lệnh INSERT
                    cmd.Parameters.AddWithValue("@MaSV", student.MaSV);
                    cmd.Parameters.AddWithValue("@HoTen", student.HoTen);
                    cmd.Parameters.AddWithValue("@NgaySinh", student.NgaySinh);
                    cmd.Parameters.AddWithValue("@GioiTinh", student.GioiTinh);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@SoDienThoai", student.SoDienThoai);
                    cmd.Parameters.AddWithValue("@MaLop", student.MaLop);
                    cmd.Parameters.AddWithValue("@MaKhoa", student.MaKhoa);
                    cmd.Parameters.AddWithValue("@TrangThai", student.Check_TrangThai);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}