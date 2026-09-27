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
using ClosedXML.Excel;
using System.IO;

namespace FinalProject_EventAttendance02
{
    public partial class Export : Form
    {
        private List<Attendance> _reportData = new List<Attendance>();
        private Event? _selectedEvent = null;

        private string _filterEvent = "All";
        private string _filterCourse = "All";
        private string _filterYear = "All";

        public Export()
        {
            InitializeComponent();
        }

        private void Export_Load(object sender, EventArgs e)
        {
            cmbEventExport.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCourseExport.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbYearExport.DropDownStyle = ComboBoxStyle.DropDownList;

            if (cmbCourseExport.Items.Count == 0)
            {
                cmbCourseExport.Items.Add("All");
                foreach (var course in generateQR.QRList
                    .Select(q => q.Course)
                    .Distinct()
                    .Where(c => !string.IsNullOrEmpty(c)))
                    cmbCourseExport.Items.Add(course);
            }
            if (cmbCourseExport.SelectedIndex == -1)
                cmbCourseExport.SelectedIndex = 0;

            if (cmbYearExport.Items.Count == 0)
            {
                cmbYearExport.Items.Add("All");
                foreach (var year in generateQR.QRList
                    .Select(q => q.Year)
                    .Distinct()
                    .Where(y => !string.IsNullOrEmpty(y)))
                    cmbYearExport.Items.Add(year);
            }
            if (cmbYearExport.SelectedIndex == -1)
                cmbYearExport.SelectedIndex = 0;

            LoadEventExportComboBox();

            dgvExport.AutoGenerateColumns = false;
            dgvExport.AllowUserToAddRows = false;
            dgvExport.ReadOnly = true;
            dgvExport.RowHeadersVisible = false;
            dgvExport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }


        //private static bool IsAttended(string? status)
        //{
        //    string s = (status ?? "").Trim().ToLower();
        //    return s == "check in" || s == "check-in" || s == "checked in" ||
        //           s == "late" ||  // CHANGED: Added "late"
        //           s == "check out" || s == "check-out" || s == "checked out" ||
        //           s == "completed";
        //}

        private static bool IsCheckIn(string? status)
        {
            string s = (status ?? "").Trim().ToLower();
            return s == "check in" || s == "check-in" || s == "checked in" || s == "completed";
        }

        private static bool IsCheckOut(string? status)
        {
            string s = (status ?? "").Trim().ToLower();
            return s == "late" ||
                   s == "check out" || s == "check-out" || s == "checked out" ||
                   s == "completed";
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Admin ad = new Admin();
            ad.Show();
            this.Hide();
        }

        private void Export_Shown(object sender, EventArgs e)
        {
            Color normal = ColorTranslator.FromHtml("#FFFBF1");
            Color hover = ColorTranslator.FromHtml("#DED3B0");

            CurveButton(btnGenerateReport);
            CurveButton(btnExport1);
            CurvePanel(filterExport, 25);
            StyleFlatButton(btnGenerateReport);
            StyleFlatButton(btnExport1);
            AddHoverEffect(btnGenerateReport, normal, hover);
            AddHoverEffect(btnExport1, normal, hover);
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
            btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
            btn.MouseLeave += (s, e) => btn.BackColor = normalColor;
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

     

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            _filterEvent = cmbEventExport.SelectedItem?.ToString() ?? "All";
            _filterCourse = cmbCourseExport.SelectedItem?.ToString() ?? "All";
            _filterYear = cmbYearExport.SelectedItem?.ToString() ?? "All";

           
            Admin? adminForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is Admin admin)
                {
                    adminForm = admin;
                    break;
                }
            }

            if (adminForm == null)
            {
                MessageBox.Show("Admin form not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

          
            _reportData = new List<Attendance>();

            
            DataGridView dgvReports = adminForm.dgvReports;

            foreach (DataGridViewRow row in dgvReports.Rows)
            {
                if (row.IsNewRow) continue;

                string studentID = row.Cells[0].Value?.ToString() ?? "";
                string studentName = row.Cells[1].Value?.ToString() ?? "";
                string course = row.Cells[2].Value?.ToString() ?? "";
                string year = row.Cells[3].Value?.ToString() ?? "";
                string eventName = row.Cells[4].Value?.ToString() ?? "";
                string dateStr = row.Cells[5].Value?.ToString() ?? "";
                string checkInStr = row.Cells[6].Value?.ToString() ?? "-";
                string checkOutStr = row.Cells[7].Value?.ToString() ?? "-";
                string status = row.Cells[8].Value?.ToString() ?? "";

              
                if (_filterEvent != "All" && eventName != _filterEvent) continue;
                if (_filterCourse != "All" && !string.Equals(course?.Trim(), _filterCourse.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                if (_filterYear != "All" && !string.Equals(year?.Trim(), _filterYear.Trim(), StringComparison.OrdinalIgnoreCase)) continue;

              
                if (status == "Not Attended") continue;

                
                DateTime eventDate = DateTime.TryParse(dateStr, out DateTime parsedDate) ? parsedDate : DateTime.Today;
                DateTime checkInTime = checkInStr != "-" && DateTime.TryParse(checkInStr, out DateTime parsedCheckIn)
                    ? parsedCheckIn : default;
                DateTime? checkOutTime = checkOutStr != "-" && DateTime.TryParse(checkOutStr, out DateTime parsedCheckOut)
                    ? parsedCheckOut : null;

               
                var ev = Event.EventList.FirstOrDefault(x => x.EventName == eventName);
                string semester = ev?.Semester ?? "";

                _reportData.Add(new Attendance
                {
                    AttendanceID = 0, 
                    StudentID = studentID,
                    StudentName = studentName,
                    Course = course ?? string.Empty,
                    Year = year?? string.Empty,
                    EventName = eventName,
                    CheckInTime = checkInTime,
                    CheckOutTime = checkOutTime,
                    Status = status,
                    Semester = semester
                });
            }

            _selectedEvent = _filterEvent != "All"
                ? Event.EventList.FirstOrDefault(ev => ev.EventName == _filterEvent)
                : null;

            lblEventExport.Text = _filterEvent != "All" ? _filterEvent : "All Events";
            lblDateExport.Text = _selectedEvent != null
                ? _selectedEvent.Date.ToString("MMMM dd, yyyy") : "All Dates";

            var semesters = _reportData
                .Select(a => Event.EventList.FirstOrDefault(ev => ev.EventName == a.EventName)?.Semester)
                .Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            lblSemesterExport.Text = semesters.Count == 0 ? "N/A"
                                   : semesters.Count == 1 ? semesters[0]!
                                   : "All Semesters";

            if (_reportData.Count == 0)
            {
                dgvExport.Rows.Clear();
                MessageBox.Show("No records found for the selected filters.", "No Data",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            PopulateExportGrid();
        }
        private void btnExport1_Click(object sender, EventArgs e)
        {
            if (_reportData == null || _reportData.Count == 0)
            {
                MessageBox.Show("Please generate a report first before exporting.",
                                "No Report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                FileName = $"AttendanceReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Attendance Report");

                string eventName = _selectedEvent?.EventName ?? "All Events";
                string eventDate = _selectedEvent != null ? _selectedEvent.Date.ToString("MMMM dd, yyyy") : "All Dates";
                var semesters = _reportData
                    .Select(a => Event.EventList.FirstOrDefault(ev => ev.EventName == a.EventName)?.Semester)
                    .Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                string semLabel = semesters.Count == 1 ? semesters[0]! : semesters.Count == 0 ? "N/A" : "All Semesters";

                ws.Cell(1, 1).Value = "Event Name:"; ws.Cell(1, 1).Style.Font.Bold = true; ws.Cell(1, 2).Value = eventName;
                ws.Cell(2, 1).Value = "Date:"; ws.Cell(2, 1).Style.Font.Bold = true; ws.Cell(2, 2).Value = eventDate;
                ws.Cell(3, 1).Value = "Semester:"; ws.Cell(3, 1).Style.Font.Bold = true; ws.Cell(3, 2).Value = semLabel;

                string[] headers = { "Student ID", "Student Name", "Course", "Year", "Event", "Date", "Check-In", "Check-Out", "Status" };

                void StyleHeader(IXLCell cell, string text)
                {
                    cell.Value = text;
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#2E4057");
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                void StyleSectionTitle(IXLCell cell, string text, XLColor color)
                {
                    cell.Value = text;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontSize = 11;
                    cell.Style.Fill.BackgroundColor = color;
                    cell.Style.Font.FontColor = XLColor.White;
                }

                void StyleCourseTitle(IXLCell cell, string text, XLColor color)
                {
                    cell.Value = text;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontSize = 10;
                    cell.Style.Fill.BackgroundColor = color;
                    cell.Style.Font.FontColor = XLColor.White;
                }

                void WriteDataRow(IXLWorksheet sheet, int r, Attendance att)
                {
                    var ev = Event.EventList.FirstOrDefault(x => x.EventName == att.EventName);
                    sheet.Cell(r, 1).Value = att.StudentID ?? "";
                    sheet.Cell(r, 2).Value = att.StudentName ?? "";
                    sheet.Cell(r, 3).Value = att.Course ?? "";
                    sheet.Cell(r, 4).Value = att.Year ?? "";
                    sheet.Cell(r, 5).Value = att.EventName ?? "";
                    sheet.Cell(r, 6).Value = ev != null ? ev.Date.ToString("MMMM dd, yyyy") : "-";
                    sheet.Cell(r, 7).Value = att.CheckInTime == default ? "-" : att.CheckInTime.ToString("hh:mm:ss tt");
                    sheet.Cell(r, 8).Value = att.CheckOutTime.HasValue ? att.CheckOutTime.Value.ToString("hh:mm:ss tt") : "-";
                    sheet.Cell(r, 9).Value = att.Status ?? "";
                }

                // Only attended records needed now
                var attended = _reportData.Where(a => a.Status != "Not Attended").ToList();

                int grandCheckIn = 0;
                int grandCheckOut = 0;

                int row = 5;

                // ════════════════════════════════════════════
                // ATTENDED - GROUPED BY COURSE (NOT YEAR)
                // ════════════════════════════════════════════
                StyleSectionTitle(ws.Cell(row, 1), "ATTENDED", XLColor.FromHtml("#2E7D32"));
                ws.Range(row, 1, row, headers.Length).Merge();
                row++;

                // CHANGED: Group by Course instead of Year
                var attendedByCourse = attended
                    .GroupBy(a => a.Course ?? "Unknown Course")
                    .OrderBy(g => g.Key)
                    .ToList();

                foreach (var courseGroup in attendedByCourse)
                {
                    string courseLabel = courseGroup.Key;
                    var courseRows = courseGroup.ToList();

                    // CHANGED: Course title instead of Year title
                    StyleCourseTitle(ws.Cell(row, 1), $"Course: {courseLabel}", XLColor.FromHtml("#4A7C59"));
                    ws.Range(row, 1, row, headers.Length).Merge();
                    row++;

                    for (int i = 0; i < headers.Length; i++) StyleHeader(ws.Cell(row, i + 1), headers[i]);
                    row++;

                    foreach (var att in courseRows) { WriteDataRow(ws, row, att); row++; }

                    // CHANGED: Subtotal by Course
                    int courseIn = courseRows.Count(a => IsCheckIn(a.Status));
                    int courseOut = courseRows.Count(a => IsCheckOut(a.Status));  // CHANGED: IsCheckOut now recognizes "Late"
                    grandCheckIn += courseIn;
                    grandCheckOut += courseOut;

                    ws.Cell(row, 1).Value = $"Subtotal — {courseLabel}";
                    ws.Cell(row, 1).Style.Font.Bold = true;
                    ws.Cell(row, 2).Value = $"Check-In: {courseIn}";
                    ws.Cell(row, 3).Value = $"Check-Out: {courseOut}";
                    foreach (int c in new[] { 1, 2, 3 })
                        ws.Cell(row, c).Style.Fill.BackgroundColor = XLColor.FromHtml("#E8F5E9");
                    row += 2;
                }

                // ════════════════════════════════════════════
                // GRAND SUMMARY
                // ════════════════════════════════════════════
                StyleSectionTitle(ws.Cell(row, 1), "GRAND SUMMARY", XLColor.FromHtml("#1565C0"));
                ws.Range(row, 1, row, headers.Length).Merge();
                row++;

                void WriteTotal(int r, string label, int value)
                {
                    ws.Cell(r, 1).Value = label; ws.Cell(r, 1).Style.Font.Bold = true;
                    ws.Cell(r, 2).Value = value;
                }

                WriteTotal(row++, "Total Check-In:", grandCheckIn);
                WriteTotal(row++, "Total Check-Out:", grandCheckOut);

                ws.Columns().AdjustToContents();
                workbook.SaveAs(sfd.FileName);

                MessageBox.Show($"Report exported successfully!\n{sfd.FileName}",
                                "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void LoadEventExportComboBox()
        {
            cmbEventExport.Items.Clear();
            cmbEventExport.Items.Add("All");

            foreach (var ev in Event.EventList
                .GroupBy(e => e.EventName)
                .Select(g => g.First()))
                cmbEventExport.Items.Add(ev.EventName);

            cmbEventExport.SelectedIndex = 0;
        }


        private void PopulateExportGrid()
        {
            dgvExport.Rows.Clear();

            foreach (var att in _reportData)
            {
                if (att == null) continue;

                var ev = Event.EventList.FirstOrDefault(x => x.EventName == att.EventName);

                int idx = dgvExport.Rows.Add(
                    att.StudentID ?? "",
                    att.StudentName ?? "",
                    att.Course ?? "",
                    att.Year ?? "",
                    att.EventName ?? "",
                    ev != null ? ev.Date.ToString("MMMM dd, yyyy") : "-",
                    att.CheckInTime == default ? "-" : att.CheckInTime.ToString("hh:mm:ss tt"),
                    att.CheckOutTime.HasValue ? att.CheckOutTime.Value.ToString("hh:mm:ss tt") : "-",
                    att.Status ?? "",
                    att.Semester ?? ""
                );


                var row = dgvExport.Rows[idx];

               
                (Color back, Color fore) = att.Status switch
                {
                    "Completed" => (Color.FromArgb(232, 245, 233), Color.FromArgb(27, 94, 32)),
                    "Check In" => (Color.FromArgb(255, 253, 231), Color.FromArgb(130, 100, 0)),
                    "Late" => (Color.FromArgb(227, 242, 253), Color.FromArgb(13, 71, 161)),  
                    "Check Out" => (Color.FromArgb(227, 242, 253), Color.FromArgb(13, 71, 161)),  // Keep for backward compatibility
                    "Not Attended" => (Color.FromArgb(255, 235, 238), Color.FromArgb(183, 28, 28)),
                    _ => (Color.White, Color.Black)
                };
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.BackColor = back;
                    cell.Style.ForeColor = fore;
                }
            }
        }
    }
}