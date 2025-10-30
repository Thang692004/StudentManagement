using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using StudentManagement.Core;
using ZstdSharp.Unsafe;

namespace StudentManagement
{
	public class ReadClasses : DatabaseServiceBase
    {
		public List<Class> GetClasses()
		{
			var classes = new List<Class>();
			using (var conn = new MySqlConnection(ConnectionString) )
			{
				conn.Open();
				string query = @"SELECT c.MaLop,c.TenLop,c.MaKhoa, k.TenKhoa FROM lop c
							   LEFT JOIN 
									khoa k ON c.MaKhoa = k.MaKhoa;  ";

				using (var cmd = new MySqlCommand(query, conn))
				using (var reader = cmd.ExecuteReader())
				{
					while(reader.Read())
					{
						Class c = new Class();

						c.ClassCode = reader.GetString("MaLop");
						c.ClassName = reader.GetString("TenLop");
						c.FacultyCode = reader.GetString("MaKhoa");
						c.FacultyName = reader.GetString("TenKhoa");

						classes.Add(c);
					}
				}

			}
			return classes;
		}
	}
}
