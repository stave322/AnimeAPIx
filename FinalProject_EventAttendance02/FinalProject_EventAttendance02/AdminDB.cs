using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_EventAttendance02
{
    public static class AdminDB
    {
        
        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower(); 
        }

        public static bool Login(string username, string password)
        {
            try
            {
                using var conn = DbHelper.GetConnection();
                const string sql = @"
                    SELECT COUNT(*) FROM admins
                    WHERE LOWER(username) = LOWER(@user)
                      AND password_hash   = @hash";

                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@user", username.Trim());
                cmd.Parameters.AddWithValue("@hash", Hash(password.Trim()));
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error during login:\n" + ex.Message,
                                "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
