using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_EventAttendance02
{
    public class Reports
    {
        public string? StudentID { get; set; }
        public string? StudentName { get; set; }
        public string? Course { get; set; }
        public string? Year { get; set; }
        public string? EventName { get; set; }

        public DateTime? Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public string? Status { get; set; }

        public void GenerateReport()
        {
           
        }

        public void ExportExcel()
        {
            
        }
    }
}
