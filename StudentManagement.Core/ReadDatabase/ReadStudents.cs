using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using StudentManagement.Core;

namespace StudentManagement
{
    public class ReadStudents
    {
        private string connStr = "Server=localhost;Port=3306;Database=quanlysinhvien;Uid=root;Pwd=thang692004;";

        public List<Student> GetStudents()
        {
            var students = new List<Student>();

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // 1. Cập nhật câu lệnh SQL để JOIN các bảng mới
                string query = @"
                    SELECT 
                        sv.MaSV,
                        sv.HoTen,
                        sv.NgaySinh,
                        sv.GioiTinh,
                        sv.DiaChi,
                        sv.Email,
                        sv.SoDienThoai,
                        sv.MaLop,
                        l.TenLop,
                        sv.MaKhoa,
                        k.TenKhoa,
                        sv.TrangThai
                    FROM 
                        sinh_vien sv
                    LEFT JOIN 
                        lop l ON sv.MaLop = l.MaLop
                    LEFT JOIN 
                        khoa k ON sv.MaKhoa = k.MaKhoa;
                ";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    // 2. Lấy vị trí các cột theo tên mới
                    int ordMaSV = reader.GetOrdinal("MaSV");
                    int ordHoTen = reader.GetOrdinal("HoTen");
                    int ordNgaySinh = reader.GetOrdinal("NgaySinh");
                    int ordGioiTinh = reader.GetOrdinal("GioiTinh");
                    int ordDiaChi = reader.GetOrdinal("DiaChi");
                    int ordEmail = reader.GetOrdinal("Email");
                    int ordSoDienThoai = reader.GetOrdinal("SoDienThoai");
                    int ordMaLop = reader.GetOrdinal("MaLop");
                    int ordTenLop = reader.GetOrdinal("TenLop");
                    int ordMaKhoa = reader.GetOrdinal("MaKhoa");
                    int ordTenKhoa = reader.GetOrdinal("TenKhoa");
                    int ordTrangThai = reader.GetOrdinal("TrangThai");

                    while (reader.Read())
                    {
                        var s = new Student();

                        // 3. Đọc dữ liệu và gán vào đối tượng Student đã cập nhật
                        s.MaSV = !reader.IsDBNull(ordMaSV) ? reader.GetString(ordMaSV) : string.Empty;
                        s.HoTen = !reader.IsDBNull(ordHoTen) ? reader.GetString(ordHoTen) : string.Empty;
                        s.NgaySinh = !reader.IsDBNull(ordNgaySinh) ? reader.GetDateTime(ordNgaySinh) : (DateTime?)null;
                        s.GioiTinh = !reader.IsDBNull(ordGioiTinh) ? reader.GetString(ordGioiTinh) : string.Empty;
                        s.DiaChi = !reader.IsDBNull(ordDiaChi) ? reader.GetString(ordDiaChi) : string.Empty;
                        s.Email = !reader.IsDBNull(ordEmail) ? reader.GetString(ordEmail) : string.Empty;
                        s.SoDienThoai = !reader.IsDBNull(ordSoDienThoai) ? reader.GetString(ordSoDienThoai) : string.Empty;
                        s.MaLop = !reader.IsDBNull(ordMaLop) ? reader.GetString(ordMaLop) : string.Empty;
                        s.TenLop = !reader.IsDBNull(ordTenLop) ? reader.GetString(ordTenLop) : string.Empty;
                        s.MaKhoa = !reader.IsDBNull(ordMaKhoa) ? reader.GetString(ordMaKhoa) : string.Empty;
                        s.TenKhoa = !reader.IsDBNull(ordTenKhoa) ? reader.GetString(ordTenKhoa) : string.Empty;
                        s.Check_TrangThai = !reader.IsDBNull(ordTrangThai) && reader.GetBoolean(ordTrangThai);
                        if (s.Check_TrangThai == true)
                        {
                            s.TrangThai = "Đang học";
                        }
                        else
                        {
                            s.TrangThai = "Nghỉ học";
                        }
                        students.Add(s);
                    }
                }
            }

            return students;
        }
    }
}