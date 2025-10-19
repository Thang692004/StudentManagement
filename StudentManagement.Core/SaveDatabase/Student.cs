using System;

namespace StudentManagement.Core
{ 
    public class Student
	    {
            public string MaSV { get; set; }
            public string HoTen { get; set; }
            public DateTime NgaySinh { get; set; }
            public string GioiTinh { get; set; }
            public string DiaChi { get; set; }
            public string Email { get; set; }
            public string SoDienThoai { get; set; }
            public string MaLop { get; set; }
            public string TenLop { get; set; } // Lấy từ bảng 'lop'
            public string MaKhoa { get; set; }
            public string TenKhoa { get; set; } // Lấy từ bảng 'khoa'
            public bool TrangThai { get; set; } // Chuyển từ tinyint sang bool

            public string DateOfBirthFormatted => NgaySinh.ToString("dd/mm/yyyy"); // 
	    
	    }
}
