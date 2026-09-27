namespace FinalProject_EventAttendance02
{
    partial class Student
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Student));
            panel1 = new Panel();
            label5 = new Label();
            pictureBox2 = new PictureBox();
            lblTime = new Label();
            label3 = new Label();
            lblDate = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            updatePanel = new Panel();
            lblTimeRemaining = new Label();
            lblEventName = new Label();
            lblEventDate = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            studentPanel = new Panel();
            lblName = new Label();
            lblStudentID = new Label();
            lblCourse = new Label();
            lblYearLevel = new Label();
            lblScanResult = new Label();
            label16 = new Label();
            label15 = new Label();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            pictureStudent = new PictureBox();
            txtScan = new TextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            updatePanel.SuspendLayout();
            studentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureStudent).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(195, 17, 12);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label5);
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(lblTime);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(lblDate);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1370, 58);
            panel1.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(74, 15);
            label5.Name = "label5";
            label5.Size = new Size(67, 32);
            label5.TabIndex = 6;
            label5.Text = "Back";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.White;
            pictureBox2.BackgroundImage = (Image)resources.GetObject("pictureBox2.BackgroundImage");
            pictureBox2.BackgroundImageLayout = ImageLayout.Center;
            pictureBox2.Location = new Point(12, 4);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(56, 50);
            pictureBox2.TabIndex = 5;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTime.ForeColor = Color.White;
            lblTime.Location = new Point(1181, 17);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(129, 30);
            lblTime.TabIndex = 4;
            lblTime.Text = "10:42:22 pm";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(1156, 17);
            label3.Name = "label3";
            label3.Size = new Size(19, 30);
            label3.TabIndex = 3;
            label3.Text = "|";
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDate.ForeColor = Color.White;
            lblDate.Location = new Point(1001, 17);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(149, 30);
            lblDate.TabIndex = 2;
            lblDate.Text = "March 7, 2026";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(556, 9);
            label1.Name = "label1";
            label1.Size = new Size(159, 40);
            label1.TabIndex = 1;
            label1.Text = "ACESTrack";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(465, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(85, 51);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // updatePanel
            // 
            updatePanel.BackColor = Color.FromArgb(28, 48, 160);
            updatePanel.Controls.Add(lblTimeRemaining);
            updatePanel.Controls.Add(lblEventName);
            updatePanel.Controls.Add(lblEventDate);
            updatePanel.Controls.Add(label8);
            updatePanel.Controls.Add(label7);
            updatePanel.Controls.Add(label6);
            updatePanel.Location = new Point(150, 64);
            updatePanel.Name = "updatePanel";
            updatePanel.Size = new Size(1026, 125);
            updatePanel.TabIndex = 1;
            // 
            // lblTimeRemaining
            // 
            lblTimeRemaining.AutoSize = true;
            lblTimeRemaining.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTimeRemaining.ForeColor = SystemColors.ButtonHighlight;
            lblTimeRemaining.Location = new Point(521, 80);
            lblTimeRemaining.Name = "lblTimeRemaining";
            lblTimeRemaining.Size = new Size(20, 25);
            lblTimeRemaining.TabIndex = 1;
            lblTimeRemaining.Text = "-";
            // 
            // lblEventName
            // 
            lblEventName.AutoSize = true;
            lblEventName.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEventName.ForeColor = Color.White;
            lblEventName.Location = new Point(521, 19);
            lblEventName.Name = "lblEventName";
            lblEventName.Size = new Size(20, 25);
            lblEventName.TabIndex = 1;
            lblEventName.Text = "-";
            // 
            // lblEventDate
            // 
            lblEventDate.AutoSize = true;
            lblEventDate.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEventDate.ForeColor = SystemColors.ButtonHighlight;
            lblEventDate.Location = new Point(521, 49);
            lblEventDate.Name = "lblEventDate";
            lblEventDate.Size = new Size(20, 25);
            lblEventDate.TabIndex = 1;
            lblEventDate.Text = "-";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.White;
            label8.Location = new Point(334, 75);
            label8.Name = "label8";
            label8.Size = new Size(178, 30);
            label8.TabIndex = 0;
            label8.Text = "Time Remaining:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.White;
            label7.Location = new Point(334, 45);
            label7.Name = "label7";
            label7.Size = new Size(181, 30);
            label7.TabIndex = 0;
            label7.Text = "Event Date/Time:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(334, 15);
            label6.Name = "label6";
            label6.Size = new Size(137, 30);
            label6.TabIndex = 0;
            label6.Text = "Event Name:";
            // 
            // studentPanel
            // 
            studentPanel.BackColor = Color.White;
            studentPanel.Controls.Add(lblName);
            studentPanel.Controls.Add(lblStudentID);
            studentPanel.Controls.Add(lblCourse);
            studentPanel.Controls.Add(lblYearLevel);
            studentPanel.Controls.Add(lblScanResult);
            studentPanel.Controls.Add(label16);
            studentPanel.Controls.Add(label15);
            studentPanel.Controls.Add(label14);
            studentPanel.Controls.Add(label13);
            studentPanel.Controls.Add(label12);
            studentPanel.Controls.Add(pictureStudent);
            studentPanel.Location = new Point(314, 195);
            studentPanel.Name = "studentPanel";
            studentPanel.Size = new Size(671, 542);
            studentPanel.TabIndex = 2;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI Semibold", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblName.Location = new Point(223, 286);
            lblName.Name = "lblName";
            lblName.Size = new Size(28, 37);
            lblName.TabIndex = 2;
            lblName.Text = "-";
            // 
            // lblStudentID
            // 
            lblStudentID.AutoSize = true;
            lblStudentID.Font = new Font("Segoe UI Semibold", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStudentID.Location = new Point(223, 335);
            lblStudentID.Name = "lblStudentID";
            lblStudentID.Size = new Size(28, 37);
            lblStudentID.TabIndex = 2;
            lblStudentID.Text = "-";
            // 
            // lblCourse
            // 
            lblCourse.AutoSize = true;
            lblCourse.Font = new Font("Segoe UI Semibold", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCourse.Location = new Point(223, 386);
            lblCourse.Name = "lblCourse";
            lblCourse.Size = new Size(28, 37);
            lblCourse.TabIndex = 2;
            lblCourse.Text = "-";
            // 
            // lblYearLevel
            // 
            lblYearLevel.AutoSize = true;
            lblYearLevel.Font = new Font("Segoe UI Semibold", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblYearLevel.Location = new Point(223, 437);
            lblYearLevel.Name = "lblYearLevel";
            lblYearLevel.Size = new Size(28, 37);
            lblYearLevel.TabIndex = 2;
            lblYearLevel.Text = "-";
            // 
            // lblScanResult
            // 
            lblScanResult.AutoSize = true;
            lblScanResult.Font = new Font("Segoe UI Semibold", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblScanResult.Location = new Point(234, 489);
            lblScanResult.Name = "lblScanResult";
            lblScanResult.Size = new Size(28, 37);
            lblScanResult.TabIndex = 2;
            lblScanResult.Text = "-";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label16.Location = new Point(47, 487);
            label16.Name = "label16";
            label16.Size = new Size(181, 40);
            label16.TabIndex = 1;
            label16.Text = "Scan Result:";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label15.Location = new Point(47, 434);
            label15.Name = "label15";
            label15.Size = new Size(163, 40);
            label15.TabIndex = 1;
            label15.Text = "Year Level:";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.Location = new Point(47, 383);
            label14.Name = "label14";
            label14.Size = new Size(120, 40);
            label14.TabIndex = 1;
            label14.Text = "Course:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label13.Location = new Point(47, 335);
            label13.Name = "label13";
            label13.Size = new Size(170, 40);
            label13.TabIndex = 1;
            label13.Text = "Student ID:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label12.Location = new Point(47, 283);
            label12.Name = "label12";
            label12.Size = new Size(107, 40);
            label12.TabIndex = 1;
            label12.Text = "Name:";
            // 
            // pictureStudent
            // 
            pictureStudent.BackgroundImage = (Image)resources.GetObject("pictureStudent.BackgroundImage");
            pictureStudent.BackgroundImageLayout = ImageLayout.Center;
            pictureStudent.BorderStyle = BorderStyle.FixedSingle;
            pictureStudent.Location = new Point(217, 16);
            pictureStudent.Name = "pictureStudent";
            pictureStudent.Size = new Size(250, 250);
            pictureStudent.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureStudent.TabIndex = 0;
            pictureStudent.TabStop = false;
            // 
            // txtScan
            // 
            txtScan.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtScan.Location = new Point(1065, 497);
            txtScan.Name = "txtScan";
            txtScan.Size = new Size(232, 33);
            txtScan.TabIndex = 3;
            txtScan.Visible = false;
            txtScan.KeyDown += txtScan_KeyDown;
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // Student
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 69, 214);
            ClientSize = new Size(1370, 749);
            Controls.Add(txtScan);
            Controls.Add(studentPanel);
            Controls.Add(updatePanel);
            Controls.Add(panel1);
            Name = "Student";
            Text = "Student";
            Load += Student_Load;
            Shown += Student_Shown;
            KeyPress += Student_KeyPress;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            updatePanel.ResumeLayout(false);
            updatePanel.PerformLayout();
            studentPanel.ResumeLayout(false);
            studentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureStudent).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBox1;
        private Label label1;
        private Label lblDate;
        private Label lblTime;
        private Label label3;
        private Label label5;
        private PictureBox pictureBox2;
        private Panel updatePanel;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label lblTimeRemaining;
        private Label lblEventName;
        private Label lblEventDate;
        private Panel studentPanel;
        private PictureBox pictureStudent;
        private Label label12;
        private Label label16;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label lblName;
        private Label lblStudentID;
        private Label lblCourse;
        private Label lblYearLevel;
        private Label lblScanResult;
        private TextBox txtScan;
        private System.Windows.Forms.Timer timer1;
    }
}