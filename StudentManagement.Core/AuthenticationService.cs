using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StudentManagement.Core.ReadDatabase;

namespace StudentManagement.Core
{
    public class AuthenticationService
    {
        private readonly ReadUsers _readUsers;

        public AuthenticationService()
        {
            _readUsers = new ReadUsers();
        }

        public string? AuthenticateUser(string TenDangNhap, string MatKhau)
        {
            if (string.IsNullOrEmpty(TenDangNhap) || string.IsNullOrEmpty(MatKhau))
            {
                return null;
            }

            return _readUsers.GetUserRoleByCredentials(TenDangNhap, MatKhau);
        }
    }
}
