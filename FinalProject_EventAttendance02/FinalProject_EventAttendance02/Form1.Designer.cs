namespace FinalProject_EventAttendance02
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            label3 = new Label();
            label2 = new Label();
            panel2 = new Panel();
            userPanel = new Panel();
            btnStudent = new Button();
            btnAdmin = new Button();
            label1 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel2.SuspendLayout();
            userPanel.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(326, 458);
            panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(94, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(134, 93);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImage = (Image)resources.GetObject("pictureBox2.BackgroundImage");
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.Location = new Point(26, 225);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(276, 191);
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.ButtonHighlight;
            label3.Location = new Point(8, 169);
            label3.Name = "label3";
            label3.Size = new Size(312, 25);
            label3.TabIndex = 1;
            label3.Text = "Event Attendance Tracking System";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(26, 121);
            label2.Name = "label2";
            label2.Size = new Size(276, 37);
            label2.TabIndex = 0;
            label2.Text = "Welcome ACESTrack";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(195, 17, 12);
            panel2.Controls.Add(userPanel);
            panel2.Dock = DockStyle.Right;
            panel2.Location = new Point(326, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(470, 458);
            panel2.TabIndex = 1;
            // 
            // userPanel
            // 
            userPanel.BackColor = Color.White;
            userPanel.BorderStyle = BorderStyle.FixedSingle;
            userPanel.Controls.Add(btnStudent);
            userPanel.Controls.Add(btnAdmin);
            userPanel.Controls.Add(label1);
            userPanel.Location = new Point(36, 47);
            userPanel.Name = "userPanel";
            userPanel.Size = new Size(403, 358);
            userPanel.TabIndex = 0;
            // 
            // btnStudent
            // 
            btnStudent.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStudent.Image = (Image)resources.GetObject("btnStudent.Image");
            btnStudent.ImageAlign = ContentAlignment.MiddleLeft;
            btnStudent.Location = new Point(70, 206);
            btnStudent.Name = "btnStudent";
            btnStudent.Size = new Size(271, 77);
            btnStudent.TabIndex = 0;
            btnStudent.Text = "    Student";
            btnStudent.UseVisualStyleBackColor = true;
            btnStudent.Click += btnStudent_Click;
            // 
            // btnAdmin
            // 
            btnAdmin.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdmin.Image = (Image)resources.GetObject("btnAdmin.Image");
            btnAdmin.ImageAlign = ContentAlignment.MiddleLeft;
            btnAdmin.Location = new Point(70, 97);
            btnAdmin.Name = "btnAdmin";
            btnAdmin.Size = new Size(271, 77);
            btnAdmin.TabIndex = 0;
            btnAdmin.Text = "    Admin";
            btnAdmin.UseVisualStyleBackColor = true;
            btnAdmin.Click += btnAdmin_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(97, 20);
            label1.Name = "label1";
            label1.Size = new Size(222, 47);
            label1.TabIndex = 0;
            label1.Text = "Choose User";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 69, 214);
            ClientSize = new Size(796, 458);
            Controls.Add(panel2);
            Controls.Add(panel1);
            ForeColor = SystemColors.WindowText;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            Shown += Form1_Shown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel2.ResumeLayout(false);
            userPanel.ResumeLayout(false);
            userPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel userPanel;
        private Button btnStudent;
        private Button btnAdmin;
        private Label label1;
        private Label label2;
        private Label label3;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
    }
}
