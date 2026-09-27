using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_EventAttendance02
{
    public class Dashboard
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan CheckInStart { get; set; }
        public TimeSpan CheckInEnd { get; set; }
        public TimeSpan CheckOutStart { get; set; }
        public int TotalCheckIns { get; set; }
        public int TotalCheckOuts { get; set; }
        public string Location { get; set; }
        public string Semester { get; set; }
        public string Status { get; set; }
    }
}
