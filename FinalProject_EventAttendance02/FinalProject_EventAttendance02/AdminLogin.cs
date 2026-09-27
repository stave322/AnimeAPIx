using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_EventAttendance02
{
    public class AdminLogin
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public bool Login(string username, string password)
        {
            return AdminDB.Login(username, password);
        }
    }
}
