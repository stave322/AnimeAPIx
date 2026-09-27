using DocumentFormat.OpenXml.Bibliography;
using QRCoder;
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
using MySql.Data.MySqlClient;

namespace FinalProject_EventAttendance02
{
    public partial class Admin : Form
    {
        // DashboardList: cached snapshot used by dashboard UI
        public static List<Dashboard> DashboardList { get; set; } = new List<Dashboard>();

        // Singleton-ish instance for other forms to call back
        public static Admin? Instance;

        // Simple property exposing whether attendance tab finished initialization
        public bool IsAttendanceTabReady => _attendanceTabReady;

        // current searched student id (when using search)
        private string? _searchedStudentID = null;
        // currently selected event id in events grid
        private int _selectedEventID = -1;
        // suppresses filter triggers while changing combo boxes programmatically
        private bool _suppressAttendanceFilter = false;
        // indicates whether attendance UI is ready to be used
        private bool _attendanceTabReady = false;
        // throttles saving event status updates to the DB
        private DateTime _lastStatusSave = DateTime.MinValue;


        // Returns the list of events that should be considered for attendance/evaluation
        private List<Event> GetEventsToEvaluate(string selectedEventFilter, string selectedSemesterFilter)
        {
            if (selectedEventFilter != "All")
            {

                return Event.EventList
                    .Where(e => e.EventName == selectedEventFilter)
                    .ToList();
            }

            // Filter events: no future events, match semester (if selected), and either already has attendance,
            // or check-in time has started, or check-in has records.
            return Event.EventList
                .Where(ev =>
                {
                    if (ev.Date.Date > DateTime.Today) return false;
                    if (selectedSemesterFilter != "All" && ev.Semester != selectedSemesterFilter) return false;
                    if (ev.Date.Date < DateTime.Today)
                        return Attendance.AttendanceList.Any(a => a.EventName == ev.EventName);
                    if (DateTime.Now.TimeOfDay >= ev.CheckInStart) return true;
                    if (Attendance.AttendanceList.Any(a => a.EventName == ev.EventName)) return true;
                    return false;
                })
                .ToList();
        }

        // Returns events to populate event combo boxes (no future events, optional semester filter)
        private List<Event> GetEventsForComboBox(string selectedSemesterFilter)
        {
            return Event.EventList
                .Where(ev =>
                {
                    if (ev.Date.Date > DateTime.Today) return false;
                    if (selectedSemesterFilter != "All" && ev.Semester != selectedSemesterFilter) return false;
                    return true;
                })
                .ToList();
        }

        // Constructor: set up form, load data and start timer
        public Admin()
        {
            InitializeComponent();
            RefreshAllDataFromDB();
            Instance = this;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            timer1.Interval = 1000;
            timer1.Start();
        }

        // Simple helper to refresh all in-memory lists from DB
        public void RefreshAllDataFromDB()
        {
            Event.EventList = EventDB.GetAll();
            Attendance.AttendanceList = AttendanceDB.GetAll();
            generateQR.QRList = StudentDB.GetAll();
        }

        // Form Load: set UI behaviors and initialize data / UI state
        private void Admin_Load(object sender, EventArgs e)
        {
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;

            cmbCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbYear.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEventAttend.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatusAttend.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSelectSemester.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbYearLevelAttend.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourseReports.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEventReports.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbYearReports.DropDownStyle = ComboBoxStyle.DropDownList;

            btncheckoutStart.Enabled = false;


            RefreshAllDataFromDB();
            setData();

            ApplyPlaceholder(cmbYearLevelAttend, "Select Year Level");
            ApplyPlaceholder(cmbCourse, "Select Course");
            ApplyPlaceholder(cmbYear, "Select Year");

            _attendanceTabReady = true;
            _searchedStudentID = null;
            _suppressAttendanceFilter = true;

            if (cmbStatusAttend.Items.Count > 0) cmbStatusAttend.SelectedIndex = 0;
            if (cmbSelectSemester.Items.Count > 0) cmbSelectSemester.SelectedIndex = 0;
            cmbYearLevelAttend.SelectedIndex = -1;
            cmbEventAttend.SelectedIndex = -1;

            _suppressAttendanceFilter = false;

            SyncDashboardList();
            LoadEvents();
            LoadDashboard();
            LoadReports();
            LoadReportsComboBoxes();

            _suppressAttendanceFilter = false;
            _attendanceTabReady = true;

            FilterAttendanceGrid();
            UpdateAttendanceCounts();

            this.BeginInvoke(new Action(() =>
            {
                FilterAttendanceGrid();
                UpdateAttendanceCounts();
            }));
        }

        // Click handler: opens form to create new event
        private void btnNewEvent_Click(object sender, EventArgs e)
        {
            Event_Details evForm = new Event_Details();
            evForm.Owner = this;
            evForm.SetNewEventMode();
            evForm.ShowDialog();
        }

        // Click handler: logout with confirmation and show login form
        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;
            Form1 fr = new Form1();
            fr.Show();
            this.Hide();
        }

        // Click handler: open QR viewer dialog
        private void btnViewQR_Click(object sender, EventArgs e)
        {
            ViewQR view = new ViewQR();
            view.ShowDialog();
        }

        // Rounds a button into pill shape using a radius equal to its height
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

        // Rounds a button with a provided radius
        private void CurveButton2(Button btn, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            btn.Region = new Region(path);
        }

        // Adds simple hover color behavior to buttons
        private void AddHoverEffect(Button btn, Color normalColor, Color hoverColor)
        {
            btn.BackColor = normalColor;
            btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
            btn.MouseLeave += (s, e) => btn.BackColor = normalColor;
            btn.FlatAppearance.MouseDownBackColor = hoverColor;
            btn.FlatAppearance.MouseOverBackColor = hoverColor;
        }

        // Rounds a panel's corners
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

        // Styles a button to be flat (no border)
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

        // Form Shown: apply UI styling and then refresh some data asynchronously
        private void Admin_Shown(object sender, EventArgs e)
        {
            Color normal = ColorTranslator.FromHtml("#FFFBF1");
            Color hover = ColorTranslator.FromHtml("#DED3B0");

            CurveButton2(btnDashboard, 40); CurveButton2(btnEvents, 40);
            CurveButton2(btnAttendance, 40); CurveButton2(btnReports, 40);
            CurveButton2(btnGenerate, 40); CurveButton2(btnLogout, 40);

            StyleFlatButton(btnDashboard); StyleFlatButton(btnEvents);
            StyleFlatButton(btnAttendance); StyleFlatButton(btnReports);
            StyleFlatButton(btnGenerate); StyleFlatButton(btnLogout);

            AddHoverEffect(btnDashboard, normal, hover); AddHoverEffect(btnEvents, normal, hover);
            AddHoverEffect(btnAttendance, normal, hover); AddHoverEffect(btnReports, normal, hover);
            AddHoverEffect(btnGenerate, normal, hover); AddHoverEffect(btnLogout, normal, hover);

            CurvePanel(totaleventsPanel, 25);
            CurvePanel(totalcheckinsPanel, 25);
            CurvePanel(totalcheckoutPanel, 25);
            CurvePanel(filtereventPanel, 25);

            CurveButton(btnNewEvent); CurveButton(btnSearchEvent);
            CurveButton(btnDeleteEvent); CurveButton(btncheckoutStart);
            StyleFlatButton(btnNewEvent); StyleFlatButton(btnSearchEvent);
            StyleFlatButton(btnDeleteEvent); StyleFlatButton(btncheckoutStart);
            AddHoverEffect(btnNewEvent, normal, hover); AddHoverEffect(btnSearchEvent, normal, hover);
            AddHoverEffect(btnDeleteEvent, normal, hover); AddHoverEffect(btncheckoutStart, normal, hover);

            CurvePanel(filtersattendancePanel, 25); CurvePanel(studentinfoPanel, 25);
            CurvePanel(totalevents3Panel, 25); CurvePanel(totalattendedPanel, 25);
            CurvePanel(totalabsentPanel, 25);
            CurveButton(btnSearchAttend); StyleFlatButton(btnSearchAttend);
            AddHoverEffect(btnSearchAttend, normal, hover);

            CurvePanel(filterReports, 25);
            CurveButton(btnExport); StyleFlatButton(btnExport);
            AddHoverEffect(btnExport, normal, hover);

            CurvePanel(studentinfo2Panel, 25);
            CurveButton(btnGenerateQR); CurveButton(btnViewQR); CurveButton(btnSave);
            StyleFlatButton(btnGenerateQR); StyleFlatButton(btnViewQR); StyleFlatButton(btnSave);
            AddHoverEffect(btnGenerateQR, normal, hover);
            AddHoverEffect(btnViewQR, normal, hover);
            AddHoverEffect(btnSave, normal, hover);

            this.BeginInvoke(new Action(() =>
            {
                try
                {
                    RefreshAllDataFromDB();
                    SyncDashboardList();
                    LoadEvents();
                    LoadEventComboBox();
                    LoadDashboard();
                    LoadReports();
                    LoadReportsComboBoxes();
                    _attendanceTabReady = true;
                    FilterAttendanceGrid();
                    UpdateAttendanceCounts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Initialization error:\n" + ex.Message,
                                    "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }));
        }

        // Switch to dashboard tab and refresh aggregated data
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            LoadDashboard();
            RefreshAllDataFromDB();
            RecalculateAllEventTotals();
            LoadDashboard();
        }
        
        // Switch to events tab and load events
        private void btnEvents_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            LoadEvents();
        }

        // Switch to attendance tab and refresh attendance-related UI
        private void btnAttendance_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
            _attendanceTabReady = true;
            RefreshAllDataFromDB();
            RecalculateAllEventTotals();
            LoadEvents();
            LoadDashboard();
            FilterAttendanceGrid();
            UpdateAttendanceCounts();
            RefreshStudentInfoLabels();
        }

        // Switch to reports tab and load reports data
        private void btnReports_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
            Attendance.AttendanceList = AttendanceDB.GetAll();
            generateQR.QRList = StudentDB.GetAll();
            Event.EventList = EventDB.GetAll();
            LoadReports();
        }

        // Switch to generate (QR) tab
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage5;
        }

        // Populate events grid from Event.EventList and refresh related UI
        public void LoadEvents()
        {
            try
            {
                RefreshAllDataFromDB();
                RecalculateAllEventTotals();
                dgvEvent.Rows.Clear();

                foreach (Event ev in Event.EventList)
                {
                    string checkoutTime = ev.CheckOutStart.HasValue
                        ? DateTime.Today.Add(ev.CheckOutStart.Value).ToString("hh:mm tt") : "-";

                    dgvEvent.Rows.Add(
                        ev.EventID.ToString("D3"), ev.EventName,
                        ev.Date.ToString("MMMM dd, yyyy"),
                        DateTime.Today.Add(ev.CheckInStart).ToString("hh:mm tt"),
                        DateTime.Today.Add(ev.CheckInEnd).ToString("hh:mm tt"),
                        checkoutTime, ev.TotalCheckIns, ev.TotalCheckOuts,
                        ev.Location, ev.Semester, ev.Status);
                }

                if (_selectedEventID != -1)
                {
                    foreach (DataGridViewRow row in dgvEvent.Rows)
                    {
                        if (int.TryParse(row.Cells[0].Value?.ToString(), out int rowEventID)
                            && rowEventID == _selectedEventID)
                        {
                            dgvEvent.ClearSelection();
                            row.Selected = true;
                            dgvEvent.CurrentCell = row.Cells[0];
                            break;
                        }
                    }
                }

                SyncDashboardList();
                LoadDashboard();
                LoadEventComboBox();
                LoadReportsComboBoxes();
                UpdateCheckoutButton();
                ColorCodeEventsGrid(dgvEvent);  //
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in LoadEvents:\n\n" + ex.Message + "\n\n" + ex.StackTrace,
                                "LoadEvents Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Start check-out window for selected event (with validations)
        private void btncheckoutStart_Click(object sender, EventArgs e)
        {
            if (_selectedEventID == -1)
            {
                MessageBox.Show("Please select an event first.", "No Event Selected",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            var ev = Event.EventList.FirstOrDefault(x => x.EventID == _selectedEventID);
            if (ev == null)
            {
                MessageBox.Show("Selected event not found.", "Error",
                                               MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }

            if (ev.Date.Date != DateTime.Today)
            {
                MessageBox.Show("Check-out can only be started for today's events.", "Invalid",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            if (ev.CheckOutStart.HasValue)
            {
                MessageBox.Show("Check-out has already been started for this event.", "Already Started",
                                MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }

            if (DateTime.Now.TimeOfDay < ev.CheckInEnd)
            {
                MessageBox.Show($"Check-out can only start after Check-in ends at {DateTime.Today.Add(ev.CheckInEnd):hh:mm tt}.",
                                "Too Early", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }


            foreach (var other in Event.EventList.Where(x =>
                x.EventID != ev.EventID && x.CheckOutStart.HasValue &&
                (!x.CheckOutEnd.HasValue || DateTime.Now.TimeOfDay <= x.CheckOutEnd.Value)))
            {
                other.CheckOutEnd = DateTime.Now.TimeOfDay;
                other.Status = "Finished";
                EventDB.Update(other);
            }


            TimeSpan now = DateTime.Now.TimeOfDay;
            ev.CheckOutStart = now;
            ev.CheckOutEnd = now.Add(TimeSpan.FromMinutes(2));
            ev.Status = "Ongoing";



            EventDB.Update(ev);

            MessageBox.Show(
                $"Check-out started for \"{ev.EventName}\"\n" +
                $"Opens:  {DateTime.Today.Add(ev.CheckOutStart.Value):hh:mm tt}\n" +
                $"Closes: {DateTime.Today.Add(ev.CheckOutEnd.Value):hh:mm tt}",
                "Check-Out Started", MessageBoxButtons.OK, MessageBoxIcon.Information);

            RefreshAllDataFromDB();
            RecalculateAllEventTotals();
            LoadEvents();
            LoadDashboard();
            LoadEventComboBox();
            LoadReportsComboBoxes();

            if (_attendanceTabReady)
            {
                Attendance.AttendanceList = AttendanceDB.GetAll() ?? new List<Attendance>();
                generateQR.QRList = StudentDB.GetAll() ?? new List<generateQR>();
                FilterAttendanceGrid();
                UpdateAttendanceCounts();
            }

            UpdateCheckoutButton();

            foreach (Form f in Application.OpenForms)
            {
                if (f is Student studentForm)
                {
                    studentForm.Invoke(new Action(() => studentForm.UpdateHeader()));
                    break;
                }
            }
        }

        // Hook for selection changes in events grid: update checkout button availability
        private void dgvEvent_SelectionChanged(object sender, EventArgs e) => UpdateCheckoutButton();


        // Search events by name and display matching results in grid
        private void btnSearchEvent_Click(object sender, EventArgs e)
        {
            string searchText = txtSearchEvent.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(searchText)) { LoadEvents(); return; }

            dgvEvent.Rows.Clear();
            foreach (Event ev in Event.EventList)
            {
                if (!ev.EventName.ToLower().Contains(searchText)) continue;
                string checkoutTime = ev.CheckOutStart.HasValue
                    ? DateTime.Today.Add(ev.CheckOutStart.Value).ToString("hh:mm tt") : "-";
                dgvEvent.Rows.Add(ev.EventID.ToString("D3"), ev.EventName,
                    ev.Date.ToString("MMMM dd, yyyy"),
                    DateTime.Today.Add(ev.CheckInStart).ToString("hh:mm tt"),
                    DateTime.Today.Add(ev.CheckInEnd).ToString("hh:mm tt"),
                    checkoutTime, ev.TotalCheckIns, ev.TotalCheckOuts,
                    ev.Location, ev.Semester, ev.Status);
            }

            if (dgvEvent.Rows.Count == 0)
                MessageBox.Show("No events found matching your search.", "Search Result",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

            ColorCodeEventsGrid(dgvEvent);
        }

        // Delete selected event after confirmation and validations
        private void btnDeleteEvent_Click(object sender, EventArgs e)
        {
            if (dgvEvent.CurrentRow == null) { MessageBox.Show("Please select an event."); return; }
            if (!int.TryParse(dgvEvent.CurrentRow.Cells[0].Value?.ToString(), out int eventID))
            { MessageBox.Show("Invalid Event ID."); return; }
            if (Event.EventList == null) { MessageBox.Show("Event list is not initialized."); return; }

            var ev = Event.EventList.FirstOrDefault(e => e.EventID == eventID);
            if (ev == null) { MessageBox.Show("Event not found."); return; }

            string currentStatus = ev.GetStatus();
            if (currentStatus == "Ongoing")
            {
                MessageBox.Show("Cannot delete an ongoing event.", "Blocked",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (currentStatus == "Finished")
            {
                MessageBox.Show("Cannot delete a finished event.", "Blocked",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            var confirm = MessageBox.Show($"Are you sure you want to delete \"{ev.EventName}\"?",
                                           "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;


            EventDB.Delete(ev.EventID);


            Event.EventList.Remove(ev);


            Event.ReassignIDs();


            foreach (var evt in Event.EventList)
            {
                EventDB.Update(evt);
            }
            if (_selectedEventID == eventID)
                _selectedEventID = -1;

            LoadEvents();
            LoadEventComboBox();
            LoadReportsComboBoxes();

            MessageBox.Show("Deleted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        // Text changed helper: reload events when search cleared
        private void txtSearchEvent_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchEvent.Text.Trim())) LoadEvents();
        }

        // Rebuilds DashboardList from Event.EventList (simple projection)
        private void SyncDashboardList()
        {
            DashboardList.Clear();
            if (Event.EventList == null) return;
            foreach (var ev in Event.EventList)
            {
                if (ev == null) continue;
                DashboardList.Add(new Dashboard
                {
                    EventID = ev.EventID,
                    EventName = ev.EventName ?? "",
                    Date = ev.Date,
                    CheckInStart = ev.CheckInStart,
                    CheckInEnd = ev.CheckInEnd,
                    CheckOutStart = ev.CheckOutStart ?? default,
                    TotalCheckIns = ev.TotalCheckIns,
                    TotalCheckOuts = ev.TotalCheckOuts,
                    Location = ev.Location ?? "",
                    Semester = ev.Semester ?? "",
                    Status = ev.Status ?? ""
                });
            }
        }

        // Populate dashboard grid and totals
        public void LoadDashboard()
        {
            try
            {
                dgvHome.Rows.Clear();
                RefreshAllDataFromDB();
                RecalculateAllEventTotals();


                var eventsToDisplay = Event.EventList
                    .OrderBy(ev => ev.Date)
                    .ToList();

                foreach (var ev in eventsToDisplay)
                {
                    string checkoutTime = ev.CheckOutStart.HasValue
                        ? DateTime.Today.Add(ev.CheckOutStart.Value).ToString("hh:mm tt") : "-";

                    int rowIndex = dgvHome.Rows.Add(
                        ev.EventID.ToString("D3"),
                        ev.EventName,
                        ev.Date.ToString("MMMM dd, yyyy"),
                        DateTime.Today.Add(ev.CheckInStart).ToString("hh:mm tt"),
                        DateTime.Today.Add(ev.CheckInEnd).ToString("hh:mm tt"),
                        checkoutTime,
                        ev.TotalCheckIns,
                        ev.TotalCheckOuts,
                        ev.Location,
                        ev.Semester,
                        ev.Status);


                    if (ev.Date.Date > DateTime.Today)
                    {
                        foreach (DataGridViewCell cell in dgvHome.Rows[rowIndex].Cells)
                        {
                            cell.Style.ForeColor = Color.Gray;
                            cell.Style.BackColor = Color.FromArgb(240, 240, 240);
                        }
                    }
                }

                txtTotalEvents.Text = eventsToDisplay.Count.ToString();
                txtTotalCheckIn.Text = eventsToDisplay.Sum(e => e.TotalCheckIns).ToString();
                txtTotalCheckOut.Text = eventsToDisplay.Sum(e => e.TotalCheckOuts).ToString();
                ColorCodeEventsGrid(dgvHome);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard:\n" + ex.Message, "Error");
            }
        }

        // Timer tick: update date/time and event status periodically
        private void timer1_Tick(object sender, EventArgs e)
        {
            txtDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            txtTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            UpdateEventStatuses();
            UpdateCheckoutButton();
        }

        // Generate QR for entered student info and display it
        private void btnGenerateQR_Click(object sender, EventArgs e)
        {

            string studentID = (txtStudentID.ForeColor == Color.Gray || txtStudentID.Text.Trim() == "Enter Student ID") ? "" : txtStudentID.Text.Trim();
            string studentName = (txtStudentName.ForeColor == Color.Gray || txtStudentName.Text.Trim() == "Enter Student Name") ? "" : txtStudentName.Text.Trim();

            if (string.IsNullOrWhiteSpace(studentName) || string.IsNullOrWhiteSpace(studentID) ||
                cmbCourse.SelectedIndex == -1 || cmbYear.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill all fields before generating a QR code.",
                                "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                generateQR qr = new generateQR
                {
                    StudentName = studentName,
                    StudentID = studentID,
                    Course = cmbCourse.Text,
                    Year = cmbYear.SelectedItem?.ToString() ?? "",
                    PhotoPath = pictureChoose.Tag as string ?? ""
                };
                qr.Generate();

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qr.QRCodeData, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                pictureQR.Image = qrCode.GetGraphic(10);
            }
            catch (Exception ex)
            {
                MessageBox.Show("QR Generation Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Save or update generated QR to DB and optionally download image
        private void btnSave_Click(object sender, EventArgs e)
        {

            string studentID = (txtStudentID.ForeColor == Color.Gray || txtStudentID.Text.Trim() == "Enter Student ID") ? "" : txtStudentID.Text.Trim();
            string studentName = (txtStudentName.ForeColor == Color.Gray || txtStudentName.Text.Trim() == "Enter Student Name") ? "" : txtStudentName.Text.Trim();
            if (string.IsNullOrWhiteSpace(studentName) || string.IsNullOrWhiteSpace(studentID) ||
                cmbCourse.SelectedIndex == -1 || cmbYear.SelectedIndex == -1)
            { MessageBox.Show("Please complete the fields first.", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            string selectedYear = cmbYear.SelectedItem?.ToString() ?? "";
            string selectedCourse = cmbCourse.SelectedItem?.ToString() ?? "";

            generateQR qr = new generateQR
            {
                StudentName = studentName,
                StudentID = studentID,
                Course = selectedCourse,
                Year = selectedYear,
                PhotoPath = pictureChoose?.Tag?.ToString() ?? ""
            };
            qr.Generate();


            bool alreadyExists = StudentDB.Exists(qr.StudentID, qr.Year);
            if (!alreadyExists)
            {
                StudentDB.Insert(qr);
                MessageBox.Show("QR Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                var confirm = MessageBox.Show(
                    $"A QR for Student ID \"{studentID}\" (Year: {selectedYear}) already exists.\n" +
                    "Do you want to regenerate and overwrite it?",
                    "Overwrite Existing QR?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                StudentDB.Update(qr);
                MessageBox.Show("QR Regenerated and Updated Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            LoadReportsComboBoxes();

            // Download QR image
            try
            {
                if (string.IsNullOrWhiteSpace(qr.QRCodeData))
                {
                    MessageBox.Show("QR code data is empty. Cannot download.", "No QR Data",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    using SaveFileDialog sfd = new SaveFileDialog
                    {
                        Filter = "PNG Image|*.png",
                        FileName = $"{qr.StudentID}_QR.png",
                        Title = "Save QR Code Image"
                    };

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using QRCodeGenerator generator = new QRCodeGenerator();
                        using QRCodeData data = generator.CreateQrCode(qr.QRCodeData, QRCodeGenerator.ECCLevel.Q);
                        using QRCode qrCode = new QRCode(data);
                        using Bitmap qrImage = qrCode.GetGraphic(10);

                        using Font valueFont = new Font("Segoe UI", 11f, FontStyle.Regular);
                        using Bitmap tempBmp = new Bitmap(1, 1);
                        using Graphics tempG = Graphics.FromImage(tempBmp);

                        float lineH = tempG.MeasureString("Ag", valueFont).Height;
                        int padding = 10;
                        int labelH = (int)(lineH * 4) + padding * 2;

                        using Bitmap finalImage = new Bitmap(qrImage.Width, qrImage.Height + labelH);
                        using Graphics g = Graphics.FromImage(finalImage);

                        g.Clear(Color.White);
                        g.DrawImage(qrImage, 0, 0);

                        float startY = qrImage.Height + padding;
                        g.DrawString($"School ID : {qr.StudentID}", valueFont, Brushes.Black, 10f, startY);
                        g.DrawString($"Name      : {qr.StudentName}", valueFont, Brushes.Black, 10f, startY + lineH);
                        g.DrawString($"Course    : {qr.Course}", valueFont, Brushes.Black, 10f, startY + lineH * 2);
                        g.DrawString($"Year Level: {qr.Year}", valueFont, Brushes.Black, 10f, startY + lineH * 3);

                        finalImage.Save(sfd.FileName);
                        MessageBox.Show("QR Code downloaded successfully!", "Success",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error downloading QR:\n" + ex.Message, "Download Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ClearQRFields();

            foreach (Form f in Application.OpenForms)
            {
                if (f is ViewQR viewQRForm) { viewQRForm.LoadQRData(); break; }
            }
        }

        // Choose a photo for the student
        private void pictureChoose_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureChoose.Image = Image.FromFile(ofd.FileName);
                pictureChoose.Tag = ofd.FileName;
            }
            else
            {
                if (pictureChoose.Image == null)
                { MessageBox.Show("Please choose a student photo.", "Photo Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            }
        }

        // Clear the QR generation input fields
        private void ClearQRFields()
        {
            txtStudentName.Clear(); txtStudentID.Clear();
            cmbCourse.SelectedIndex = -1; cmbYear.SelectedIndex = -1;
            pictureChoose.Image = null; pictureChoose.Tag = null;
            pictureQR.Image = null;
        }

        // Called externally when a new attendance is recorded to refresh UI
        public void AddAttendance(Attendance? att)
        {

            if (att == null) return;
            RefreshAllDataFromDB();
            RecalculateAllEventTotals();
            LoadEvents();
            LoadDashboard();
            LoadReports();
            LoadReportsComboBoxes();
            if (_attendanceTabReady)
            {
                FilterAttendanceGrid();
                UpdateAttendanceCounts();
            }
        }

        // Populate attendance grid from Attendance.AttendanceList
        public void LoadAttendanceGrid()
        {
            dgvAttendance.Rows.Clear();
            Attendance.AttendanceList = AttendanceDB.GetAll();
            generateQR.QRList = StudentDB.GetAll();

            foreach (var att in Attendance.AttendanceList)
            {
                if (att == null) continue;


                string yearDisplay = generateQR.QRList
                    .FirstOrDefault(q =>
                        string.Equals(q.StudentID?.Trim(), att.StudentID?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(q.Year?.Trim(), att.Year?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(q.Course?.Trim(), att.Course?.Trim(), StringComparison.OrdinalIgnoreCase))
                    ?.Year ?? att.Year ?? "";

                string checkInDisplay = att.CheckInTime == default ? "-" : att.CheckInTime.ToString("hh:mm:ss tt");
                string checkOutDisplay = att.CheckOutTime?.ToString("hh:mm:ss tt") ?? "-";

                string statusDisplay = att.Status switch
                {
                    "Check In" or "Check-in" => "Check In",
                    "Check Out" or "Check-out" => "Late", 
                    "Completed" => "Completed",
                    _ => string.IsNullOrWhiteSpace(att.Status) ? "-" : att.Status
                };

                dgvAttendance.Rows.Add(
                    att.AttendanceID.ToString("D4"),
                    att.StudentID ?? "-",
                    att.Course ?? "-",
                    yearDisplay,
                    att.EventName ?? "-",
                    checkInDisplay,
                    checkOutDisplay,
                    statusDisplay,
                    att.Semester ?? "-");
            }

            UpdateAttendanceCounts();
        }

        // Re-evaluate statuses of events (Upcoming/Ongoing/Finished) and save periodically
        public void UpdateEventStatuses()
        {
            bool changed = false;
            TimeSpan nowTs = DateTime.Now.TimeOfDay;

            foreach (var ev in Event.EventList)
            {
                string newStatus;
                if (ev.Date.Date != DateTime.Today)
                    newStatus = ev.GetStatus();
                else if (ev.CheckOutStart.HasValue)
                    newStatus = (ev.CheckOutEnd.HasValue && nowTs > ev.CheckOutEnd.Value) ? "Finished" : "Ongoing";
                else if (nowTs >= ev.CheckInStart && nowTs <= ev.CheckInEnd)
                    newStatus = "Ongoing";
                else if (nowTs > ev.CheckInEnd)
                    newStatus = "Finished";
                else
                    newStatus = "Upcoming";

                if (ev.Status != newStatus)
                {
                    ev.Status = newStatus;
                    changed = true;
                    if ((DateTime.Now - _lastStatusSave).TotalSeconds >= 30)
                    {
                        EventDB.Update(ev);
                        _lastStatusSave = DateTime.Now;
                    }
                }
            }

            if (changed)
            {
                RecalculateAllEventTotals();
                LoadEvents();
                LoadDashboard();
                LoadReports();
                LoadReportsComboBoxes();
                if (_attendanceTabReady)
                {
                    FilterAttendanceGrid();
                    UpdateAttendanceCounts();
                }
                UpdateCheckoutButton();
            }
        }

        // Placeholder behavior for event search textbox
        private void txtSearchEvent_Enter(object sender, EventArgs e)
        {
            if (txtSearchEvent.Text == "Search Event")
            { txtSearchEvent.Text = ""; txtSearchEvent.ForeColor = Color.Black; }
        }

        private void txtSearchEvent_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchEvent.Text))
            { txtSearchEvent.Text = "Search Event"; txtSearchEvent.ForeColor = Color.Gray; LoadEvents(); }
        }

        // Placeholder behavior for attendance search textbox
        private void txtSearchAttendance_Enter(object sender, EventArgs e)
        {
            if (txtSearchAttendance.Text == "Search Student ID")
            { txtSearchAttendance.Text = ""; txtSearchAttendance.ForeColor = Color.Black; }
        }

        private void txtSearchAttendance_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchAttendance.Text))
            {
                txtSearchAttendance.Text = "Search Student ID";
                txtSearchAttendance.ForeColor = Color.Gray;
                if (_searchedStudentID != null) { ClearStudentInfo(); FilterAttendanceGrid(); }
            }
        }


        // Populate event attendance combo box with deduplicated event names
        private void LoadEventComboBox()
        {
            _suppressAttendanceFilter = true;
            foreach (var ev in Event.EventList)
                if (!ev.CheckOutStart.HasValue) ev.Status = ev.GetStatus();

            string selectedSemester = cmbSelectSemester.SelectedItem?.ToString() ?? "All";
            string? previousSelection = cmbEventAttend.SelectedItem?.ToString();

            cmbEventAttend.Items.Clear();
            cmbEventAttend.Items.Add("All");


            foreach (var ev in GetEventsForComboBox(selectedSemester)
                .GroupBy(e => e.EventName)
                .Select(g => g.First()))
                cmbEventAttend.Items.Add(ev.EventName);

            if (previousSelection != null && cmbEventAttend.Items.Contains(previousSelection))
                cmbEventAttend.SelectedItem = previousSelection;
            else
                cmbEventAttend.SelectedIndex = 0;

            _suppressAttendanceFilter = false;
        }

        // Event handler for event combobox selection - triggers filtering
        private void cmbEventAttend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressAttendanceFilter) return;
            if (!_attendanceTabReady) return;
            FilterAttendanceGrid();

            RefreshStudentInfoLabels();
        }

        // Main filter logic: builds rows for attendance grid depending on selected filters and searched student
        public void FilterAttendanceGrid()
        {
            if (!_attendanceTabReady) return;

            Attendance.AttendanceList = AttendanceDB.GetAll();
            generateQR.QRList = StudentDB.GetAll();

            string selectedEvent = (cmbEventAttend.SelectedIndex < 1) ? "All" : cmbEventAttend.SelectedItem?.ToString() ?? "All";
            string selectedSemester = (cmbSelectSemester.SelectedIndex < 1) ? "All" : cmbSelectSemester.SelectedItem?.ToString() ?? "All";
            string selectedYear = (cmbYearLevelAttend.SelectedIndex == -1) ? "All" : cmbYearLevelAttend.SelectedItem?.ToString() ?? "All";
            string selectedStatus = (cmbStatusAttend.SelectedIndex < 1) ? "All" : cmbStatusAttend.SelectedItem?.ToString()?.Trim() ?? "All";

            dgvAttendance.Rows.Clear();
            dgvAttendance.AllowUserToAddRows = false;


            string ComputeStatus(bool hasCheckIn, bool hasCheckOut)
            {
                if (hasCheckIn && hasCheckOut) return "Completed";
                if (hasCheckIn) return "Check-in";
                if (hasCheckOut) return "Late";
                return "Not Attended";
            }


            string NormalizeStatusFilter(string raw) => raw.Trim().ToLowerInvariant() switch
            {
                "all" => "All",
                "completed" => "Completed",
                "check-in" => "Check-in",
                "check in" => "Check-in",
                "checked in" => "Check-in",
                "late" => "Late",
                "check-out" => "Late",
                "check out" => "Late",
                "checked out" => "Late",
                "not attended" => "Not Attended",
                "absent" => "Not Attended",
                "no attendance" => "Not Attended",
                _ => raw.Trim()
            };

            string normalizedFilter = selectedStatus == "All" ? "All" : NormalizeStatusFilter(selectedStatus);

            bool PassesStatusFilter(string computedStatus)
            {
                if (normalizedFilter == "All") return true;
                string rawLower = selectedStatus.Trim().ToLowerInvariant();
                if (rawLower == "attend" || rawLower == "attended")
                    return computedStatus == "Check-in" || computedStatus == "Late" || computedStatus == "Completed";  // CHANGED: Include "Late"
                return computedStatus == normalizedFilter;
            }

            if (!string.IsNullOrWhiteSpace(_searchedStudentID))
            {
                var matchingStudents = generateQR.QRList
                    .Where(q => string.Equals(q.StudentID?.Trim(), _searchedStudentID, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (!matchingStudents.Any()) { UpdateAttendanceCounts(); return; }

                if (selectedYear != "All")
                    matchingStudents = matchingStudents
                        .Where(q => string.Equals(q.Year?.Trim(), selectedYear.Trim(), StringComparison.OrdinalIgnoreCase))
                        .ToList();

                if (!matchingStudents.Any()) { UpdateAttendanceCounts(); return; }

                var eventsToShow = GetEventsToEvaluate(selectedEvent, selectedSemester);

                foreach (var student in matchingStudents)
                {
                    foreach (var ev in eventsToShow)
                    {
                        bool hasCheckIn = HasCheckInForStudentEvent(student.StudentID, student.Year, student.Course, ev);
                        bool hasCheckOut = HasCheckOutForStudentEvent(student.StudentID, student.Year, student.Course, ev);

                        bool eventIsOver = ev.Date.Date < DateTime.Today ||
                   (ev.Date.Date == DateTime.Today && DateTime.Now.TimeOfDay > ev.CheckInEnd);
                        if (!hasCheckIn && !hasCheckOut && !eventIsOver) continue;

                        string computedStatus = ComputeStatus(hasCheckIn, hasCheckOut);
                        if (!PassesStatusFilter(computedStatus)) continue;

                        var checkInRecord = GetFirstCheckInRecord(student.StudentID, student.Year, student.Course, ev);
                        var checkOutRecord = GetFirstCheckOutRecord(student.StudentID, student.Year, student.Course, ev);

                        object[] row =
                        {
                            hasCheckIn  ? checkInRecord!.AttendanceID.ToString("D4")
                                        : hasCheckOut ? checkOutRecord!.AttendanceID.ToString("D4") : "-",
                            student.StudentID ?? "-",
                            student.Course    ?? "-",
                            student.Year      ?? "-",
                            ev.EventName,
                            (hasCheckIn && checkInRecord != null)
                                ? checkInRecord.CheckInTime.ToString("hh:mm:ss tt") : "-",
                            (hasCheckOut && checkOutRecord?.CheckOutTime.HasValue == true)
                                ? checkOutRecord!.CheckOutTime!.Value.ToString("hh:mm:ss tt") : "-",
                            computedStatus,
                            ev.Semester ?? "-"
                        };

                        dgvAttendance.Rows.Add(row);
                    }
                }

                UpdateAttendanceCounts();
                ColorCodeAttendanceGrid();  //
                return;
            }

            var eventsNormal = GetEventsToEvaluate(selectedEvent, selectedSemester);
            var scopedStudents = generateQR.QRList
                .Where(q => selectedYear == "All" ||
                            string.Equals(q.Year?.Trim(), selectedYear.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var ev in eventsNormal)
            {
                foreach (var student in scopedStudents)
                {
                    if (string.IsNullOrWhiteSpace(student.StudentID)) continue;

                    bool hasCheckIn = HasCheckInForStudentEvent(student.StudentID, student.Year, student.Course, ev);
                    bool hasCheckOut = HasCheckOutForStudentEvent(student.StudentID, student.Year, student.Course, ev);

                    //bool eventIsOver = ev.Date.Date < DateTime.Today ||
                    // (ev.Date.Date == DateTime.Today && DateTime.Now.TimeOfDay > ev.CheckInEnd);
                    //if (!hasCheckIn && !hasCheckOut && !eventIsOver) continue;

                    //string computedStatus = ComputeStatus(hasCheckIn, hasCheckOut);
                    //if (!PassesStatusFilter(computedStatus)) continue;

                    bool eventIsOver = ev.Date.Date < DateTime.Today ||
                    (ev.Date.Date == DateTime.Today && DateTime.Now.TimeOfDay > ev.CheckInEnd);
                    if (!hasCheckIn && !hasCheckOut && !eventIsOver) continue;

                    string computedStatus = ComputeStatus(hasCheckIn, hasCheckOut);
                    if (!PassesStatusFilter(computedStatus)) continue;

                    var checkInRecord = GetFirstCheckInRecord(student.StudentID, student.Year, student.Course, ev);
                    var checkOutRecord = GetFirstCheckOutRecord(student.StudentID, student.Year, student.Course, ev);

                    object[] row =
                    {
                        hasCheckIn  ? checkInRecord!.AttendanceID.ToString("D4")
                                    : hasCheckOut ? checkOutRecord!.AttendanceID.ToString("D4") : "-",
                        student.StudentID ?? "-",
                        student.Course    ?? "-",
                        student.Year      ?? "-",
                        ev.EventName,
                        (hasCheckIn && checkInRecord != null)
                            ? checkInRecord.CheckInTime.ToString("hh:mm:ss tt") : "-",
                        (hasCheckOut && checkOutRecord?.CheckOutTime.HasValue == true)
                            ? checkOutRecord!.CheckOutTime!.Value.ToString("hh:mm:ss tt") : "-",
                        computedStatus,
                        ev.Semester ?? "-"
                    };

                    dgvAttendance.Rows.Add(row);
                }
            }

            UpdateAttendanceCounts();
            ColorCodeAttendanceGrid();  //
        }

        // Status combobox changed: reload event combo and filters
        private void cmbStatusAttend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_attendanceTabReady) return;
            LoadEventComboBox();
            FilterAttendanceGrid();
            UpdateAttendanceCounts();
            RefreshStudentInfoLabels();
        }

        // Search a student by id or name then show their attendance rows
        private void btnSearchAttend_Click(object sender, EventArgs e)
        {
            string searchText = txtSearchAttendance.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText) || searchText == "Search Student ID")
            {
                MessageBox.Show("Please enter a Student ID to search.", "Search",
                                MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }


            Attendance.AttendanceList = AttendanceDB.GetAll();
            generateQR.QRList = StudentDB.GetAll();

            var student = generateQR.QRList?.FirstOrDefault(q =>
                q.StudentID?.Equals(searchText, StringComparison.OrdinalIgnoreCase) == true);
            if (student == null)
                student = generateQR.QRList?.FirstOrDefault(q =>
                    q.StudentName?.Equals(searchText, StringComparison.OrdinalIgnoreCase) == true);

            if (student == null)
            {
                MessageBox.Show("No student found matching that ID.", "Not Found",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ClearStudentInfo(); return;
            }


            _searchedStudentID = null;
            dgvAttendance.Rows.Clear();


            _suppressAttendanceFilter = true;
            cmbYearLevelAttend.SelectedIndex = -1;
            _suppressAttendanceFilter = false;


            lblName.Text = student.StudentName ?? "";
            lblStudentID.Text = student.StudentID ?? "";
            lblCourse.Text = student.Course ?? "";
            lblYearLevel.Text = student.Year ?? "";

            if (!string.IsNullOrWhiteSpace(student.PhotoPath) && File.Exists(student.PhotoPath))
                pictureStudent.Image = Image.FromFile(student.PhotoPath);
            else
                pictureStudent.Image = null;

            string currentSemesterFilter = cmbSelectSemester.SelectedItem?.ToString() ?? "All";
            lblSemester.Text = (!string.IsNullOrWhiteSpace(currentSemesterFilter) && currentSemesterFilter != "All")
                ? currentSemesterFilter : "All";


            _searchedStudentID = student.StudentID;
            FilterAttendanceGrid();

            if (dgvAttendance.Rows.Count == 0)
                MessageBox.Show($"Student \"{student.StudentName}\" was found, but has no attendance records.",
                                "No Matching Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Clears the currently displayed student info and resets search state
        private void ClearStudentInfo()
        {
            _searchedStudentID = null;
            lblName.Text = ""; lblStudentID.Text = ""; lblCourse.Text = "";
            lblYearLevel.Text = ""; lblSemester.Text = "";
            pictureStudent.Image = null;
            dgvAttendance.Rows.Clear();

            _suppressAttendanceFilter = true;
            cmbYearLevelAttend.SelectedIndex = -1;
            _suppressAttendanceFilter = false;
        }

        // Build the reports grid by aggregating attendance records matching the filters
        public void LoadReports()
        {
            dgvReports.Rows.Clear();
            Attendance.AttendanceList = AttendanceDB.GetAll() ?? new List<Attendance>();
            generateQR.QRList = StudentDB.GetAll() ?? new List<generateQR>();
            Event.EventList = EventDB.GetAll() ?? new List<Event>();

            string selectedEvent = cmbEventReports.SelectedItem?.ToString() ?? "All";
            string selectedCourse = cmbCourseReports.SelectedItem?.ToString() ?? "All";
            string selectedYear = cmbYearReports.SelectedItem?.ToString() ?? "All";

            var eventsToShow = Event.EventList
                .Where(ev =>
                {
                    if (ev.Date.Date > DateTime.Today) return false;
                    if (selectedEvent != "All" && ev.EventName != selectedEvent) return false;
                    return true;
                })
                .ToList();

            var scopedStudents = generateQR.QRList
                .Where(q =>
                    (selectedCourse == "All" || string.Equals(q.Course?.Trim(), selectedCourse.Trim(), StringComparison.OrdinalIgnoreCase)) &&
                    (selectedYear == "All" || string.Equals(q.Year?.Trim(), selectedYear.Trim(), StringComparison.OrdinalIgnoreCase)))
                .ToList();

            int totalCheckIn = 0;
            int totalCheckOut = 0;
            int totalAbsent = 0;


            var addedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var ev in eventsToShow)
            {
                foreach (var student in scopedStudents)
                {
                    if (string.IsNullOrWhiteSpace(student.StudentID)) continue;


                    string rowKey = $"{student.StudentID?.Trim().ToLower()}|{student.Year?.Trim().ToLower()}|{student.Course?.Trim().ToLower()}|{ev.EventID}";
                    if (!addedKeys.Add(rowKey)) continue;

                    var checkInRecord = GetFirstCheckInRecord(student.StudentID, student.Year, student.Course, ev);
                    var checkOutRecord = GetFirstCheckOutRecord(student.StudentID, student.Year, student.Course, ev);

                    bool hasCheckIn = checkInRecord != null;
                    bool hasCheckOut = checkOutRecord != null;


                    if (!hasCheckIn && !hasCheckOut) continue;

                   
                    string status = (hasCheckIn, hasCheckOut) switch
                    {
                        (true, true) => "Completed",
                        (true, false) => "Check In",
                        (false, true) => "Late",  
                        _ => "Not Attended"
                    };

                    string checkInDisplay = hasCheckIn ? checkInRecord!.CheckInTime.ToString("hh:mm:ss tt") : "-";
                    string checkOutDisplay = hasCheckOut ? checkOutRecord!.CheckOutTime!.Value.ToString("hh:mm:ss tt") : "-";

                    dgvReports.Rows.Add(
                        student.StudentID ?? "",
                        student.StudentName ?? "",
                        student.Course ?? "",
                        student.Year ?? "",
                        ev.EventName ?? "",
                        ev.Date.ToString("MMMM dd, yyyy"),
                        checkInDisplay,
                        checkOutDisplay,
                        status
                    );

                    if (status == "Completed") { totalCheckIn++; totalCheckOut++; }
                    else if (status == "Check In") totalCheckIn++;
                    else if (status == "Late") totalCheckOut++;  

                    if (!hasCheckIn) totalAbsent++;
                    if (ev.CheckOutStart.HasValue && !hasCheckOut) totalAbsent++;
                }
            }

            ColorCodeReportsGrid();
        }

        // Open export dialog
        private void btnExport_Click(object sender, EventArgs e)
        {
            Export ex = new Export();
            ex.ShowDialog();
        }

        // Populate report-related combo boxes (events, courses, years)
        private void LoadReportsComboBoxes()
        {
            string? previousEvent = cmbEventReports.SelectedItem?.ToString();
            cmbEventReports.Items.Clear();
            cmbEventReports.Items.Add("All");


            foreach (var ev in Event.EventList
                .GroupBy(e => e.EventName)
                .Select(g => g.First()))
                cmbEventReports.Items.Add(ev.EventName);

            if (previousEvent != null && cmbEventReports.Items.Contains(previousEvent))
                cmbEventReports.SelectedItem = previousEvent;
            else
                cmbEventReports.SelectedIndex = 0;

            if (cmbCourseReports.Items.Count == 0)
            {
                cmbCourseReports.Items.Add("All");
                foreach (var course in generateQR.QRList.Select(q => q.Course).Distinct()
                    .Where(c => !string.IsNullOrEmpty(c)).OrderBy(c => c))
                    cmbCourseReports.Items.Add(course);
            }
            if (cmbCourseReports.SelectedIndex == -1) cmbCourseReports.SelectedIndex = 0;

            if (cmbYearReports.Items.Count == 0)
            {
                cmbYearReports.Items.Add("All");
                foreach (var year in generateQR.QRList.Select(q => q.Year).Distinct()
                    .Where(y => !string.IsNullOrEmpty(y)).OrderBy(y => y))
                    cmbYearReports.Items.Add(year);
            }
            if (cmbYearReports.SelectedIndex == -1) cmbYearReports.SelectedIndex = 0;
        }

        // Enable/disable checkout button depending on selected event and time
        public void UpdateCheckoutButton()
        {
            if (_selectedEventID == -1) { btncheckoutStart.Enabled = false; return; }
            var ev = Event.EventList.FirstOrDefault(x => x.EventID == _selectedEventID);
            if (ev == null) { btncheckoutStart.Enabled = false; return; }

            bool shouldEnable = ev.Date.Date == DateTime.Today &&
                                DateTime.Now.TimeOfDay >= ev.CheckInEnd &&
                                !ev.CheckOutStart.HasValue;

            if (btncheckoutStart.Enabled != shouldEnable)
                btncheckoutStart.Enabled = shouldEnable;
        }

        // Handle clicking an event row: select it and set selected id
        private void dgvEvent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            dgvEvent.ClearSelection();
            dgvEvent.Rows[e.RowIndex].Selected = true;
            dgvEvent.CurrentCell = dgvEvent.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (int.TryParse(dgvEvent.Rows[e.RowIndex].Cells[0].Value?.ToString(), out int eventID))
                _selectedEventID = eventID;
            UpdateCheckoutButton();
        }

        // When attendance search text cleared, reset student info
        private void txtSearchAttendance_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchAttendance.ForeColor == Color.Gray) return;
            if (string.IsNullOrWhiteSpace(txtSearchAttendance.Text))
            { ClearStudentInfo(); FilterAttendanceGrid(); }
        }

        // Semester selection changed: refresh filters and UI
        private void cmbSelectSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_attendanceTabReady) return;
            LoadEventComboBox();
            FilterAttendanceGrid();
            UpdateAttendanceCounts();
            RefreshStudentInfoLabels();
        }

        // Report filter change handlers (simple forward)
        private void cmbCourseReports_SelectedIndexChanged(object sender, EventArgs e) => LoadReports();
        private void cmbEventReports_SelectedIndexChanged(object sender, EventArgs e) => LoadReports();
        private void cmbYearReports_SelectedIndexChanged(object sender, EventArgs e) => LoadReports();

        // Calculate and show aggregated attendance counts for the currently displayed rows
        public void UpdateAttendanceCounts()
        {
            if (!_attendanceTabReady) return;

            int checkInCount = 0;
            int checkOutCount = 0;
            int absentCount = 0;

            bool anyCheckoutWindowStartedGlobally = Event.EventList?.Any(ev => ev.CheckOutStart.HasValue) == true;

            foreach (DataGridViewRow row in dgvAttendance.Rows)
            {
                if (row.IsNewRow) continue;

                string status = (row.Cells.Count > 7 ? row.Cells[7].Value?.ToString()?.Trim() : null) ?? "";

                switch (status)
                {
                    case "Completed": checkInCount++; checkOutCount++; break;
                    case "Check In":
                    case "Check-in": checkInCount++; break;
                    case "Late": 
                    case "Check Out":  
                    case "Check-out": 
                        checkOutCount++;
                        break;
                }

                string checkInCell = (row.Cells.Count > 5 ? row.Cells[5].Value?.ToString() ?? "" : "").Trim();
                string checkOutCell = (row.Cells.Count > 6 ? row.Cells[6].Value?.ToString() ?? "" : "").Trim();

                bool hasCheckInCell = !string.IsNullOrEmpty(checkInCell) && checkInCell != "-";
                bool hasCheckOutCell = !string.IsNullOrEmpty(checkOutCell) && checkOutCell != "-";

                string eventName = (row.Cells.Count > 4 ? row.Cells[4].Value?.ToString() ?? "" : "").Trim();
                var evMatch = Event.EventList?.FirstOrDefault(e =>
                    string.Equals(e.EventName?.Trim(), eventName, StringComparison.OrdinalIgnoreCase));
                bool checkoutWindowExistsForEvent = evMatch?.CheckOutStart.HasValue == true;

                int penalty = 0;
                if (!hasCheckInCell) penalty += 1;
                if (checkoutWindowExistsForEvent && !hasCheckOutCell) penalty += 1;
                absentCount += penalty;
            }

            lblTotalCheckinAttend.Text = checkInCount.ToString();
            lblTotalCheckoutAttend.Text = anyCheckoutWindowStartedGlobally ? checkOutCount.ToString() : "-";
            lblTotalAbsentAttend.Text = absentCount.ToString();

            if (!string.IsNullOrWhiteSpace(_searchedStudentID))
            {
                string yearFilter = (cmbYearLevelAttend.SelectedIndex == -1)
                    ? "All"
                    : cmbYearLevelAttend.SelectedItem?.ToString() ?? "All";

                lblCScounted.Text = ComputeStudentCSHours(
                    _searchedStudentID,
                    cmbEventAttend?.SelectedItem?.ToString() ?? "All",
                    cmbSelectSemester?.SelectedItem?.ToString() ?? "All",
                    yearFilter) + " hrs";
            }
            else
            {
                lblCScounted.Text = "-";
            }

        }

        // Compute student penalty/CS hours based on missed check-in/checkout across events
        private string ComputeStudentCSHours(
      string studentID,
      string selectedEvent,
      string selectedSemester,
      string selectedYear)
        {
            if (string.IsNullOrWhiteSpace(studentID)) return "0";

            var matchingStudents = generateQR.QRList
                .Where(q => string.Equals(q.StudentID?.Trim(), studentID, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (selectedYear != "All")
                matchingStudents = matchingStudents
                    .Where(q => string.Equals(q.Year?.Trim(), selectedYear.Trim(), StringComparison.OrdinalIgnoreCase))
                    .ToList();

            var eventsToCheck = GetEventsToEvaluate(selectedEvent, selectedSemester);
            int totalPenaltyHours = 0;

            foreach (var student in matchingStudents)
            {
                foreach (var ev in eventsToCheck)
                {
                    bool eventIsOver = ev.Date.Date < DateTime.Today ||
                                       (ev.Date.Date == DateTime.Today &&
                                        DateTime.Now.TimeOfDay > ev.CheckInEnd);
                    if (!eventIsOver) continue;

                    bool hasCheckIn = HasCheckInForStudentEvent(student.StudentID, student.Year, student.Course, ev);
                    bool hasCheckOut = HasCheckOutForStudentEvent(student.StudentID, student.Year, student.Course, ev);
                    bool checkoutExists = ev.CheckOutStart.HasValue;

                    if (!hasCheckIn && !hasCheckOut && checkoutExists)
                        totalPenaltyHours += 2;   
                    else if (!hasCheckIn)
                        totalPenaltyHours += 1;  
                    else if (checkoutExists && !hasCheckOut)
                        totalPenaltyHours += 1;   
                }
            }

            return totalPenaltyHours.ToString();
        }

        // Empty selection changed handler for attendance grid (kept for compatibility)
        private void dgvAttendance_SelectionChanged(object sender, EventArgs e) { }

        // Year-level filter changed: re-filter and update displayed student year if a student was searched
        private void cmbYearLevelAttend_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_attendanceTabReady) return;
            FilterAttendanceGrid();

            if (string.IsNullOrWhiteSpace(_searchedStudentID)) return;

            string selectedYear = cmbYearLevelAttend.SelectedItem?.ToString() ?? "";


            if (!string.IsNullOrWhiteSpace(selectedYear))
            {
                lblYearLevel.Text = selectedYear;
            }
            else
            {

                var student = generateQR.QRList?.FirstOrDefault(q =>
                    string.Equals(q.StudentID?.Trim(), _searchedStudentID, StringComparison.OrdinalIgnoreCase));
                lblYearLevel.Text = student?.Year ?? "";
            }
            RefreshStudentInfoLabels();
        }

        // Configure DataGridView styles used across the form
        private void setData()
        {
            Font dgvFont = new Font("Segoe UI", 9f, FontStyle.Regular);

            void ApplyDGVStyle(DataGridView dgv, bool allowSelection)
            {
                dgv.Font = dgvFont;
                dgv.DefaultCellStyle.Font = dgvFont;
                dgv.ColumnHeadersDefaultCellStyle.Font = dgvFont;
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgv.ColumnHeadersDefaultCellStyle.BackColor;
                dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgv.ColumnHeadersDefaultCellStyle.ForeColor;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgv.ColumnHeadersVisible = true;
                dgv.ReadOnly = true;
                dgv.AllowUserToAddRows = false;
                dgv.RowHeadersVisible = false;
                dgv.AutoGenerateColumns = false;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;  // Default
                dgv.MultiSelect = false;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.ColumnHeadersHeight = 40;
                foreach (DataGridViewColumn col in dgv.Columns)
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;

                if (!allowSelection)
                {
                    dgv.DefaultCellStyle.SelectionBackColor = dgv.DefaultCellStyle.BackColor;
                    dgv.DefaultCellStyle.SelectionForeColor = dgv.DefaultCellStyle.ForeColor;
                    dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;  // <-- ADD THIS LINE
                }
            }

            ApplyDGVStyle(dgvHome, allowSelection: false);
            ApplyDGVStyle(dgvAttendance, allowSelection: false);
            ApplyDGVStyle(dgvReports, allowSelection: false);
            ApplyDGVStyle(dgvEvent, allowSelection: true);


            dgvHome.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvHome.DefaultCellStyle.SelectionBackColor = dgvHome.DefaultCellStyle.BackColor;
            dgvHome.DefaultCellStyle.SelectionForeColor = dgvHome.DefaultCellStyle.ForeColor;
            dgvHome.CellClick += (s, ev) => dgvHome.ClearSelection();
            dgvHome.RowEnter += (s, ev) => dgvHome.ClearSelection();

            dgvEvent.DefaultCellStyle.SelectionBackColor = Color.FromArgb(13, 71, 161);
            dgvEvent.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvAttendance.AllowUserToAddRows = false;
            dgvAttendance.ReadOnly = true;
            dgvAttendance.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAttendance.MultiSelect = false;
        }


        // Secondary search handler variant (kept for compatibility)
        private void txtSearchEvent_TextChanged_1(object sender, EventArgs e)
        {
            if (txtSearchEvent.ForeColor == Color.Gray) return;
            if (string.IsNullOrWhiteSpace(txtSearchEvent.Text)) LoadEvents();
        }


        // Recalculate totals (check-ins/outs) for every event based on attendance records
        public void RecalculateAllEventTotals()
        {
            foreach (var ev in Event.EventList)
            {
                int checkInCount = 0;
                int checkOutCount = 0;

                foreach (var student in generateQR.QRList)
                {
                    if (string.IsNullOrWhiteSpace(student.StudentID)) continue;

                    bool hasAnyRecord = Attendance.AttendanceList.Any(a =>
                        string.Equals(a.StudentID?.Trim(), student.StudentID?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(a.EventName?.Trim(), ev.EventName?.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (!hasAnyRecord) continue;

                    if (HasCheckInForStudentEvent(student.StudentID, student.Year, student.Course, ev)) checkInCount++;
                    if (HasCheckOutForStudentEvent(student.StudentID, student.Year, student.Course, ev)) checkOutCount++;
                }

                ev.TotalCheckIns = checkInCount;
                ev.TotalCheckOuts = checkOutCount;
                EventDB.Update(ev);
            }
        }

        // Returns attendance records for a given student/event constrained to valid check-in/checkout window times
        private List<Attendance> GetAttendanceRecordsForStudentEvent(
    string? studentID, string? year, string? course, Event ev)
        {
            if (string.IsNullOrWhiteSpace(studentID) || ev == null)
                return new List<Attendance>();


            return Attendance.AttendanceList
                .Where(a =>
                    string.Equals(a.StudentID?.Trim(), studentID.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(a.Year?.Trim(), year?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(a.Course?.Trim(), course?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(a.EventName?.Trim(), ev.EventName?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    (

                        (a.CheckInTime != default &&
                         a.CheckInTime.Date == ev.Date.Date &&
                         a.CheckInTime.TimeOfDay >= ev.CheckInStart &&
                         a.CheckInTime.TimeOfDay <= ev.CheckInEnd)
                        ||

                        (a.CheckOutTime.HasValue &&
                         a.CheckOutTime!.Value.Date == ev.Date.Date &&
                         ev.CheckOutStart.HasValue &&
                         (
                            (ev.CheckOutEnd.HasValue &&
                             a.CheckOutTime!.Value.TimeOfDay >= ev.CheckOutStart.Value &&
                             a.CheckOutTime!.Value.TimeOfDay <= ev.CheckOutEnd.Value)
                            ||
                            (!ev.CheckOutEnd.HasValue &&
                             a.CheckOutTime!.Value.TimeOfDay >= ev.CheckOutStart.Value)
                         ))
                    )
                )
                .ToList();
        }



        // Returns the first valid check-in record for the student/event (earliest)
        private Attendance? GetFirstCheckInRecord(
  string? studentID, string? year, string? course, Event ev)
        {
            var records = GetAttendanceRecordsForStudentEvent(studentID, year, course, ev);

            return records
                .Where(a => a.CheckInTime != default &&
                            a.CheckInTime.Date == ev.Date.Date &&
                            a.CheckInTime.TimeOfDay >= ev.CheckInStart &&
                            a.CheckInTime.TimeOfDay <= ev.CheckInEnd)
                .OrderBy(a => a.CheckInTime)
                .FirstOrDefault();
        }

        // Returns the first valid check-out record for the student/event (earliest within checkout window)
        private Attendance? GetFirstCheckOutRecord(
         string? studentID, string? year, string? course, Event ev)
        {
            var records = GetAttendanceRecordsForStudentEvent(studentID, year, course, ev);
            var withCheckOut = records.Where(a => a.CheckOutTime.HasValue && a.CheckOutTime!.Value.Date == ev.Date.Date).ToList();
            if (!withCheckOut.Any()) return null;

            if (ev.CheckOutStart.HasValue && ev.CheckOutEnd.HasValue)
            {
                return withCheckOut
                    .Where(a =>
                        a.CheckOutTime!.Value.TimeOfDay >= ev.CheckOutStart.Value &&
                        a.CheckOutTime!.Value.TimeOfDay <= ev.CheckOutEnd.Value)
                    .OrderBy(a => a.CheckOutTime)
                    .FirstOrDefault();
            }

            if (ev.CheckOutStart.HasValue)
            {
                return withCheckOut
                    .Where(a => a.CheckOutTime!.Value.TimeOfDay >= ev.CheckOutStart.Value)
                    .OrderBy(a => a.CheckOutTime)
                    .FirstOrDefault();
            }

            return null;
        }


        // Convenience predicates for checking existence of check-in/out
        private bool HasCheckInForStudentEvent(string? studentID, string? year, string? course, Event ev)
            => GetFirstCheckInRecord(studentID, year, course, ev) != null;

        private bool HasCheckOutForStudentEvent(string? studentID, string? year, string? course, Event ev)
            => GetFirstCheckOutRecord(studentID, year, course, ev) != null;

        // Enter key in event search triggers search
        private void txtSearchEvent_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSearchEvent_Click(sender, e);
            }
        }

        // Enter key in attendance search triggers search
        private void txtSearchAttendance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSearchAttend_Click(sender, e);
            }
        }

        // Placeholder handling for student name textbox
        private void txtStudentName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentName.Text))
            {
                txtStudentName.Text = "Enter Student Name";
                txtStudentName.ForeColor = Color.Gray;
            }
        }

        private void txtStudentName_Enter(object sender, EventArgs e)
        {
            if (txtStudentName.Text == "Enter Student Name")
            {
                txtStudentName.Text = "";
                txtStudentName.ForeColor = Color.Black;
            }
        }

        // Placeholder handling for student id textbox
        private void txtStudentID_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text))
            {
                txtStudentID.Text = "Enter Student ID";
                txtStudentID.ForeColor = Color.Gray;
            }
        }

        private void txtStudentID_Enter(object sender, EventArgs e)
        {
            if (txtStudentID.Text == "Enter Student ID")
            {
                txtStudentID.Text = "";
                txtStudentID.ForeColor = Color.Black;
            }
        }
        // Sets owner-drawn placeholder text for combo boxes
        private void ApplyPlaceholder(ComboBox cmb, string placeholder)
        {
            cmb.DrawMode = DrawMode.OwnerDrawFixed;
            cmb.Tag = placeholder;
            cmb.DrawItem += Cmb_DrawItem;
            cmb.SelectedIndex = -1;
        }

        // DrawItem handler used by placeholder combo boxes (renders placeholder when no selection)
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

        // Refresh displayed labels for the currently searched student
        private void RefreshStudentInfoLabels()
        {
            if (string.IsNullOrWhiteSpace(_searchedStudentID)) return;


            var student = generateQR.QRList?.FirstOrDefault(q =>
                string.Equals(q.StudentID?.Trim(), _searchedStudentID, StringComparison.OrdinalIgnoreCase));

            if (student == null) return;


            lblName.Text = student.StudentName ?? "";
            lblStudentID.Text = student.StudentID ?? "";
            lblCourse.Text = student.Course ?? "";


            string selectedYear = cmbYearLevelAttend.SelectedItem?.ToString() ?? "";
            lblYearLevel.Text = !string.IsNullOrWhiteSpace(selectedYear)
                ? selectedYear
                : student.Year ?? "";


            string selectedSemester = cmbSelectSemester.SelectedItem?.ToString() ?? "All";
            lblSemester.Text = (!string.IsNullOrWhiteSpace(selectedSemester) && selectedSemester != "All")
                ? selectedSemester
                : "All";


            string yearForCS = (cmbYearLevelAttend.SelectedIndex == -1)
    ? "All"
    : cmbYearLevelAttend.SelectedItem?.ToString() ?? "All";

            lblCScounted.Text = ComputeStudentCSHours(
                _searchedStudentID,
                cmbEventAttend?.SelectedItem?.ToString() ?? "All",
                cmbSelectSemester?.SelectedItem?.ToString() ?? "All",
                yearForCS) + " hrs";

            try
            {
                if (!string.IsNullOrWhiteSpace(student.PhotoPath) && File.Exists(student.PhotoPath))
                {
                    pictureStudent.Image?.Dispose();
                    pictureStudent.Image = Image.FromFile(student.PhotoPath);
                }
                else
                {
                    pictureStudent.Image?.Dispose();
                    pictureStudent.Image = null;
                }
            }
            catch { pictureStudent.Image = null; }
        }

        // Allow only digits in student ID textbox
        private void txtStudentID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Apply coloring to attendance rows according to status text
        private void ColorCodeAttendanceGrid()
        {
            foreach (DataGridViewRow row in dgvAttendance.Rows)
            {
                if (row.IsNewRow) continue;
                string status = row.Cells[7].Value?.ToString()?.Trim() ?? "";
                ApplyAttendanceRowColor(row, status);
            }
        }

        // Apply coloring to reports rows according to status text
        private void ColorCodeReportsGrid()
        {
            foreach (DataGridViewRow row in dgvReports.Rows)
            {
                if (row.IsNewRow) continue;
                string status = row.Cells[8].Value?.ToString()?.Trim() ?? "";
                ApplyAttendanceRowColor(row, status);
            }
        }

        // Apply row coloring for events grid based on event status
        private void ColorCodeEventsGrid(DataGridView dgv)
        {

            Color selectionBack = Color.FromArgb(13, 71, 161);
            Color selectionFore = Color.White;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;


                string status = row.Cells[10].Value?.ToString()?.Trim() ?? "";

                (Color back, Color fore) = status switch
                {
                    "Ongoing" => (Color.FromArgb(232, 245, 233), Color.FromArgb(27, 94, 32)),   // Green
                    "Finished" => (Color.FromArgb(227, 242, 253), Color.FromArgb(13, 71, 161)),  // Blue
                    "Upcoming" => (Color.FromArgb(255, 253, 231), Color.FromArgb(130, 100, 0)),  // Yellow
                    _ => (Color.White, Color.Black)
                };

                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = back;
                    cell.Style.ForeColor = fore;

                    bool isHome = (dgv == dgvHome);
                    cell.Style.SelectionBackColor = isHome ? back : selectionBack;
                    cell.Style.SelectionForeColor = isHome ? fore : selectionFore;
                }
            }
        }

        // Apply consistent attendance-status-based colors to a data grid row
        private void ApplyAttendanceRowColor(DataGridViewRow row, string status)
        {
            (Color back, Color fore) = status switch
            {
                "Completed" => (Color.FromArgb(232, 245, 233), Color.FromArgb(27, 94, 32)),   // Green
                "Check In" => (Color.FromArgb(255, 253, 231), Color.FromArgb(130, 100, 0)),  // Yellow
                "Check-in" => (Color.FromArgb(255, 253, 231), Color.FromArgb(130, 100, 0)),  // Yellow
                "Late" => (Color.FromArgb(227, 242, 253), Color.FromArgb(13, 71, 161)),
                "Check Out" => (Color.FromArgb(227, 242, 253), Color.FromArgb(13, 71, 161)),  // Blue
                "Check-out" => (Color.FromArgb(227, 242, 253), Color.FromArgb(13, 71, 161)),  // Blue
                "Not Attended" => (Color.FromArgb(255, 235, 238), Color.FromArgb(183, 28, 28)),  // Red
                _ => (Color.White, Color.Black)
            };

            foreach (DataGridViewCell cell in row.Cells)
            {
                cell.Style.BackColor = back;
                cell.Style.ForeColor = fore;
                cell.Style.SelectionBackColor = back;
                cell.Style.SelectionForeColor = fore;
            }
        }

        // Double-click an event row to open edit dialog (unless ongoing)
        private void dgvEvent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            dgvEvent.ClearSelection();
            dgvEvent.Rows[e.RowIndex].Selected = true;
            dgvEvent.CurrentCell = dgvEvent.Rows[e.RowIndex].Cells[0];

            if (!int.TryParse(dgvEvent.Rows[e.RowIndex].Cells[0].Value?.ToString(), out int eventID))
            { MessageBox.Show("Invalid Event ID", "Error"); return; }

            var selectedEvent = Event.EventList.FirstOrDefault(ev => ev.EventID == eventID);
            if (selectedEvent == null) return;

            string status = selectedEvent.GetStatus();
            if (status == "Ongoing")
            {
                MessageBox.Show("Cannot edit an ongoing event.", "Blocked",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            Event_Details editForm = new Event_Details { Owner = this };
            editForm.LoadEventForEdit(selectedEvent);
            if (status == "Finished") editForm.SetFinishedMode();
            editForm.ShowDialog();
            LoadEvents();
        }

        // Reserved double-click content handler (no-op)
        private void dgvEvent_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // Expose the reports grid for export/printing
        public DataGridView GetReportsGrid()
        {
            return dgvReports;
        }
    }
}
