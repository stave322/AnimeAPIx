using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject_EventAttendance02
{
    public static class DbHelper
    {
        private const string ConnectionString =
           "Server=localhost;Database=eventattendance_db;Uid=root;Pwd=1234567890;SslMode=Disabled;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("MySQL Connection Error:\n" + ex.Message,
                                "Connection Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error); 
                return false;
            }
        }
    }
}
