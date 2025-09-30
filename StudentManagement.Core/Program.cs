using System;

namespace StudentManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Tạo đối tượng Control và gọi hàm GetUsers
            Control control = new Control();
            control.GetUsers();

            // Dừng màn hình để xem kết quả
            Console.WriteLine("Nhấn Enter để thoát...");
            Console.ReadLine();
        }
    }
}
