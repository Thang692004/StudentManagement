using MySql.Data.MySqlClient;
using StudentManagement.Core;
using System;
using System.Data; // Cần thiết cho MySqlDbType

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

                // ✅ SỬA ĐỔI: Đã thêm cột 'DiaChi' vào lệnh UPDATE
                string query = @"UPDATE sinh_vien SET 
                                 HoTen = @HoTen, 
                                 NgaySinh = @NgaySinh, 
                                 GioiTinh = @GioiTinh,
                                 DiaChi = @DiaChi,        -- ✅ THÊM DÒNG NÀY ĐỂ CẬP NHẬT ĐỊA CHỈ
                                 Email = @Email, 
                                 SoDienThoai = @SoDienThoai,
                                 MaLop = @MaLop,
                                 MaKhoa = @MaKhoa,
                                 TrangThai = @TrangThai
                                 WHERE MaSV = @MaSV";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    // 1. NGÀY SINH: Dùng MySqlParameter để chỉ định rõ kiểu DATE
                    cmd.Parameters.Add("@NgaySinh", MySqlDbType.Date).Value = student.NgaySinh;

                    // 2. ĐỊA CHỈ: Thêm tham số DiaChi
                    cmd.Parameters.AddWithValue("@DiaChi", student.DiaChi);

                    // 3. Các tham số khác (sử dụng AddWithValue vì là kiểu chuỗi/bool)
                    cmd.Parameters.AddWithValue("@HoTen", student.HoTen);
                    cmd.Parameters.AddWithValue("@GioiTinh", student.GioiTinh);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@SoDienThoai", student.SoDienThoai);
                    cmd.Parameters.AddWithValue("@MaLop", student.MaLop);
                    cmd.Parameters.AddWithValue("@MaKhoa", student.MaKhoa);
                    cmd.Parameters.AddWithValue("@TrangThai", student.Check_TrangThai);

                    // Điều kiện WHERE (bắt buộc phải có)
                    cmd.Parameters.AddWithValue("@MaSV", student.MaSV);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}