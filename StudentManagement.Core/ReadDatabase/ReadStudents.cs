using System;
using System.Collections.Generic;
using MySqlConnector;
using StudentManagement.Core;

namespace StudentManagement
{
    public class ReadStudents : DatabaseServiceBase
    {
        // Cho phép truyền hoặc không truyền connection string
        public ReadStudents(string? connectionString = null) : base(connectionString) { }

        public List<Student> GetStudents()
        {
            var students = new List<Student>();

            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh,
                        sv.DiaChi, sv.Email, sv.SoDienThoai,
                        sv.MaLop, l.TenLop, sv.MaKhoa, k.TenKhoa, sv.TrangThai
                    FROM sinh_vien sv
                    LEFT JOIN lop l ON sv.MaLop = l.MaLop
                    LEFT JOIN khoa k ON sv.MaKhoa = k.MaKhoa;
                ";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var s = new Student
                        {
                            MaSV = reader["MaSV"]?.ToString() ?? "",
                            HoTen = reader["HoTen"]?.ToString() ?? "",
                            NgaySinh = reader["NgaySinh"] as DateTime?,
                            GioiTinh = reader["GioiTinh"]?.ToString() ?? "",
                            DiaChi = reader["DiaChi"]?.ToString() ?? "",
                            Email = reader["Email"]?.ToString() ?? "",
                            SoDienThoai = reader["SoDienThoai"]?.ToString() ?? "",
                            MaLop = reader["MaLop"]?.ToString() ?? "",
                            TenLop = reader["TenLop"]?.ToString() ?? "",
                            MaKhoa = reader["MaKhoa"]?.ToString() ?? "",
                            TenKhoa = reader["TenKhoa"]?.ToString() ?? ""
                        };

                        bool active = reader["TrangThai"] != DBNull.Value && Convert.ToBoolean(reader["TrangThai"]);
                        s.TrangThai = active ? "Đang học" : "Nghỉ học";
                        s.Check_TrangThai = active;

                        students.Add(s);
                    }
                }
            }

            return students;
        }

        public Student? GetStudentByMaSV(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) return null;

            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh,
                        sv.DiaChi, sv.Email, sv.SoDienThoai,
                        sv.MaLop, l.TenLop, sv.MaKhoa, k.TenKhoa, sv.TrangThai
                    FROM sinh_vien sv
                    LEFT JOIN lop l ON sv.MaLop = l.MaLop
                    LEFT JOIN khoa k ON sv.MaKhoa = k.MaKhoa
                    WHERE sv.MaSV = @maSV;
                ";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maSV", maSV);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var s = new Student
                            {
                                MaSV = reader["MaSV"]?.ToString() ?? "",
                                HoTen = reader["HoTen"]?.ToString() ?? "",
                                NgaySinh = reader["NgaySinh"] as DateTime?,
                                GioiTinh = reader["GioiTinh"]?.ToString() ?? "",
                                DiaChi = reader["DiaChi"]?.ToString() ?? "",
                                Email = reader["Email"]?.ToString() ?? "",
                                SoDienThoai = reader["SoDienThoai"]?.ToString() ?? "",
                                MaLop = reader["MaLop"]?.ToString() ?? "",
                                TenLop = reader["TenLop"]?.ToString() ?? "",
                                MaKhoa = reader["MaKhoa"]?.ToString() ?? "",
                                TenKhoa = reader["TenKhoa"]?.ToString() ?? ""
                            };

                            bool active = reader["TrangThai"] != DBNull.Value && Convert.ToBoolean(reader["TrangThai"]);
                            s.TrangThai = active ? "Đang học" : "Nghỉ học";
                            s.Check_TrangThai = active;

                            return s;
                        }
                    }
                }
            }

            return null;
        }
    }
}
