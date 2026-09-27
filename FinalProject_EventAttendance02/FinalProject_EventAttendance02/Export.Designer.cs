namespace FinalProject_EventAttendance02
{
    partial class Export
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Export));
            panel1 = new Panel();
            label1 = new Label();
            filterExport = new Panel();
            btnGenerateReport = new Button();
            btnExport1 = new Button();
            cmbEventExport = new ComboBox();
            cmbCourseExport = new ComboBox();
            cmbYearExport = new ComboBox();
            label5 = new Label();
            label4 = new Label();
            label2 = new Label();
            label6 = new Label();
            reportPanel = new Panel();
            lblEventExport = new Label();
            lblDateExport = new Label();
            lblSemesterExport = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            dgvExport = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewTextBoxColumn();
            Column6 = new DataGridViewTextBoxColumn();
            Column7 = new DataGridViewTextBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            panel1.SuspendLayout();
            filterExport.SuspendLayout();
            reportPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvExport).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(195, 17, 12);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(983, 52);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(280, 9);
            label1.Name = "label1";
            label1.Size = new Size(378, 30);
            label1.TabIndex = 0;
            label1.Text = " 📁 Export Event Attendance Report ";
            // 
            // filterExport
            // 
            filterExport.BackColor = Color.White;
            filterExport.Controls.Add(btnGenerateReport);
            filterExport.Controls.Add(btnExport1);
            filterExport.Controls.Add(cmbEventExport);
            filterExport.Controls.Add(cmbCourseExport);
            filterExport.Controls.Add(cmbYearExport);
            filterExport.Controls.Add(label5);
            filterExport.Controls.Add(label4);
            filterExport.Controls.Add(label2);
            filterExport.Location = new Point(110, 58);
            filterExport.Name = "filterExport";
            filterExport.Size = new Size(691, 173);
            filterExport.TabIndex = 1;
            // 
            // btnGenerateReport
            // 
            btnGenerateReport.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerateReport.Image = (Image)resources.GetObject("btnGenerateReport.Image");
            btnGenerateReport.ImageAlign = ContentAlignment.MiddleLeft;
            btnGenerateReport.Location = new Point(415, 61);
            btnGenerateReport.Name = "btnGenerateReport";
            btnGenerateReport.Size = new Size(219, 40);
            btnGenerateReport.TabIndex = 2;
            btnGenerateReport.Text = "    Generate Report";
            btnGenerateReport.UseVisualStyleBackColor = true;
            btnGenerateReport.Click += btnGenerateReport_Click;
            // 
            // btnExport1
            // 
            btnExport1.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExport1.Image = (Image)resources.GetObject("btnExport1.Image");
            btnExport1.ImageAlign = ContentAlignment.MiddleLeft;
            btnExport1.Location = new Point(415, 108);
            btnExport1.Name = "btnExport1";
            btnExport1.Size = new Size(219, 40);
            btnExport1.TabIndex = 2;
            btnExport1.Text = "  Export to Excel";
            btnExport1.UseVisualStyleBackColor = true;
            btnExport1.Click += btnExport1_Click;
            // 
            // cmbEventExport
            // 
            cmbEventExport.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbEventExport.FormattingEnabled = true;
            cmbEventExport.Location = new Point(144, 13);
            cmbEventExport.Name = "cmbEventExport";
            cmbEventExport.Size = new Size(218, 33);
            cmbEventExport.TabIndex = 1;
            // 
            // cmbCourseExport
            // 
            cmbCourseExport.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbCourseExport.FormattingEnabled = true;
            cmbCourseExport.Items.AddRange(new object[] { "All", "BSIT", "BSHM", "BSBA", "BEED", "BSED", "BSTM", "BSE", "BSMA", "SHS" });
            cmbCourseExport.Location = new Point(144, 61);
            cmbCourseExport.Name = "cmbCourseExport";
            cmbCourseExport.Size = new Size(160, 33);
            cmbCourseExport.TabIndex = 1;
            // 
            // cmbYearExport
            // 
            cmbYearExport.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbYearExport.FormattingEnabled = true;
            cmbYearExport.Items.AddRange(new object[] { "All", "1st Year", "2nd Year", "3rd Year", "4th Year", "Grade 11", "Grade 12" });
            cmbYearExport.Location = new Point(144, 108);
            cmbYearExport.Name = "cmbYearExport";
            cmbYearExport.Size = new Size(160, 33);
            cmbYearExport.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(26, 107);
            label5.Name = "label5";
            label5.Size = new Size(112, 30);
            label5.TabIndex = 0;
            label5.Text = "Year Level:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(26, 60);
            label4.Name = "label4";
            label4.Size = new Size(84, 30);
            label4.TabIndex = 0;
            label4.Text = "Course:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(26, 12);
            label2.Name = "label2";
            label2.Size = new Size(71, 30);
            label2.TabIndex = 0;
            label2.Text = "Event:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(316, 267);
            label6.Name = "label6";
            label6.Size = new Size(271, 40);
            label6.TabIndex = 2;
            label6.Text = "📋 Report Preview";
            // 
            // reportPanel
            // 
            reportPanel.BackColor = Color.White;
            reportPanel.Controls.Add(lblEventExport);
            reportPanel.Controls.Add(lblDateExport);
            reportPanel.Controls.Add(lblSemesterExport);
            reportPanel.Controls.Add(label9);
            reportPanel.Controls.Add(label8);
            reportPanel.Controls.Add(label7);
            reportPanel.Location = new Point(211, 310);
            reportPanel.Name = "reportPanel";
            reportPanel.Size = new Size(472, 123);
            reportPanel.TabIndex = 3;
            // 
            // lblEventExport
            // 
            lblEventExport.AutoSize = true;
            lblEventExport.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEventExport.Location = new Point(148, 14);
            lblEventExport.Name = "lblEventExport";
            lblEventExport.Size = new Size(20, 25);
            lblEventExport.TabIndex = 1;
            lblEventExport.Text = "-";
            // 
            // lblDateExport
            // 
            lblDateExport.AutoSize = true;
            lblDateExport.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDateExport.Location = new Point(148, 48);
            lblDateExport.Name = "lblDateExport";
            lblDateExport.Size = new Size(20, 25);
            lblDateExport.TabIndex = 1;
            lblDateExport.Text = "-";
            // 
            // lblSemesterExport
            // 
            lblSemesterExport.AutoSize = true;
            lblSemesterExport.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSemesterExport.Location = new Point(148, 82);
            lblSemesterExport.Name = "lblSemesterExport";
            lblSemesterExport.Size = new Size(20, 25);
            lblSemesterExport.TabIndex = 1;
            lblSemesterExport.Text = "-";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(43, 82);
            label9.Name = "label9";
            label9.Size = new Size(99, 25);
            label9.TabIndex = 0;
            label9.Text = "Semester:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(43, 48);
            label8.Name = "label8";
            label8.Size = new Size(58, 25);
            label8.TabIndex = 0;
            label8.Text = "Date:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(43, 14);
            label7.Name = "label7";
            label7.Size = new Size(66, 25);
            label7.TabIndex = 0;
            label7.Text = "Event:";
            // 
            // dgvExport
            // 
            dgvExport.AllowUserToAddRows = false;
            dgvExport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvExport.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8, Column9 });
            dgvExport.Location = new Point(12, 439);
            dgvExport.Name = "dgvExport";
            dgvExport.ReadOnly = true;
            dgvExport.Size = new Size(959, 187);
            dgvExport.TabIndex = 4;
            // 
            // Column1
            // 
            Column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column1.HeaderText = "Student ID";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            // 
            // Column2
            // 
            Column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column2.HeaderText = "Student Name";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // Column3
            // 
            Column3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column3.HeaderText = "Course";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            // 
            // Column4
            // 
            Column4.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column4.HeaderText = "Year Level";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            // 
            // Column5
            // 
            Column5.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column5.HeaderText = "Event Name";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            // 
            // Column6
            // 
            Column6.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column6.HeaderText = "Date";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            // 
            // Column7
            // 
            Column7.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column7.HeaderText = "Check-in Time";
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            // 
            // Column8
            // 
            Column8.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column8.HeaderText = "Check-out Time";
            Column8.Name = "Column8";
            Column8.ReadOnly = true;
            // 
            // Column9
            // 
            Column9.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Column9.HeaderText = "Status";
            Column9.Name = "Column9";
            Column9.ReadOnly = true;
            // 
            // Export
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 69, 214);
            ClientSize = new Size(983, 640);
            Controls.Add(dgvExport);
            Controls.Add(reportPanel);
            Controls.Add(label6);
            Controls.Add(filterExport);
            Controls.Add(panel1);
            Name = "Export";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Export";
            Load += Export_Load;
            Shown += Export_Shown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            filterExport.ResumeLayout(false);
            filterExport.PerformLayout();
            reportPanel.ResumeLayout(false);
            reportPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvExport).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Panel filterExport;
        private Label label5;
        private Label label4;
        private Label label2;
        private Button btnGenerateReport;
        private Button btnExport1;
        private ComboBox cmbEventExport;
        private ComboBox cmbCourseExport;
        private ComboBox cmbYearExport;
        private Label label6;
        private Panel reportPanel;
        private Label label8;
        private Label label7;
        private Label lblEventExport;
        private Label lblDateExport;
        private Label lblSemesterExport;
        private Label label9;
        private DataGridView dgvExport;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column9;
    }
}