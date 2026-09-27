using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject_EventAttendance02
{
    public partial class ViewQR : Form
    {
        private generateQR generateQR;
        public ViewQR()
        {
            InitializeComponent();
            generateQR = new generateQR();
        }

        private void ViewQR_Load(object sender, EventArgs e)
        {

            Font dgvFont = new Font("Segoe UI", 9f, FontStyle.Regular);
            dgvGenerateQR.Font = dgvFont;
            dgvGenerateQR.DefaultCellStyle.Font = dgvFont;
            dgvGenerateQR.ColumnHeadersDefaultCellStyle.Font = dgvFont;
            dgvGenerateQR.EnableHeadersVisualStyles = false;
            dgvGenerateQR.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            foreach (DataGridViewColumn col in dgvGenerateQR.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;


            dgvGenerateQR.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGenerateQR.MultiSelect = false;
            dgvGenerateQR.ReadOnly = true;
            dgvGenerateQR.RowHeadersVisible = false;
            dgvGenerateQR.AllowUserToAddRows = false;


            if (dgvGenerateQR.Columns["colPhoto"] is DataGridViewImageColumn imgCol)
            {
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }


            dgvGenerateQR.RowTemplate.Height = 100;

            var allStudents = generateQR.GetAll();
            LoadQRData();
            dgvGenerateQR.ClearSelection();


            this.MinimizeBox = false;
            this.MaximizeBox = false;


        }

        private void ViewQR_Shown(object sender, EventArgs e)
        {
            Color normal = ColorTranslator.FromHtml("#FFFBF1"); // white
            Color hover = ColorTranslator.FromHtml("#DED3B0"); // slightly darker

            CurvePanel(filterHistory, 25);
            CurvePanel(QRPanel, 25);

            CurveButton(btnDeleteQR);
            CurveButton(btnDownloadQR);
            CurveButton(btnSearchQR);

            StyleFlatButton(btnDeleteQR);
            StyleFlatButton(btnDownloadQR);
            StyleFlatButton(btnSearchQR);

            AddHoverEffect(btnDeleteQR, normal, hover);
            AddHoverEffect(btnDownloadQR, normal, hover);
            AddHoverEffect(btnSearchQR, normal, hover);
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

        public void LoadQRData()
        {
            dgvGenerateQR.Rows.Clear();

            try
            {

                var allStudents = StudentDB.GetAll();

                foreach (var qr in allStudents)
                {
                    dgvGenerateQR.Rows.Add(
                        qr.StudentID ?? "",
                        qr.StudentName ?? "",
                        qr.Course ?? "",
                        qr.Year ?? "",
                        LoadStudentPhoto(qr.PhotoPath)
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading QR data:\n" + ex.Message, "Load Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteQR_Click(object sender, EventArgs e)
        {
            if (dgvGenerateQR.CurrentRow == null || dgvGenerateQR.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Please select a QR to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string? studentID = dgvGenerateQR.CurrentRow.Cells[0]?.Value?.ToString()?.Trim();
            string? yearLevel = dgvGenerateQR.CurrentRow.Cells[3]?.Value?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(studentID))
            {
                MessageBox.Show("Invalid Student ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"Delete QR for Student ID: {studentID} ({yearLevel})?\n\nThis will also remove linked attendance records.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {

                int rows = StudentDB.Delete(studentID, yearLevel ?? "");

                if (rows > 0)
                {

                    LoadQRData();

                    if (Admin.Instance != null)
                    {
                        Admin.Instance.RefreshAllDataFromDB();
                        Admin.Instance.FilterAttendanceGrid();
                        Admin.Instance.UpdateAttendanceCounts();
                        Admin.Instance.LoadReports();
                    }

                    MessageBox.Show("QR and linked records deleted successfully!", "Success");
                }
                else
                {
                    MessageBox.Show("Record not found or already deleted.", "Not Found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDownloadQR_Click(object sender, EventArgs e)
        {
            if (dgvGenerateQR.CurrentRow == null || dgvGenerateQR.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Please select a QR first.", "No Selection");
                return;
            }

            string? studentID = dgvGenerateQR.CurrentRow.Cells[0]?.Value?.ToString()?.Trim();
            string? yearLevel = dgvGenerateQR.CurrentRow.Cells[3]?.Value?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(studentID))
            {
                MessageBox.Show("Invalid Student ID.", "Error");
                return;
            }

            try
            {

                var allStudents = StudentDB.GetAll();
                var qrData = allStudents.FirstOrDefault(q =>
                    q.StudentID == studentID &&
                    (string.IsNullOrWhiteSpace(yearLevel) || q.Year == yearLevel));

                if (qrData == null)
                {
                    MessageBox.Show("QR data not found.", "Not Found");
                    return;
                }

                if (string.IsNullOrWhiteSpace(qrData.QRCodeData))
                {
                    MessageBox.Show("This student has no saved QR code data. Please regenerate and save the QR first.",
                                    "No QR Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                using SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "PNG Image|*.png",
                    FileName = $"{studentID}_QR.png",
                    Title = "Save QR Code"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using QRCodeGenerator generator = new QRCodeGenerator();
                    using QRCodeData data = generator.CreateQrCode(qrData.QRCodeData, QRCodeGenerator.ECCLevel.Q);
                    using QRCode qr = new QRCode(data);
                    using Bitmap qrImage = qr.GetGraphic(10);

                    string schoolID = qrData.StudentID ?? "";
                    string name = qrData.StudentName ?? "";
                    string year = qrData.Year ?? "";

                    using Font valueFont = new Font("Segoe UI", 11f, FontStyle.Regular);
                    using Bitmap tempBmp = new Bitmap(1, 1);
                    using Graphics tempG = Graphics.FromImage(tempBmp);

                    float lineH = tempG.MeasureString("Ag", valueFont).Height;
                    int padding = 10;
                    int labelH = (int)(lineH * 3) + padding * 2;

                    using Bitmap finalImage = new Bitmap(qrImage.Width, qrImage.Height + labelH);
                    using Graphics g = Graphics.FromImage(finalImage);

                    g.Clear(Color.White);
                    g.DrawImage(qrImage, 0, 0);

                    float startY = qrImage.Height + padding;
                    g.DrawString($"School ID : {schoolID}", valueFont, Brushes.Black, 10f, startY);
                    g.DrawString($"Name      : {name}", valueFont, Brushes.Black, 10f, startY + lineH);
                    g.DrawString($"Year Level: {year}", valueFont, Brushes.Black, 10f, startY + lineH * 2);

                    finalImage.Save(sfd.FileName);
                    MessageBox.Show("QR Code downloaded successfully!", "Success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error downloading QR:\n" + ex.Message, "Download Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        private void btnSearchQR_Click(object sender, EventArgs e)
        {
            string text = txtSearchQR.Text.Trim();
            if (string.IsNullOrEmpty(text) || text == "Search Student Name or ID")
            {
                LoadQRData();
                return;
            }
            SearchQR(text);
        }

        private void dgvGenerateQR_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvGenerateQR.CurrentRow == null || dgvGenerateQR.CurrentRow.IsNewRow)
            {
                pictureQR.Image?.Dispose();
                pictureQR.Image = null;
                return;
            }

            DisplayQRForRow(dgvGenerateQR.CurrentRow.Index);

        }

        private void txtSearchQR_TextChanged(object sender, EventArgs e)
        {
            string text = txtSearchQR.Text.Trim();


            if (string.IsNullOrEmpty(text) || text == "Search Student Name or ID")
                LoadQRData();
        }


        private void SearchQR(string searchText)
        {
            dgvGenerateQR.Rows.Clear();
            searchText = searchText.Trim().ToLower();


            var filtered = StudentDB.GetAll().Where(q =>
                (q.StudentID?.ToLower().Contains(searchText) ?? false) ||
                (q.StudentName?.ToLower().Contains(searchText) ?? false) ||
                (q.Course?.ToLower().Contains(searchText) ?? false) ||
                (q.Year?.ToLower().Contains(searchText) ?? false)
            ).ToList();

            foreach (var qr in filtered)
            {
                dgvGenerateQR.Rows.Add(
                    qr.StudentID ?? "",
                    qr.StudentName ?? "",
                    qr.Course ?? "",
                    qr.Year ?? "",
                    LoadStudentPhoto(qr.PhotoPath)
                );
            }

            dgvGenerateQR.ClearSelection();

            if (filtered.Count == 0)
                MessageBox.Show("No results found.");

        }

        public void AddStudentToGrid(generateQR qr)
        {
            if (qr == null) return;

            dgvGenerateQR.Rows.Add(
                qr.StudentID ?? "",
                qr.StudentName ?? "",
                qr.Course ?? "",
                qr.Year ?? "",
                LoadStudentPhoto(qr.PhotoPath)
            );
        }

        public void LoadQRList()
        {
            dgvGenerateQR.Rows.Clear();

            foreach (var qr in generateQR.QRList)
            {
                dgvGenerateQR.Rows.Add(
                    qr.StudentID ?? "",
                    qr.StudentName ?? "",
                    qr.Course ?? "",
                    qr.Year ?? "",
                    LoadStudentPhoto(qr.PhotoPath)
                );
            }
        }

        private Image? LoadStudentPhoto(string? photoPath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(photoPath) && File.Exists(photoPath))
                    return Image.FromFile(photoPath);
            }
            catch { }
            return null;
        }

        private void txtSearchQR_Enter(object sender, EventArgs e)
        {
            if (txtSearchQR.Text == "Search Student Name or ID")
            {
                txtSearchQR.Text = "";
                txtSearchQR.ForeColor = Color.Black;
            }
        }

        private void txtSearchQR_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchQR.Text))
            {
                txtSearchQR.Text = "Search Student Name or ID";
                txtSearchQR.ForeColor = Color.Gray;

            }
        }

        private void RefreshIfSearchCleared()
        {
            string text = txtSearchQR.Text.Trim();

            if (string.IsNullOrEmpty(text) || text == "Search Student Name or ID")
                LoadQRData();
        }


        private void dgvGenerateQR_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DisplayQRForRow(e.RowIndex);
        }

        private void DisplayQRForRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvGenerateQR.Rows.Count) return;

            string? studentID = dgvGenerateQR.Rows[rowIndex].Cells[0]?.Value?.ToString()?.Trim();
            string? yearLevel = dgvGenerateQR.Rows[rowIndex].Cells[3]?.Value?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(studentID)) return;

            // Force a fresh single-record fetch instead of GetAll() + FirstOrDefault
            var qrData = StudentDB.GetAll()
                .FirstOrDefault(q => q.StudentID == studentID && q.Year == yearLevel);

            if (qrData == null) return;

            try
            {
                pictureQR.Image?.Dispose();
                pictureQR.Image = null;

                if (!string.IsNullOrWhiteSpace(qrData.QRCodeData))
                {
                    using var generator = new QRCodeGenerator();
                    using var data = generator.CreateQrCode(qrData.QRCodeData, QRCodeGenerator.ECCLevel.Q);
                    using var qr = new QRCode(data);

                    // Clone so disposing 'qr' doesn't kill the displayed image
                    pictureQR.Image = (Bitmap)qr.GetGraphic(10).Clone();
                }
                else if (!string.IsNullOrEmpty(qrData.PhotoPath) && File.Exists(qrData.PhotoPath))
                {
                    pictureQR.Image = Image.FromFile(qrData.PhotoPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying QR: " + ex.Message);
            }

        }

        private void txtSearchQR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSearchQR_Click(sender, e);
            }
        }
    }
}
