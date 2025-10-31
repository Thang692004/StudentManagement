using MySql.Data.MySqlClient;
using StudentManagement.Core;
using System;

namespace StudentManagement
{
    public class InsertStudents : DatabaseServiceBase
    {
        public void AddStudent(Student student)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                // ✅ SỬA ĐỔI: Đã thêm cột 'DiaChi' vào danh sách cột và VALUES
                string query = @"INSERT INTO sinh_vien (MaSV, HoTen, NgaySinh, GioiTinh, Email, SoDienThoai, MaLop, MaKhoa, DiaChi, TrangThai) 
                                 VALUES (@MaSV, @HoTen, @NgaySinh, @GioiTinh, @Email, @SoDienThoai, @MaLop, @MaKhoa, @DiaChi, @TrangThai)";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    // Thêm các tham số cho lệnh INSERT
                    cmd.Parameters.AddWithValue("@MaSV", student.MaSV);
                    cmd.Parameters.AddWithValue("@HoTen", student.HoTen);

                    // NgaySinh: Đảm bảo được truyền đi
                    cmd.Parameters.Add("@NgaySinh", MySqlDbType.Date).Value = student.NgaySinh;

                    cmd.Parameters.AddWithValue("@GioiTinh", student.GioiTinh);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@SoDienThoai", student.SoDienThoai);
                    cmd.Parameters.AddWithValue("@MaLop", student.MaLop);
                    cmd.Parameters.AddWithValue("@MaKhoa", student.MaKhoa);

                    // ✅ THÊM THAM SỐ DIACHI
                    cmd.Parameters.AddWithValue("@DiaChi", student.DiaChi);

                    // TrangThai: Đảm bảo sử dụng Check_TrangThai (bool) và nó sẽ ánh xạ thành 0/1 (tinyint)
                    cmd.Parameters.AddWithValue("@TrangThai", student.Check_TrangThai);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}