namespace Project_Error_404
{
	partial class Events
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
            panel1 = new Panel();
            button4 = new Button();
            button3 = new Button();
            label4 = new Label();
            button2 = new Button();
            button1 = new Button();
            panel2 = new Panel();
            dataGridView1 = new DataGridView();
            panel5 = new Panel();
            textBox1 = new TextBox();
            button7 = new Button();
            button6 = new Button();
            button5 = new Button();
            panel4 = new Panel();
            label5 = new Label();
            panel3 = new Panel();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            Event = new DataGridViewTextBoxColumn();
            Date = new DataGridViewTextBoxColumn();
            location = new DataGridViewTextBoxColumn();
            button8 = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.GradientInactiveCaption;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(button4);
            panel1.Controls.Add(button3);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(5, 59);
            panel1.Name = "panel1";
            panel1.Size = new Size(193, 387);
            panel1.TabIndex = 0;
            // 
            // button4
            // 
            button4.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button4.Location = new Point(21, 314);
            button4.Name = "button4";
            button4.Size = new Size(152, 42);
            button4.TabIndex = 0;
            button4.Text = "REPORT";
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.Location = new Point(21, 243);
            button3.Name = "button3";
            button3.Size = new Size(152, 42);
            button3.TabIndex = 0;
            button3.Text = "ATTENDANCE";
            button3.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(7, 12);
            label4.Name = "label4";
            label4.Size = new Size(118, 22);
            label4.TabIndex = 0;
            label4.Text = "DASHBOARD :";
            // 
            // button2
            // 
            button2.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Location = new Point(21, 178);
            button2.Name = "button2";
            button2.Size = new Size(152, 42);
            button2.TabIndex = 0;
            button2.Text = "EVENTS";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(21, 117);
            button1.Name = "button1";
            button1.Size = new Size(152, 42);
            button1.TabIndex = 0;
            button1.Text = "USER";
            button1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.GradientInactiveCaption;
            panel2.BorderStyle = BorderStyle.Fixed3D;
            panel2.Controls.Add(dataGridView1);
            panel2.Controls.Add(panel5);
            panel2.Controls.Add(panel4);
            panel2.Location = new Point(204, 59);
            panel2.Name = "panel2";
            panel2.Size = new Size(928, 387);
            panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = SystemColors.GradientInactiveCaption;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Event, Date, location });
            dataGridView1.Location = new Point(9, 117);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(911, 252);
            dataGridView1.TabIndex = 2;
            // 
            // panel5
            // 
            panel5.Controls.Add(textBox1);
            panel5.Controls.Add(button7);
            panel5.Controls.Add(button6);
            panel5.Controls.Add(button8);
            panel5.Controls.Add(button5);
            panel5.Location = new Point(11, 64);
            panel5.Name = "panel5";
            panel5.Size = new Size(909, 37);
            panel5.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Arial Narrow", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(708, 6);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(198, 28);
            textBox1.TabIndex = 1;
            textBox1.Text = "Search";
            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;
            // 
            // button7
            // 
            button7.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button7.Location = new Point(357, 0);
            button7.Name = "button7";
            button7.Size = new Size(112, 34);
            button7.TabIndex = 0;
            button7.Text = "DELETE";
            button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button6.Location = new Point(239, 0);
            button6.Name = "button6";
            button6.Size = new Size(112, 34);
            button6.TabIndex = 0;
            button6.Text = "UPDATE";
            button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button5.Location = new Point(3, 0);
            button5.Name = "button5";
            button5.Size = new Size(112, 34);
            button5.TabIndex = 0;
            button5.Text = "CREATE";
            button5.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            panel4.Controls.Add(label5);
            panel4.Location = new Point(9, 12);
            panel4.Name = "panel4";
            panel4.Size = new Size(908, 41);
            panel4.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(418, 10);
            label5.Name = "label5";
            label5.Size = new Size(174, 22);
            label5.TabIndex = 0;
            label5.Text = "EVENT MANAGEMENT";
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.GradientInactiveCaption;
            panel3.BorderStyle = BorderStyle.Fixed3D;
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(label1);
            panel3.Location = new Point(5, 6);
            panel3.Name = "panel3";
            panel3.Size = new Size(1127, 47);
            panel3.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(1042, 12);
            label3.Name = "label3";
            label3.Size = new Size(74, 22);
            label3.TabIndex = 2;
            label3.Text = "LOGOUT";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(894, 12);
            label2.Name = "label2";
            label2.Size = new Size(142, 22);
            label2.TabIndex = 1;
            label2.Text = "WELCOME ADMIN";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(7, 12);
            label1.Name = "label1";
            label1.Size = new Size(126, 22);
            label1.TabIndex = 0;
            label1.Text = "EVENT SYSTEM";
            // 
            // Event
            // 
            Event.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Event.HeaderText = "Event Name";
            Event.MinimumWidth = 8;
            Event.Name = "Event";
            // 
            // Date
            // 
            Date.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Date.HeaderText = "Date";
            Date.MinimumWidth = 8;
            Date.Name = "Date";
            // 
            // location
            // 
            location.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            location.HeaderText = "Location";
            location.MinimumWidth = 8;
            location.Name = "location";
            // 
            // button8
            // 
            button8.Font = new Font("Arial Narrow", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button8.Location = new Point(121, 0);
            button8.Name = "button8";
            button8.Size = new Size(112, 34);
            button8.TabIndex = 0;
            button8.Text = "READ";
            button8.UseVisualStyleBackColor = true;
            // 
            // Events
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientActiveCaption;
            ClientSize = new Size(1138, 450);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Events";
            Text = "Admin";
            Load += Admin_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
		private Panel panel2;
		private Panel panel3;
		private Label label1;
		private Label label2;
		private Button button4;
		private Button button3;
		private Button button2;
		private Button button1;
		private Label label3;
		private Label label4;
		private Panel panel4;
		private Label label5;
		private Panel panel5;
		private TextBox textBox1;
		private Button button7;
		private Button button6;
		private Button button5;
		private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Event;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn location;
        private Button button8;
    }
}