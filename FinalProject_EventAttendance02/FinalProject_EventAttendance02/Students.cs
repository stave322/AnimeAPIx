using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_EventAttendance02
{
    public class Students
    {
        public string StudentName { get; set; } = "";
        public string StudentID { get; set; } = "";
        public string Course { get; set; } = "";
        public string Year { get; set; } = "";

        public string ScanResult { get; set; } = "";
        public string PhotoPath { get; set; } = "";

        public string CurrentEventName { get; set; } = "No Event Today";
        public string CurrentEventDate { get; set; } = "";
        public string TimeRemaining { get; set; } = "No active event";

        public DateTime CurrentSessionCheckInStart { get; set; } = DateTime.MinValue;

        public Event? GetActiveEvent()
        {
            TimeSpan nowTs = DateTime.Now.TimeOfDay;

            return Event.EventList
                .Where(ev => ev.Date.Date == DateTime.Today)
                .Where(ev =>
                {
                    if (nowTs <= ev.CheckInEnd)
                        return true;

                    if (ev.CheckOutStart.HasValue)
                    {
                        DateTime coEnd = ev.Date.Date + (ev.CheckOutEnd ?? ev.CheckOutStart.Value.Add(TimeSpan.FromHours(1)));
                        return nowTs <= coEnd.TimeOfDay;
                    }

                    return nowTs > ev.CheckInEnd;
                })
                .OrderByDescending(ev =>
                {
                    if (nowTs >= ev.CheckInStart && nowTs <= ev.CheckInEnd) return 3;

                    if (ev.CheckOutStart.HasValue)
                    {
                        DateTime coEnd = ev.Date.Date + (ev.CheckOutEnd ?? ev.CheckOutStart.Value.Add(TimeSpan.FromHours(1)));
                        if (nowTs <= coEnd.TimeOfDay) return 2;
                    }

                    return (nowTs > ev.CheckInEnd && !ev.CheckOutStart.HasValue) ? 1 : 0;
                })
                .ThenBy(ev => ev.CheckInStart)
                .FirstOrDefault();
        }

        
        public void ScanQR(string qrData)
        {
            var todayEvent = GetActiveEvent();

            if (todayEvent == null)
            {
                ClearStudentInfo();
                ScanResult = "No Event Today";
                return;
            }

            if (todayEvent.Date.Date != DateTime.Today)
            {
                ClearStudentInfo();
                ScanResult = "No Event Today";
                return;
            }

            DateTime now = DateTime.Now;
            DateTime checkInStartFull = todayEvent.Date.Date + todayEvent.CheckInStart;
            DateTime checkInEndFull = todayEvent.Date.Date + todayEvent.CheckInEnd;

            bool isCheckInWindow = now >= checkInStartFull && now <= checkInEndFull;
            bool isCheckOutWindow = todayEvent.CheckOutStart.HasValue &&
                                    now >= todayEvent.Date.Date + todayEvent.CheckOutStart.Value &&
                                    (!todayEvent.CheckOutEnd.HasValue ||
                                     now <= todayEvent.Date.Date + todayEvent.CheckOutEnd.Value);

            if (!isCheckInWindow && !isCheckOutWindow)
            {
                if (now < checkInStartFull)
                {
                    ClearStudentInfo();
                    ScanResult = "Check-in Not Started";
                    return;
                }
                ClearStudentInfo();
                ScanResult = "Check-in Closed";
                return;
            }

            var parts = qrData.Split('|');
            if (parts.Length < 2)
            {
                ClearStudentInfo();
                ScanResult = "Invalid QR";
                return;
            }

            string scannedStudentID = parts[0].Trim();
            string scannedYear = parts[1].Trim();

            string scannedCourse = parts.Length >= 5 ? parts[2].Trim() : "";
            string scannedName = parts.Length >= 6 ? parts[3].Trim() : "";

            var allStudents = StudentDB.GetAll();
            generateQR? validQR = null;

            if (!string.IsNullOrEmpty(scannedCourse) && !string.IsNullOrEmpty(scannedName))
            {
                validQR = allStudents.FirstOrDefault(q =>
                    string.Equals(q.StudentID?.Trim(), scannedStudentID, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(q.Year?.Trim(), scannedYear, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(q.Course?.Trim(), scannedCourse, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(q.StudentName?.Trim(), scannedName, StringComparison.OrdinalIgnoreCase));
            }

            if (validQR == null && !string.IsNullOrEmpty(scannedCourse))
            {
                validQR = allStudents.FirstOrDefault(q =>
                    string.Equals(q.StudentID?.Trim(), scannedStudentID, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(q.Year?.Trim(), scannedYear, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(q.Course?.Trim(), scannedCourse, StringComparison.OrdinalIgnoreCase));
            }

            if (validQR == null)
            {
                validQR = allStudents.FirstOrDefault(q =>
                    string.Equals(q.StudentID?.Trim(), scannedStudentID, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(q.Year?.Trim(), scannedYear, StringComparison.OrdinalIgnoreCase));
            }

            if (validQR == null)
                validQR = allStudents.FirstOrDefault(q =>
                    string.Equals(q.QRCodeData?.Trim(), qrData.Trim(), StringComparison.Ordinal));

            if (validQR == null)
            {
                ClearStudentInfo();
                ScanResult = "Invalid QR";
                return;
            }

            StudentName = validQR.StudentName;
            StudentID = validQR.StudentID;
            Course = validQR.Course;
            Year = validQR.Year;
            PhotoPath = validQR.PhotoPath;

            CurrentSessionCheckInStart = checkInStartFull;

            
            Attendance.AttendanceList = AttendanceDB.GetAll() ?? new List<Attendance>();

            
            bool alreadyCheckedOutForThisSession = Attendance.AttendanceList.Any(a =>
                string.Equals(a.StudentID?.Trim(), StudentID?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.Year?.Trim(), Year?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.Course?.Trim(), Course?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.EventName?.Trim(), todayEvent.EventName?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                a.CheckOutTime.HasValue &&
                a.CheckOutTime.Value.Date == todayEvent.Date.Date &&  
                                                                       
                (!todayEvent.CheckOutStart.HasValue ||
                 (a.CheckOutTime.Value.TimeOfDay >= todayEvent.CheckOutStart.Value &&
                  (!todayEvent.CheckOutEnd.HasValue || a.CheckOutTime.Value.TimeOfDay <= todayEvent.CheckOutEnd.Value))));

            if (isCheckInWindow)
            {
                ScanResult = "Check In";
            }
            else if (isCheckOutWindow)
            {
               
                ScanResult = alreadyCheckedOutForThisSession ? "Already Checked Out" : "Check Out";
            }
            else
            {
                ScanResult = "Check-in Closed";
            }
        }

       
        public void UpdateTodayEvent()
        {
            DateTime now = DateTime.Now;

            var todayEvent = GetActiveEvent();

            if (todayEvent != null)
            {
                CurrentEventName = todayEvent.EventName;
                CurrentEventDate = todayEvent.Date.ToString("MMMM dd, yyyy");

                DateTime start = todayEvent.Date.Date + todayEvent.CheckInStart;
                DateTime end = todayEvent.Date.Date + todayEvent.CheckInEnd;

                if (now < start)
                {
                    TimeSpan remaining = start - now;
                    TimeRemaining = $"Starts in {remaining.Hours}h {remaining.Minutes}m";
                }
                else if (now <= end)
                {
                    TimeSpan remaining = end - now;
                    TimeRemaining = $"Ongoing - Ends in {remaining.Hours}h {remaining.Minutes}m";
                }
                else if (todayEvent.CheckOutStart.HasValue)
                {
                    TimeRemaining = "Check-Out Open";
                }
                else
                {
                    TimeRemaining = "Event Ended";
                }
            }
            else
            {
                CurrentEventName = "No Event Today";
                CurrentEventDate = DateTime.Today.ToString("MMMM dd, yyyy");
                TimeRemaining = "No active event";
            }
        }

        public void ClearStudentInfo()
        {
            StudentName = "";
            StudentID = "";
            Course = "";
            Year = "";
            PhotoPath = "";
        }


    }
}
