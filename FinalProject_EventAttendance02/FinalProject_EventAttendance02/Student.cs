using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject_EventAttendance02
{
    public partial class Student : Form
    {

        // Holds the currently loaded student data and helper methods.
        private Students currentStudent;
        // Buffer for accumulating characters from a keyboard-style barcode/QR scanner.
        private StringBuilder _scanBuffer = new StringBuilder();

        // Constructor: initialize form, timers, input preview and header.
        public Student()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            currentStudent = new Students();


            if (timer1 == null)
            {
                timer1 = new System.Windows.Forms.Timer();
            }

            timer1.Stop();
            timer1.Interval = 1000;
            timer1.Start();

            UpdateHeader();
        }

        // Form load handler (reserved for future init work).
        private void Student_Load(object sender, EventArgs e)
        {

        }

        // Click handler to return to the main/login form.
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        // Rounds the corners of a panel using the provided radius.
        private void CurvePanel(Panel pnl, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(pnl.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(pnl.Width - radius, pnl.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, pnl.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();

            pnl.Region = new Region(path);
        }

        // Shown handler: apply visual rounding to panels after the form is displayed.
        private void Student_Shown(object sender, EventArgs e)
        {
            CurvePanel(updatePanel, 25);
            CurvePanel(studentPanel, 25);


        }

        // Updates displayed student header (name, photo, current event and time/status).
        public void UpdateHeader()
        {
            if (currentStudent == null) return;

            Event.EventList = EventDB.GetAll();

            DateTime now = DateTime.Now;
            TimeSpan nowTs = now.TimeOfDay;

            var todayEvent = currentStudent.GetActiveEvent();

            if (todayEvent != null)
            {
                currentStudent.CurrentEventName = todayEvent.EventName;
                currentStudent.CurrentEventDate = todayEvent.Date.ToString("MMMM dd, yyyy");

                DateTime ciStart = todayEvent.Date.Date + todayEvent.CheckInStart;
                DateTime ciEnd = todayEvent.Date.Date + todayEvent.CheckInEnd;

                if (now < ciStart)
                {
                    TimeSpan rem = ciStart - now;
                    lblTimeRemaining.Text = $"Starts in {(int)rem.TotalHours:00}:{rem.Minutes:00}:{rem.Seconds:00}";
                    lblTimeRemaining.ForeColor = Color.FromArgb(127, 140, 141); // Gray — not started yet
                }
                else if (now <= ciEnd)
                {
                    TimeSpan rem = ciEnd - now;
                    lblTimeRemaining.Text = $"Ends in {(int)rem.TotalHours:00}:{rem.Minutes:00}:{rem.Seconds:00}";
                    lblTimeRemaining.ForeColor = Color.FromArgb(39, 174, 96);   // Green — check-in open
                }
                else if (todayEvent.CheckOutStart.HasValue)
                {
                    DateTime coEnd = todayEvent.Date.Date +
                        (todayEvent.CheckOutEnd ?? todayEvent.CheckOutStart.Value.Add(TimeSpan.FromHours(1)));

                    if (now <= coEnd)
                    {
                        lblTimeRemaining.Text = "Check-Out Open";
                        lblTimeRemaining.ForeColor = Color.FromArgb(41, 128, 185); // Blue — checkout open
                    }
                    else
                    {
                        lblTimeRemaining.Text = "Check-Out Closed";
                        lblTimeRemaining.ForeColor = Color.FromArgb(192, 57, 43);  // Red — closed
                    }
                }
                else
                {
                    lblTimeRemaining.Text = "Check-In Ended — Awaiting Check-Out";
                    lblTimeRemaining.ForeColor = Color.FromArgb(243, 156, 18);     // Orange — waiting
                }
            }
            else
            {
                currentStudent.CurrentEventName = "No Event Today";
                currentStudent.CurrentEventDate = DateTime.Today.ToString("MMMM dd, yyyy");
                lblTimeRemaining.Text = "No Event Today";
                lblTimeRemaining.ForeColor = Color.FromArgb(229, 57, 53);  // Red
            }

            lblName.Text = currentStudent.StudentName;
            lblStudentID.Text = currentStudent.StudentID;
            lblCourse.Text = currentStudent.Course;
            lblYearLevel.Text = currentStudent.Year;
            lblEventName.Text = currentStudent.CurrentEventName;
            lblEventDate.Text = currentStudent.CurrentEventDate;

            try
            {
                if (!string.IsNullOrEmpty(currentStudent.PhotoPath) && File.Exists(currentStudent.PhotoPath))
                {
                    pictureStudent.Image?.Dispose();
                    pictureStudent.Image = Image.FromFile(currentStudent.PhotoPath);
                }
                else
                {
                    pictureStudent.Image?.Dispose();
                    pictureStudent.Image = null;
                }
            }
            catch { pictureStudent.Image = null; }
        }



        // Handles Enter on the scan textbox: validate QR, perform check-in/out and update DB + admin UI.
        private void txtScan_KeyDown(object sender, KeyEventArgs e)
        {
            
            try
            {
                Event.EventList = EventDB.GetAll();
            }
            catch
            {

            }

            if (e.KeyCode != Keys.Enter) return;

            string qrData = txtScan.Text.Trim();
            currentStudent.UpdateTodayEvent();
            currentStudent.ScanQR(qrData);

            if (currentStudent.ScanResult == "Invalid QR")
            {
                //lblScanResult.Text = "Invalid QR";
                SetScanResultColor("Invalid QR");

                txtScan.Clear();
                UpdateHeader();
                return;
            }

            if (currentStudent.ScanResult == "No Event Today" ||
                currentStudent.CurrentEventName == "No Event Today")
            {
                //lblScanResult.Text = "No Event Today";
                SetScanResultColor("Invalid QR");

                txtScan.Clear();
                return;
            }

            if (currentStudent.ScanResult == "Check-in Not Started" ||
                currentStudent.ScanResult == "Check-in Closed")
            {
                //lblScanResult.Text = currentStudent.ScanResult;
                SetScanResultColor(currentStudent.ScanResult);
                txtScan.Clear();
                return;
            }

            DateTime sessionStart = currentStudent.CurrentSessionCheckInStart;

            var sessionAtt = Attendance.AttendanceList.FirstOrDefault(a =>
                 string.Equals(a.StudentID, currentStudent.StudentID, StringComparison.OrdinalIgnoreCase) &&
                 string.Equals(a.EventName, currentStudent.CurrentEventName, StringComparison.OrdinalIgnoreCase) &&
                 string.Equals(a.Year, currentStudent.Year, StringComparison.OrdinalIgnoreCase) &&
                 string.Equals(a.Course, currentStudent.Course, StringComparison.OrdinalIgnoreCase) &&
                 a.CheckInTime >= sessionStart);

            var todayEvent = currentStudent.GetActiveEvent();

            //  CHECK IN 
            if (currentStudent.ScanResult == "Check In")
            {
                if (sessionAtt != null)
                {
                    //lblScanResult.Text = "Already Checked In";
                    SetScanResultColor("Already Checked In");
                    txtScan.Clear();
                    return;
                }

                Attendance att = new Attendance
                {
                    AttendanceID = Attendance.AttendanceList.Count == 0
                        ? 1 : Attendance.AttendanceList.Max(a => a.AttendanceID) + 1,
                    StudentID = currentStudent.StudentID,
                    StudentName = currentStudent.StudentName,
                    Course = currentStudent.Course,
                    Year = currentStudent.Year,
                    EventName = currentStudent.CurrentEventName,
                    Semester = Event.EventList
                        .FirstOrDefault(ev => ev.EventName == currentStudent.CurrentEventName)?.Semester ?? "",
                    CheckInTime = DateTime.Now,
                    Status = "Check In"
                };

                Attendance.AttendanceList.Add(att);


                int studentDbId = GetStudentDbId(currentStudent.StudentID, currentStudent.Year, currentStudent.Course);
                int eventDbId = GetEventDbId(currentStudent.CurrentEventName);

                if (studentDbId > 0 && eventDbId > 0)
                    att.AttendanceID = AttendanceDB.InsertCheckIn(studentDbId, eventDbId, att.CheckInTime);


                NotifyAdminDashboard(att);
                //lblScanResult.Text = "Check In Success";
                SetScanResultColor("Check In Success");
            }

            // ── CHECK OUT ──
            else if (currentStudent.ScanResult == "Check Out")
            {
                if (todayEvent == null)
                {
                    SetScanResultColor("No Event Today");
                    txtScan.Clear();
                    return;
                }

                DateTime eventCheckInEnd = todayEvent.Date.Date + todayEvent.CheckInEnd;
                if (DateTime.Now < eventCheckInEnd)
                {
                    SetScanResultColor("Check-out not open yet");
                    txtScan.Clear();
                    return;
                }

                
                var sessionCheckInRecord = Attendance.AttendanceList.FirstOrDefault(a =>
                    string.Equals(a.StudentID, currentStudent.StudentID, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(a.EventName, currentStudent.CurrentEventName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(a.Year, currentStudent.Year, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(a.Course, currentStudent.Course, StringComparison.OrdinalIgnoreCase) &&
                    a.CheckInTime != default &&
                    a.CheckInTime.Date == todayEvent.Date.Date &&  
                    a.CheckInTime.TimeOfDay >= todayEvent.CheckInStart &&  
                    a.CheckInTime.TimeOfDay <= todayEvent.CheckInEnd);

                
                bool alreadyCheckedOutThisSession = Attendance.AttendanceList.Any(a =>
                    string.Equals(a.StudentID, currentStudent.StudentID, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(a.EventName, currentStudent.CurrentEventName, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(a.Year, currentStudent.Year, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(a.Course, currentStudent.Course, StringComparison.OrdinalIgnoreCase) &&
                    a.CheckOutTime.HasValue &&
                    a.CheckOutTime.Value.Date == todayEvent.Date.Date &&
                    
                    (!todayEvent.CheckOutStart.HasValue ||
                     (a.CheckOutTime.Value.TimeOfDay >= todayEvent.CheckOutStart.Value &&
                      (!todayEvent.CheckOutEnd.HasValue || a.CheckOutTime.Value.TimeOfDay <= todayEvent.CheckOutEnd.Value))));

                if (alreadyCheckedOutThisSession)
                {
                    SetScanResultColor("Already Checked Out");
                    txtScan.Clear();
                    UpdateHeader();
                    return;
                }

                if (sessionCheckInRecord != null)
                {
                    
                    sessionCheckInRecord.CheckOutTime = DateTime.Now;
                    sessionCheckInRecord.Status = "Completed";

                    if (sessionCheckInRecord.AttendanceID > 0)
                        AttendanceDB.UpdateCheckOut(sessionCheckInRecord.AttendanceID, sessionCheckInRecord.CheckOutTime.Value);

                    NotifyAdminCheckOut(sessionCheckInRecord);
                    SetScanResultColor("Check Out Success");
                }
                else
                {
                   
                    Attendance att = new Attendance
                    {
                        AttendanceID = Attendance.AttendanceList.Count == 0
                            ? 1 : Attendance.AttendanceList.Max(a => a.AttendanceID) + 1,
                        StudentID = currentStudent.StudentID,
                        StudentName = currentStudent.StudentName,
                        Course = currentStudent.Course,
                        Year = currentStudent.Year,
                        EventName = currentStudent.CurrentEventName,
                        Semester = Event.EventList
                            .FirstOrDefault(ev => ev.EventName == currentStudent.CurrentEventName)?.Semester ?? "",
                        CheckInTime = default,
                        CheckOutTime = DateTime.Now,
                        Status = "Check Out"
                    };

                    Attendance.AttendanceList.Add(att);

                    int studentDbId = GetStudentDbId(currentStudent.StudentID, currentStudent.Year, currentStudent.Course);
                    int eventDbId = GetEventDbId(currentStudent.CurrentEventName);

                    if (studentDbId > 0 && eventDbId > 0)
                        att.AttendanceID = AttendanceDB.InsertCheckOut(studentDbId, eventDbId, att.CheckOutTime!.Value);

                    NotifyAdminCheckOut(att);
                    SetScanResultColor("Check Out Success");
                }
            
        
            }

            txtScan.Clear();
            UpdateHeader();


            if (Admin.Instance != null)
            {
                if (Admin.Instance.IsHandleCreated)
                {

                    Admin.Instance.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            Admin.Instance.RefreshAllDataFromDB();
                            Admin.Instance.RecalculateAllEventTotals();
                            Admin.Instance.LoadEvents();
                            Admin.Instance.LoadDashboard();
                            Admin.Instance.UpdateEventStatuses();
                            Admin.Instance.UpdateCheckoutButton();

                            if (Admin.Instance.IsAttendanceTabReady)
                            {
                                Admin.Instance.FilterAttendanceGrid();
                                Admin.Instance.UpdateAttendanceCounts();
                            }
                        }
                        catch
                        {

                        }
                    }));
                }
                else
                {

                    Task.Run(() =>
                    {
                        try
                        {
                            Admin.Instance.RefreshAllDataFromDB();

                        }
                        catch { }
                    });
                }
            }
            else
            {
                var admin = new Admin();
                admin.Hide();
            }
        }

        // Tell the admin UI that a new attendance record exists so it can refresh its view.
        private void NotifyAdminDashboard(Attendance att)
        {
            if (Admin.Instance != null)
            {
                if (Admin.Instance.IsHandleCreated)
                {
                    Admin.Instance.BeginInvoke(new Action(() =>
                    {
                       
                        Admin.Instance.AddAttendance(att);
                    }));
                }
                else
                {
                    
                    Task.Run(() =>
                    {
                        try { Admin.Instance.RefreshAllDataFromDB(); } catch { }
                    });
                }
            }
        }

        // Timer tick: refresh displayed date/time and header every second.
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");

            UpdateHeader();
        }

        // Tell the admin UI a check-out occurred so it can refresh its view.
        private void NotifyAdminCheckOut(Attendance att)
        {
            if (Admin.Instance != null)
            {
                if (Admin.Instance.IsHandleCreated)
                {
                    Admin.Instance.BeginInvoke(new Action(() =>
                    {
                        Admin.Instance.AddAttendance(att);
                    }));
                }
                else
                {
                    Task.Run(() =>
                    {
                        try { Admin.Instance.RefreshAllDataFromDB(); } catch { }
                    });
                }
            }
        }

        // Return the students table primary key, or -1 if not found.
        private int GetStudentDbId(string studentCode, string year, string course)
        {
            using var conn = DbHelper.GetConnection();
            using var cmd = new MySqlCommand(
                @"SELECT student_id FROM students 
                  WHERE student_code = @code 
                    AND year_level   = @year 
                    AND course       = @course 
                  LIMIT 1", conn);
            cmd.Parameters.AddWithValue("@code", studentCode);
            cmd.Parameters.AddWithValue("@year", year);
            cmd.Parameters.AddWithValue("@course", course);
            conn.Open();
            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        // Resolve an event id for a given event name; prefers today's active event, then DB lookups.
        private int GetEventDbId(string eventName)
        {
            
            var activeEvent = GetActiveEvent();
            if (activeEvent != null && string.Equals(activeEvent.EventName, eventName, StringComparison.OrdinalIgnoreCase))
            {
                return activeEvent.EventID;
            }

            
            try
            {
                using var conn = DbHelper.GetConnection();
                using var cmd = new MySqlCommand(
                    "SELECT event_id FROM events WHERE event_name = @name AND DATE(`date`) = @date ORDER BY event_id DESC LIMIT 1", conn);
                cmd.Parameters.AddWithValue("@name", eventName);
                cmd.Parameters.AddWithValue("@date", DateTime.Today.Date);
                conn.Open();
                var result = cmd.ExecuteScalar();
                if (result != null) return Convert.ToInt32(result);
            }
            catch
            {
                
            }

            
            using var conn2 = DbHelper.GetConnection();
            using var cmd2 = new MySqlCommand(
                "SELECT event_id FROM events WHERE event_name = @name ORDER BY event_id DESC LIMIT 1", conn2);
            cmd2.Parameters.AddWithValue("@name", eventName);
            conn2.Open();
            var result2 = cmd2.ExecuteScalar();
            return result2 != null ? Convert.ToInt32(result2) : -1;
        }

        // Accumulate keystrokes from a scanner and trigger a scan when Return is pressed.
        private void Student_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                txtScan.Text = _scanBuffer.ToString();
                _scanBuffer.Clear();

                
                txtScan_KeyDown(this, new KeyEventArgs(Keys.Enter));
            }
            else
            {
                _scanBuffer.Append(e.KeyChar);
            }

            e.Handled = true;
        }

        // Sets the scan result label text and color based on a result string.
        private void SetScanResultColor(string result)
        {
            lblScanResult.Text = result;
            lblScanResult.ForeColor = result switch
            {
                "Check In Success" => Color.FromArgb(39, 174, 96),   // Green
                "Check Out Success" => Color.FromArgb(41, 128, 185),  // Blue
                "Already Checked In" => Color.FromArgb(243, 156, 18),  // Orange
                "Already Checked Out" => Color.FromArgb(243, 156, 18),  // Orange
                "Invalid QR" => Color.FromArgb(192, 57, 43),   // Red
                "No Event Today" => Color.FromArgb(127, 140, 141), // Gray
                "Check-in Not Started" => Color.FromArgb(127, 140, 141), // Gray
                "Check-in Closed" => Color.FromArgb(192, 57, 43),   // Red
                "Check-out not open yet" => Color.FromArgb(243, 156, 18), // Orange
                _ => Color.Black
            };
        }

        // Convenience wrapper: get the student's currently active event from the student model.
        private Event? GetActiveEvent()
        {
            
            return currentStudent?.GetActiveEvent();
        }
    }
}
