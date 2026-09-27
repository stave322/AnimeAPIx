using DocumentFormat.OpenXml.Drawing;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject_EventAttendance02
{
    public partial class Add_Session : Form
    {
        private Event currentEvent;
        private System.Windows.Forms.Timer? _clockTimer;
        private bool _timerActive = false;

        public Add_Session(Event ev)
        {
            InitializeComponent();
            currentEvent = ev;
        }

        private void Add_Session_Load(object sender, EventArgs e)
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            if (currentEvent == null) return;

           
            lblEventName.Text = currentEvent.EventName;
            lblLocation.Text = currentEvent.Location;

           
            checkinStart.Format = DateTimePickerFormat.Custom;
            checkinStart.CustomFormat = "hh:mm tt";
            checkinStart.ShowUpDown = true;

            checkinEnd.Format = DateTimePickerFormat.Custom;
            checkinEnd.CustomFormat = "hh:mm tt";
            checkinEnd.ShowUpDown = true;

           
            DateTime now = DateTime.Now;
            DateTime currentMinute = new DateTime(now.Year, now.Month, now.Day,
                                                  now.Hour, now.Minute, 0);

            checkinStart.Value = currentMinute;
            checkinEnd.Value = currentMinute.AddHours(1);

           
            _timerActive = true;
            checkinStart.ValueChanged += StopTimerOnUserEdit;
            checkinEnd.ValueChanged += StopTimerOnUserEdit;

           
            _clockTimer = new System.Windows.Forms.Timer();
            _clockTimer.Interval = 1000;
            _clockTimer.Tick += ClockTimer_Tick;
            _clockTimer.Start();
        }

        private void btnSavetime_Click(object sender, EventArgs e)
        {

            if (currentEvent == null) return;

            DateTime now = DateTime.Now;
            TimeSpan newStart = checkinStart.Value.TimeOfDay;
            TimeSpan newEnd = checkinEnd.Value.TimeOfDay;

           
            if (DateTime.Today != currentEvent.Date.Date)
            {
                MessageBox.Show("Cannot add a session after the event's day has passed.",
                                "Too Late", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            if (now <= currentEvent.Date.Date + currentEvent.CheckInEnd)
            {
                MessageBox.Show("Current session is not yet finished.", "Too Early",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            if (newEnd <= newStart)
            {
                MessageBox.Show("End time must be after start time.", "Invalid Time",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            if (newStart < now.TimeOfDay)
            {
                MessageBox.Show("Start time cannot be in the past.", "Invalid Time",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            bool alreadyExists = Event.EventList.Any(other =>
                other.EventName == currentEvent.EventName &&
                other.Date.Date == DateTime.Today &&   
                other.CheckInStart == newStart &&
                other.CheckInEnd == newEnd);

            if (alreadyExists)
            {
                MessageBox.Show("A session with this time range already exists for this event.",
                                "Duplicate Session", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            Event newSession = new Event(
                currentEvent.EventName,
                currentEvent.Location,
                DateTime.Today,         
                currentEvent.Semester,
                newStart,
                newEnd
            );

            newSession.Status = "Upcoming";
            newSession.CheckOutStart = null;   
            newSession.CheckOutEnd = null;

            int newDbId = EventDB.Insert(newSession);
            newSession.EventID = newDbId;

            MessageBox.Show("New session added successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (Admin.Instance != null)
            {
                Admin.Instance.RefreshAllDataFromDB();
                Admin.Instance.RecalculateAllEventTotals();   
                Admin.Instance.LoadEvents();
                Admin.Instance.LoadDashboard();
                Admin.Instance.LoadReports();

                if (Admin.Instance.IsAttendanceTabReady)
                {
                    Admin.Instance.FilterAttendanceGrid();
                    Admin.Instance.UpdateAttendanceCounts();
                }
            }

           
            foreach (Form f in Application.OpenForms)
            {
                if (f is Event_Details evDetailsForm)
                {
                    evDetailsForm.Close();
                    break;
                }
            }

            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ClockTimer_Tick(object? sender, EventArgs e)
        {
            if (!_timerActive) return;

            DateTime now = DateTime.Now;
            DateTime currentMinute = new DateTime(now.Year, now.Month, now.Day,
                                                  now.Hour, now.Minute, 0);

            checkinStart.Value = currentMinute;
            checkinEnd.Value = currentMinute.AddHours(1);
        }


        private void StopTimerOnUserEdit(object? sender, EventArgs e)
        {
            if (!_timerActive) return;

            _timerActive = false;

            checkinStart.ValueChanged -= StopTimerOnUserEdit;
            checkinEnd.ValueChanged -= StopTimerOnUserEdit;
        }

        private void Add_Session_FormClosed(object sender, FormClosedEventArgs e)
        {
            _clockTimer?.Stop();
            _clockTimer?.Dispose();

        }

        private void CurveButton(Button btn)
        {
            GraphicsPath path = new GraphicsPath();
            int radius = btn.Height;

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();

            btn.Region = new Region(path);
        }

        private void AddHoverEffect(Button btn, Color normalColor, Color hoverColor)
        {
            btn.BackColor = normalColor;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = hoverColor;
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = normalColor;
            };

           
            btn.FlatAppearance.MouseDownBackColor = hoverColor;
            btn.FlatAppearance.MouseOverBackColor = hoverColor;
        }

        private void StyleFlatButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.UseVisualStyleBackColor = false;
            btn.TabStop = false;
            btn.FlatAppearance.MouseOverBackColor = btn.BackColor;
            btn.FlatAppearance.MouseDownBackColor = btn.BackColor;
            btn.FlatAppearance.BorderColor = btn.BackColor;
        }

        private void Add_Session_Shown(object sender, EventArgs e)
        {
            Color normal = ColorTranslator.FromHtml("#FFFBF1"); // white
            Color hover = ColorTranslator.FromHtml("#DED3B0"); // slightly darker

            CurveButton(btnSavetime);
            StyleFlatButton(btnSavetime);
            AddHoverEffect(btnSavetime, normal, hover);
        }
    }
}
