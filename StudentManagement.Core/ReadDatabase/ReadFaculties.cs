using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using StudentManagement.Core;
using ZstdSharp.Unsafe;
namespace StudentManagement
{
    public class ReadFaculties
    {

        private string connStr = "Server=localhost;Port=3306;Database=quanlysinhvien;Uid=root;Pwd=thang692004;";

        public List<Faculty> GetFaculties()
        {
            var faculties = new List<Faculty>();
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = @"SELECT MaKhoa, TenKhoa FROM khoa  ";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Faculty f = new Faculty();
                        f.FacultyCode = reader.GetString("MaKhoa");
                        f.FacultyName = reader.GetString("TenKhoa");

                        faculties.Add(f);
                    }
                }

            }
            return faculties;
        }
    }
}
