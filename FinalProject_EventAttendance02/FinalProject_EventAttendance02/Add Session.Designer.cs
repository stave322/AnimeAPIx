namespace FinalProject_EventAttendance02
{
    partial class Add_Session
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Add_Session));
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            btnSavetime = new Button();
            checkinStart = new DateTimePicker();
            checkinEnd = new DateTimePicker();
            label4 = new Label();
            lblLocation = new Label();
            label5 = new Label();
            lblEventName = new Label();
            label2 = new Label();
            label3 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
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
            label1.Location = new Point(349, 9);
            label1.Name = "label1";
            label1.Size = new Size(108, 30);
            label1.TabIndex = 0;
            label1.Text = "Add Time";
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(btnSavetime);
            panel2.Controls.Add(checkinStart);
            panel2.Controls.Add(checkinEnd);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(lblLocation);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(lblEventName);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label3);
            panel2.Location = new Point(97, 75);
            panel2.Name = "panel2";
            panel2.Size = new Size(600, 373);
            panel2.TabIndex = 1;
            panel2.Paint += panel2_Paint;
            // 
            // btnSavetime
            // 
            btnSavetime.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSavetime.Image = (Image)resources.GetObject("btnSavetime.Image");
            btnSavetime.ImageAlign = ContentAlignment.MiddleLeft;
            btnSavetime.Location = new Point(390, 314);
            btnSavetime.Name = "btnSavetime";
            btnSavetime.Size = new Size(185, 40);
            btnSavetime.TabIndex = 3;
            btnSavetime.Text = "    Save Time";
            btnSavetime.UseVisualStyleBackColor = true;
            btnSavetime.Click += btnSavetime_Click;
            // 
            // checkinStart
            // 
            checkinStart.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            checkinStart.Location = new Point(275, 143);
            checkinStart.Name = "checkinStart";
            checkinStart.Size = new Size(133, 35);
            checkinStart.TabIndex = 2;
            // 
            // checkinEnd
            // 
            checkinEnd.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            checkinEnd.Location = new Point(275, 191);
            checkinEnd.Name = "checkinEnd";
            checkinEnd.Size = new Size(133, 35);
            checkinEnd.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(58, 191);
            label4.Name = "label4";
            label4.Size = new Size(201, 30);
            label4.TabIndex = 0;
            label4.Text = "Check-in End Time:";
            // 
            // lblLocation
            // 
            lblLocation.AutoSize = true;
            lblLocation.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLocation.Location = new Point(201, 82);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(21, 30);
            lblLocation.TabIndex = 0;
            lblLocation.Text = "-";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(58, 82);
            label5.Name = "label5";
            label5.Size = new Size(104, 30);
            label5.TabIndex = 0;
            label5.Text = "Location:";
            // 
            // lblEventName
            // 
            lblEventName.AutoSize = true;
            lblEventName.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEventName.Location = new Point(201, 43);
            lblEventName.Name = "lblEventName";
            lblEventName.Size = new Size(21, 30);
            lblEventName.TabIndex = 0;
            lblEventName.Text = "-";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(58, 43);
            label2.Name = "label2";
            label2.Size = new Size(137, 30);
            label2.TabIndex = 0;
            label2.Text = "Event Name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(58, 143);
            label3.Name = "label3";
            label3.Size = new Size(211, 30);
            label3.TabIndex = 0;
            label3.Text = "Check-in Start Time:";
            // 
            // Add_Session
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 69, 214);
            ClientSize = new Size(804, 473);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Add_Session";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Add Time";
            FormClosed += Add_Session_FormClosed;
            Load += Add_Session_Load;
            Shown += Add_Session_Shown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private Label label4;
        private Label label3;
        private DateTimePicker checkinStart;
        private DateTimePicker checkinEnd;
        private Button btnSavetime;
        private Label lblLocation;
        private Label label5;
        private Label lblEventName;
        private Label label2;
    }
}