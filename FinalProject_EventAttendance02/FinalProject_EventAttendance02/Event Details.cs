using Microsoft.VisualBasic;
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
    public partial class Event_Details : Form
    {


        private bool _isEditMode = false;
        private int _editingEventId = -1;

        private System.Windows.Forms.Timer? _realTimeTimer;
        private bool _timerActive = false;
        public Event_Details()
        {
            InitializeComponent();

        }

        private void Event_Details_Load(object sender, EventArgs e)
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            dateEvent.MinDate = DateTime.Today;
            dateEvent.KeyPress += (s, e) => e.Handled = true;
            dateEvent.TabStop = false;

            selSemester.DropDownStyle = ComboBoxStyle.DropDownList;
            selSemester.TabStop = false;
            ApplyPlaceholder(selSemester, "Select Semester");

            checkinStart.Format = DateTimePickerFormat.Custom;
            checkinStart.CustomFormat = "hh:mm tt";
            checkinStart.ShowUpDown = true;

            checkinEnd.Format = DateTimePickerFormat.Custom;
            checkinEnd.CustomFormat = "hh:mm tt";
            checkinEnd.ShowUpDown = true;


            dateEvent.TabStop = false;

            _realTimeTimer = new System.Windows.Forms.Timer();
            _realTimeTimer.Interval = 1000;
            _realTimeTimer.Tick += RealTimeTimer_Tick;
            _realTimeTimer.Start();

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

            // prevent default click/hover flash
            btn.FlatAppearance.MouseDownBackColor = hoverColor;
            btn.FlatAppearance.MouseOverBackColor = hoverColor;
        }

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

        private void Event_Details_Shown(object sender, EventArgs e)
        {

            Color normal = ColorTranslator.FromHtml("#FFFBF1"); // white
            Color hover = ColorTranslator.FromHtml("#DED3B0"); // slightly darker

            CurvePanel(eventinfoPanel, 25);

            CurveButton(btnCreateEvent);
            CurveButton(btnUpdateEvent);
            CurveButton(btnAddTime);

            StyleFlatButton(btnCreateEvent);
            StyleFlatButton(btnUpdateEvent);
            StyleFlatButton(btnAddTime);

            AddHoverEffect(btnCreateEvent, normal, hover);
            AddHoverEffect(btnUpdateEvent, normal, hover);
            AddHoverEffect(btnAddTime, normal, hover);


            this.ActiveControl = null;

            UpdateAddTimeButton();
        }

        private void btnCreateEvent_Click(object sender, EventArgs e)
        {
            if (txtEventName.Text == "" || txtLocation.Text == "" || selSemester.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidTime()) return;


            string newName = txtEventName.Text.Trim();
            DateTime newDate = dateEvent.Value.Date;

            bool nameExistsOnSameDay = Event.EventList.Any(ev =>
                ev.EventName.Equals(newName, StringComparison.OrdinalIgnoreCase) &&
                ev.Date.Date == newDate);

            if (nameExistsOnSameDay)
            {
                MessageBox.Show(
                    $"An event named \"{newName}\" already exists on {newDate:MMMM dd, yyyy}.\n" +
                    "Please use a different name, or use Add Time to extend an existing event.",
                    "Duplicate Event", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Event newEvent = new Event(
                txtEventName.Text.Trim(),
                txtLocation.Text.Trim(),
                dateEvent.Value.Date,
                selSemester.SelectedItem?.ToString() ?? "",
                checkinStart.Value.TimeOfDay,
                checkinEnd.Value.TimeOfDay
            );

            newEvent.Status = newEvent.GetStatus();

            int newDbId = EventDB.Insert(newEvent);
            newEvent.EventID = newDbId;



            MessageBox.Show("Event Created Successfully!", "Event Created", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (this.Owner is Admin adminForm)
                adminForm.LoadEvents();

            this.Close();

        }


        private void btnUpdateEvent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEventName.Text) ||
       string.IsNullOrWhiteSpace(txtLocation.Text) ||
       selSemester.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields.", "Incomplete Fields",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidTime()) return;

            var ev = Event.EventList.FirstOrDefault(x => x.EventID == _editingEventId);
            if (ev == null)
            {
                MessageBox.Show("Event no longer exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ev.EventName = txtEventName.Text.Trim();
            ev.Location = txtLocation.Text.Trim();
            ev.Date = dateEvent.Value.Date;
            ev.Semester = selSemester.Text;
            ev.CheckInStart = checkinStart.Value.TimeOfDay;
            ev.CheckInEnd = checkinEnd.Value.TimeOfDay;

            ev.Status = ev.GetStatus();
            EventDB.Update(ev);

            MessageBox.Show("Event updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

           
            if (this.Owner is Admin adminForm)
            {
                adminForm.RefreshAllDataFromDB();  
                adminForm.LoadEvents();            
                adminForm.LoadDashboard();         
            }

            this.Close();
        }

        private bool IsValidTime()
        {
            //DateTime now = DateTime.Now;

            //DateTime selectedStart = dateEvent.Value.Date + checkinStart.Value.TimeOfDay;

            //if (selectedStart < now)
            //{
            //    MessageBox.Show("Cannot set time in the past.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}

            //if (checkinEnd.Value.TimeOfDay <= checkinStart.Value.TimeOfDay)
            //{
            //    MessageBox.Show("End must be after start.", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}

            //return true;
            // Only block past-time when CREATING — not when editing finished/past events
            if (!_isEditMode)
            {
                DateTime now = DateTime.Now;
                DateTime selectedStart = dateEvent.Value.Date + checkinStart.Value.TimeOfDay;

                if (selectedStart < now)
                {
                    MessageBox.Show("Cannot set time in the past.", "Invalid Time",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            if (checkinEnd.Value.TimeOfDay <= checkinStart.Value.TimeOfDay)
            {
                MessageBox.Show("End must be after start.", "Invalid Time",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        public void LoadEventForEdit(Event ev)
        {
            if (ev == null)
            {
                MessageBox.Show("No event passed to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _timerActive = false;
            _isEditMode = true;
            _editingEventId = ev.EventID;


            checkinStart.ValueChanged -= StopTimerOnUserEdit;
            checkinEnd.ValueChanged -= StopTimerOnUserEdit;


            txtEventName.Text = ev.EventName ?? "";
            txtLocation.Text = ev.Location ?? "";
            dateEvent.Value = ev.Date.Date;

            if (selSemester.Items.Contains(ev.Semester))
                selSemester.SelectedItem = ev.Semester;
            else
                selSemester.SelectedIndex = -1;

            checkinStart.Value = ev.Date.Date + ev.CheckInStart;
            checkinEnd.Value = ev.Date.Date + ev.CheckInEnd;

            btnCreateEvent.Enabled = false;
            btnCreateEvent.Visible = false;
            btnUpdateEvent.Enabled = true;
            btnUpdateEvent.Visible = true;
            btnUpdateEvent.Focus();

            this.AcceptButton = btnUpdateEvent;
            this.Text = $"Edit Event — {ev.EventName ?? "Unnamed"} (ID: {ev.EventID})";
        }

        public void SetNewEventMode()
        {
            _isEditMode = false;
            _editingEventId = -1;
            _timerActive = true;

            txtEventName.Text = "";
            txtLocation.Text = "";
            selSemester.SelectedIndex = -1;

            DateTime now = DateTime.Now;
            DateTime currentMinute = new DateTime(now.Year, now.Month, now.Day,
                                                  now.Hour, now.Minute, 0);

            dateEvent.Value = currentMinute;
            checkinStart.Value = currentMinute;
            checkinEnd.Value = currentMinute.AddHours(1);


            checkinStart.ValueChanged += StopTimerOnUserEdit;
            checkinEnd.ValueChanged += StopTimerOnUserEdit;

            btnCreateEvent.Enabled = true;
            btnCreateEvent.Visible = true;
            btnUpdateEvent.Enabled = false;
            btnUpdateEvent.Visible = false;
            btnAddTime.Enabled = false;

            this.AcceptButton = btnCreateEvent;
            this.Text = "Create New Event";
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            var ev = Event.EventList.FirstOrDefault(e => e.EventID == _editingEventId);
            if (ev == null) return;

            Add_Session ads = new Add_Session(ev);
            ads.ShowDialog();

            if (this.Owner is Admin admin)
            {
                admin.LoadEvents();
                admin.LoadDashboard();
            }
        }

        private void UpdateAddTimeButton()
        {
            var ev = Event.EventList.FirstOrDefault(e => e.EventID == _editingEventId);

            if (ev == null)
            {
                btnAddTime.Enabled = false;
                return;
            }


            if (ev.CheckOutStart == null)
            {
                btnAddTime.Enabled = false;
                return;
            }


            bool nextSessionExists = Event.EventList.Any(other =>
                other.EventID != ev.EventID &&
                other.EventName == ev.EventName &&
                other.Date.Date == DateTime.Today &&
                other.CheckInStart > ev.CheckInEnd);

            if (nextSessionExists)
            {
                btnAddTime.Enabled = false;
                return;
            }


            btnAddTime.Enabled = DateTime.Today == ev.Date.Date;
        }

        private void Event_Details_FormClosed(object sender, FormClosedEventArgs e)
        {
            _realTimeTimer?.Stop();
            _realTimeTimer?.Dispose();
        }

        private void RealTimeTimer_Tick(object? sender, EventArgs e)
        {
            if (_isEditMode || !_timerActive) return;

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

        public void SetFinishedMode()
        {

            txtEventName.Enabled = false;
            txtLocation.Enabled = false;
            dateEvent.Enabled = false;
            selSemester.Enabled = false;
            checkinStart.Enabled = false;
            checkinEnd.Enabled = false;


            btnCreateEvent.Enabled = false;
            btnCreateEvent.Visible = false;
            btnUpdateEvent.Enabled = false;
            btnUpdateEvent.Visible = false;


            UpdateAddTimeButton();

            this.Text = $"Event Details — (Finished)";
        }

        private void ApplyPlaceholder(ComboBox cmb, string placeholder)
        {
            cmb.DrawMode = DrawMode.OwnerDrawFixed;
            cmb.Tag = placeholder;
            cmb.DrawItem += Cmb_DrawItem;
            cmb.SelectedIndex = -1;
        }

        private void Cmb_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not ComboBox cmb) return;

            e.DrawBackground();

            string text;
            Color textColor;

            if (e.Index < 0)
            {
               
                text = cmb.Tag?.ToString() ?? "";
                textColor = Color.Gray;
            }
            else
            {
                text = cmb.Items[e.Index]?.ToString() ?? "";
                textColor = ((e.State & DrawItemState.Selected) != 0)
                    ? SystemColors.HighlightText
                    : cmb.ForeColor;
            }

            using var brush = new SolidBrush(textColor);
            e.Graphics.DrawString(
                text,
                e.Font ?? cmb.Font,
                brush,
                new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 1, e.Bounds.Width, e.Bounds.Height));

            e.DrawFocusRectangle();
        }
    }
}
