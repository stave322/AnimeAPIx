namespace FinalProject_EventAttendance02
{
    partial class Event_Details
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Event_Details));
            panel1 = new Panel();
            label1 = new Label();
            eventinfoPanel = new Panel();
            btnCreateEvent = new Button();
            btnUpdateEvent = new Button();
            selSemester = new ComboBox();
            txtLocation = new TextBox();
            txtEventName = new TextBox();
            dateEvent = new DateTimePicker();
            label7 = new Label();
            groupBox1 = new GroupBox();
            btnAddTime = new Button();
            checkinEnd = new DateTimePicker();
            checkinStart = new DateTimePicker();
            label9 = new Label();
            label8 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            eventinfoPanel.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(195, 17, 12);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(804, 52);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(321, 9);
            label1.Name = "label1";
            label1.Size = new Size(223, 30);
            label1.TabIndex = 0;
            label1.Text = "📋 Event Information";
            // 
            // eventinfoPanel
            // 
            eventinfoPanel.BackColor = Color.White;
            eventinfoPanel.Controls.Add(btnCreateEvent);
            eventinfoPanel.Controls.Add(btnUpdateEvent);
            eventinfoPanel.Controls.Add(selSemester);
            eventinfoPanel.Controls.Add(txtLocation);
            eventinfoPanel.Controls.Add(txtEventName);
            eventinfoPanel.Controls.Add(dateEvent);
            eventinfoPanel.Controls.Add(label7);
            eventinfoPanel.Controls.Add(groupBox1);
            eventinfoPanel.Controls.Add(label5);
            eventinfoPanel.Controls.Add(label4);
            eventinfoPanel.Controls.Add(label3);
            eventinfoPanel.Controls.Add(label2);
            eventinfoPanel.Location = new Point(71, 71);
            eventinfoPanel.Name = "eventinfoPanel";
            eventinfoPanel.Size = new Size(655, 632);
            eventinfoPanel.TabIndex = 1;
            // 
            // btnCreateEvent
            // 
            btnCreateEvent.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCreateEvent.Image = (Image)resources.GetObject("btnCreateEvent.Image");
            btnCreateEvent.ImageAlign = ContentAlignment.MiddleLeft;
            btnCreateEvent.Location = new Point(472, 577);
            btnCreateEvent.Name = "btnCreateEvent";
            btnCreateEvent.Size = new Size(145, 40);
            btnCreateEvent.TabIndex = 5;
            btnCreateEvent.Text = "    Create";
            btnCreateEvent.UseVisualStyleBackColor = true;
            btnCreateEvent.Click += btnCreateEvent_Click;
            // 
            // btnUpdateEvent
            // 
            btnUpdateEvent.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUpdateEvent.Image = (Image)resources.GetObject("btnUpdateEvent.Image");
            btnUpdateEvent.ImageAlign = ContentAlignment.MiddleLeft;
            btnUpdateEvent.Location = new Point(307, 577);
            btnUpdateEvent.Name = "btnUpdateEvent";
            btnUpdateEvent.Size = new Size(145, 40);
            btnUpdateEvent.TabIndex = 5;
            btnUpdateEvent.Text = "    Update";
            btnUpdateEvent.UseVisualStyleBackColor = true;
            btnUpdateEvent.Click += btnUpdateEvent_Click;
            // 
            // selSemester
            // 
            selSemester.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            selSemester.FormattingEnabled = true;
            selSemester.Items.AddRange(new object[] { "1st Semester", "2nd Semester" });
            selSemester.Location = new Point(218, 173);
            selSemester.Name = "selSemester";
            selSemester.Size = new Size(255, 33);
            selSemester.TabIndex = 4;
            // 
            // txtLocation
            // 
            txtLocation.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtLocation.Location = new Point(218, 82);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(309, 33);
            txtLocation.TabIndex = 3;
            // 
            // txtEventName
            // 
            txtEventName.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtEventName.Location = new Point(218, 37);
            txtEventName.Name = "txtEventName";
            txtEventName.Size = new Size(309, 33);
            txtEventName.TabIndex = 3;
            // 
            // dateEvent
            // 
            dateEvent.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dateEvent.Location = new Point(218, 126);
            dateEvent.Name = "dateEvent";
            dateEvent.Size = new Size(255, 35);
            dateEvent.TabIndex = 2;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(218, 229);
            label7.Name = "label7";
            label7.Size = new Size(180, 30);
            label7.TabIndex = 0;
            label7.Text = "Attendance Time";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnAddTime);
            groupBox1.Controls.Add(checkinEnd);
            groupBox1.Controls.Add(checkinStart);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(label8);
            groupBox1.Location = new Point(38, 262);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(579, 245);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            // 
            // btnAddTime
            // 
            btnAddTime.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddTime.Image = (Image)resources.GetObject("btnAddTime.Image");
            btnAddTime.ImageAlign = ContentAlignment.MiddleLeft;
            btnAddTime.Location = new Point(180, 178);
            btnAddTime.Name = "btnAddTime";
            btnAddTime.Size = new Size(201, 40);
            btnAddTime.TabIndex = 7;
            btnAddTime.Text = "    Add Time";
            btnAddTime.UseVisualStyleBackColor = true;
            btnAddTime.Click += btnAddTime_Click;
            // 
            // checkinEnd
            // 
            checkinEnd.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            checkinEnd.Location = new Point(269, 99);
            checkinEnd.Name = "checkinEnd";
            checkinEnd.Size = new Size(122, 35);
            checkinEnd.TabIndex = 2;
            // 
            // checkinStart
            // 
            checkinStart.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            checkinStart.Location = new Point(269, 49);
            checkinStart.Name = "checkinStart";
            checkinStart.Size = new Size(122, 35);
            checkinStart.TabIndex = 2;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(37, 103);
            label9.Name = "label9";
            label9.Size = new Size(201, 30);
            label9.TabIndex = 1;
            label9.Text = "Check-in End Time:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(37, 54);
            label8.Name = "label8";
            label8.Size = new Size(211, 30);
            label8.TabIndex = 1;
            label8.Text = "Check-in Start Time:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(75, 172);
            label5.Name = "label5";
            label5.Size = new Size(109, 30);
            label5.TabIndex = 0;
            label5.Text = "Semester:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(75, 126);
            label4.Name = "label4";
            label4.Size = new Size(64, 30);
            label4.TabIndex = 0;
            label4.Text = "Date:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(75, 81);
            label3.Name = "label3";
            label3.Size = new Size(104, 30);
            label3.TabIndex = 0;
            label3.Text = "Location:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(75, 36);
            label2.Name = "label2";
            label2.Size = new Size(137, 30);
            label2.TabIndex = 0;
            label2.Text = "Event Name:";
            // 
            // Event_Details
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 69, 214);
            ClientSize = new Size(804, 718);
            Controls.Add(eventinfoPanel);
            Controls.Add(panel1);
            Name = "Event_Details";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Event Details";
            FormClosed += Event_Details_FormClosed;
            Load += Event_Details_Load;
            Shown += Event_Details_Shown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            eventinfoPanel.ResumeLayout(false);
            eventinfoPanel.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Panel eventinfoPanel;
        private GroupBox groupBox1;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private DateTimePicker checkinEnd;
        private DateTimePicker checkinStart;
        private Label label9;
        private Label label8;
        private ComboBox selSemester;
        private TextBox txtLocation;
        private TextBox txtEventName;
        private DateTimePicker dateEvent;
        private Button btnCreateEvent;
        private Button btnUpdateEvent;
        private Label label7;
        private Button btnAddTime;
    }
}