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
        private readonly ReadDatabase.ReadUsers _readUsers;

        public AuthenticationService()
        {
            _readUsers = new ReadDatabase.ReadUsers();
        }

        /// <summary>
        /// Authenticate and return role + MaSV (if available).
        /// </summary>
        public AuthenticationResult? AuthenticateUser(string TenDangNhap, string MatKhau)
        {
            if (string.IsNullOrEmpty(TenDangNhap) || string.IsNullOrEmpty(MatKhau))
            {
                return null;
            }

            try
            {
                // ReadUsers will return role and MaSV for the account (or null if not found)
                var (role, maSV) = _readUsers.GetRoleAndMaSVByCredentials(TenDangNhap, MatKhau);
                if (role == null) return null;

                return new AuthenticationResult
                {
                    Role = role,
                    MaSV = maSV,
                    Username = TenDangNhap
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
