using MySql.Data.MySqlClient;
using System;

namespace StudentManagement
{
    public class Control
    {
        private string connStr = "Server=localhost;Port=3306;Database=quanlysinhvien;Uid=root;Pwd=thang692004;";

        public void GetUsers()
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = "SELECT * FROM users"; // bảng của bạn
                MySqlCommand cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader.GetName(i)} | ");
                }
                Console.WriteLine();
                while (reader.Read())
                {
                    // In các giá trị của những cột còn lại
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetValue(i)} | ");
                    }

                    Console.WriteLine(); // Xuống dòng cho mỗi bản ghi
                }

            }
        }
    }
}
