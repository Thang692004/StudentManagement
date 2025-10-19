using MySql.Data.MySqlClient;
using StudentManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                // CHÚ Ý: Đảm bảo tên cột và tên bảng trong SQL khớp với DB của bạn
                string query = @"UPDATE sinh_vien SET 
                                HoTen = @HoTen, 
                                NgaySinh = @NgaySinh, 
                                GioiTinh = @GioiTinh,
                                Email = @Email, 
                                SoDienThoai = @SoDienThoai,
                                MaLop = @MaLop,
                                MaKhoa = @MaKhoa
                            WHERE MaSV = @MaSV"; // Cập nhật dựa trên Mã Sinh Viên

                using (var cmd = new MySqlCommand(query, conn))
                {
                    // Sử dụng tham số (@...) để bảo mật và tránh lỗi SQL injection
                    cmd.Parameters.AddWithValue("@HoTen", student.HoTen);
                    cmd.Parameters.AddWithValue("@NgaySinh", student.NgaySinh);
                    cmd.Parameters.AddWithValue("@GioiTinh", student.GioiTinh);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@SoDienThoai", student.SoDienThoai);
                    cmd.Parameters.AddWithValue("@MaLop", student.MaLop);
                    cmd.Parameters.AddWithValue("@MaKhoa", student.MaKhoa);
                    cmd.Parameters.AddWithValue("@MaSV", student.MaSV); // Điều kiện WHERE

                    cmd.ExecuteNonQuery(); // Thực thi lệnh UPDATE
                }
            }
        }
    }
}
