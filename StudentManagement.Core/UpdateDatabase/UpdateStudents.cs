
using MySql.Data.MySqlClient;
using StudentManagement.Core;
using System;

namespace StudentManagement
{
    public class UpdateStudents
    {
        private string connStr = "Server=localhost;Port=3306;Database=quanlysinhvien;Uid=root;Pwd=thang692004;";

        public void UpdateStudent(Student student)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                // Đã thêm cột TrangThai vào lệnh UPDATE
                string query = @"UPDATE sinh_vien SET 
                                 HoTen = @HoTen, 
                                 NgaySinh = @NgaySinh, 
                                 GioiTinh = @GioiTinh,
                                 Email = @Email, 
                                 SoDienThoai = @SoDienThoai,
                                 MaLop = @MaLop,
                                 MaKhoa = @MaKhoa,
                                 TrangThai = @TrangThai -- ĐÃ THÊM DÒNG NÀY
                             WHERE MaSV = @MaSV";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    // Sử dụng tham số (@...)
                    cmd.Parameters.AddWithValue("@HoTen", student.HoTen);
                    cmd.Parameters.AddWithValue("@NgaySinh", student.NgaySinh);
                    cmd.Parameters.AddWithValue("@GioiTinh", student.GioiTinh);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@SoDienThoai", student.SoDienThoai);
                    cmd.Parameters.AddWithValue("@MaLop", student.MaLop);
                    cmd.Parameters.AddWithValue("@MaKhoa", student.MaKhoa);

                    // Thêm tham số TrangThai
                    cmd.Parameters.AddWithValue("@TrangThai", student.Check_TrangThai);

                    cmd.Parameters.AddWithValue("@MaSV", student.MaSV); // Điều kiện WHERE

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}