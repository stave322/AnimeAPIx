using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinalProject_EventAttendance02
{
    public class generateQR
    {

        public static List<generateQR> QRList = new List<generateQR>();
       

        public string StudentName { get; set; } = "";
        public string StudentID { get; set; } = "";
        public string Course { get; set; } = "";
        public string Year { get; set; } = "";
        public string PhotoPath { get; set; } = "";
        public string QRCodeData { get; set; } = "";

        public void Generate()
        {
            var seed = (StudentID ?? "").Trim();
            var namePart = (StudentName ?? "").Trim();
            var yearPart = (Year ?? "").Trim();
            var coursePart = (Course ?? "").Trim();
            var nonce = Guid.NewGuid().ToString("N");

          
            QRCodeData = string.IsNullOrWhiteSpace(seed)
                ? ""
                : $"{seed}|{yearPart}|{coursePart}|{namePart}|{DateTime.UtcNow.Ticks}|{nonce}";
        }
        public static List<generateQR> GetAll() => StudentDB.GetAll();
    }
}
