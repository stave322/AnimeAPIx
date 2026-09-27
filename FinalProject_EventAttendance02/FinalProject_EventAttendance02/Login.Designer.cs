namespace FinalProject_EventAttendance02
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            loginPanel = new Panel();
            backUser = new Label();
            btnLogin = new Button();
            showPass = new CheckBox();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            label1 = new Label();
            iconPanel = new Panel();
            pictureBox3 = new PictureBox();
            loginPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            iconPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // loginPanel
            // 
            loginPanel.BackColor = Color.White;
            loginPanel.BorderStyle = BorderStyle.FixedSingle;
            loginPanel.Controls.Add(backUser);
            loginPanel.Controls.Add(btnLogin);
            loginPanel.Controls.Add(showPass);
            loginPanel.Controls.Add(pictureBox2);
            loginPanel.Controls.Add(pictureBox1);
            loginPanel.Controls.Add(txtPassword);
            loginPanel.Controls.Add(txtUsername);
            loginPanel.Controls.Add(label1);
            loginPanel.Location = new Point(211, 57);
            loginPanel.Name = "loginPanel";
            loginPanel.Size = new Size(369, 378);
            loginPanel.TabIndex = 0;
            // 
            // backUser
            // 
            backUser.AutoSize = true;
            backUser.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            backUser.Location = new Point(89, 343);
            backUser.Name = "backUser";
            backUser.Size = new Size(178, 21);
            backUser.TabIndex = 5;
            backUser.Text = "← Back to Choose User";
            backUser.Click += backUser_Click;
            // 
            // btnLogin
            // 
            btnLogin.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.Location = new Point(29, 276);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(310, 53);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // showPass
            // 
            showPass.AutoSize = true;
            showPass.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            showPass.Location = new Point(197, 233);
            showPass.Name = "showPass";
            showPass.Size = new Size(142, 25);
            showPass.TabIndex = 3;
            showPass.Text = "Show Password";
            showPass.UseVisualStyleBackColor = true;
            showPass.CheckedChanged += showPass_CheckedChanged;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImage = (Image)resources.GetObject("pictureBox2.BackgroundImage");
            pictureBox2.BackgroundImageLayout = ImageLayout.Center;
            pictureBox2.Location = new Point(29, 185);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(43, 33);
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Center;
            pictureBox1.Location = new Point(29, 123);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(43, 33);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(78, 185);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(261, 33);
            txtPassword.TabIndex = 1;
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtUsername.Location = new Point(78, 123);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(261, 33);
            txtUsername.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(78, 35);
            label1.Name = "label1";
            label1.Size = new Size(215, 40);
            label1.TabIndex = 0;
            label1.Text = "ADMIN LOGIN";
            // 
            // iconPanel
            // 
            iconPanel.BackColor = Color.FromArgb(195, 17, 12);
            iconPanel.Controls.Add(pictureBox3);
            iconPanel.Location = new Point(358, 12);
            iconPanel.Name = "iconPanel";
            iconPanel.Size = new Size(84, 78);
            iconPanel.TabIndex = 1;
            // 
            // pictureBox3
            // 
            pictureBox3.BackgroundImage = (Image)resources.GetObject("pictureBox3.BackgroundImage");
            pictureBox3.BackgroundImageLayout = ImageLayout.Center;
            pictureBox3.Location = new Point(12, 13);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(60, 50);
            pictureBox3.TabIndex = 0;
            pictureBox3.TabStop = false;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 69, 214);
            ClientSize = new Size(796, 458);
            Controls.Add(iconPanel);
            Controls.Add(loginPanel);
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            Load += Login_Load;
            Shown += Login_Shown;
            loginPanel.ResumeLayout(false);
            loginPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            iconPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel loginPanel;
        private PictureBox pictureBox1;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private Label label1;
        private CheckBox showPass;
        private PictureBox pictureBox2;
        private Button btnLogin;
        private Label backUser;
        private Panel iconPanel;
        private PictureBox pictureBox3;
    }
}