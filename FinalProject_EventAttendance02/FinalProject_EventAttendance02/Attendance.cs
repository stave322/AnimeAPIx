using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FinalProject_EventAttendance02
{
    public class Attendance
    {

        public static List<Attendance> AttendanceList { get; set; } = new List<Attendance>();

       
        private static int _tempIdCounter = 0;

        public static int GetNextTempId() => Interlocked.Decrement(ref _tempIdCounter);

        public int AttendanceID { get; set; }
        public string StudentID { get; set; } = "";
        public string StudentName { get; set; } = "";
        public string Course { get; set; } = "";
        public string Year { get; set; } = "";
        public string EventName { get; set; } = "";

        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public string Status { get; set; } = "";
        public string Semester { get; set; } = "";

        public void CheckIn()
        {
            CheckInTime = DateTime.Now;
            Status = "Check-in";
        }

        public void CheckOut()
        {
            CheckOutTime = DateTime.Now;
            Status = "Check-out";
        }
    }
}
