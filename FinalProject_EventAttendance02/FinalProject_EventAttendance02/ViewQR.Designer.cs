namespace FinalProject_EventAttendance02
{
    partial class ViewQR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewQR));
            panel1 = new Panel();
            label1 = new Label();
            filterHistory = new Panel();
            btnSearchQR = new Button();
            txtSearchQR = new TextBox();
            label2 = new Label();
            dgvGenerateQR = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewImageColumn();
            label3 = new Label();
            QRPanel = new Panel();
            pictureQR = new PictureBox();
            btnDownloadQR = new Button();
            btnDeleteQR = new Button();
            panel1.SuspendLayout();
            filterHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGenerateQR).BeginInit();
            QRPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureQR).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(195, 17, 12);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(780, 52);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(318, 9);
            label1.Name = "label1";
            label1.Size = new Size(177, 30);
            label1.TabIndex = 0;
            label1.Text = "QR Code History";
            // 
            // filterHistory
            // 
            filterHistory.BackColor = Color.White;
            filterHistory.Controls.Add(btnSearchQR);
            filterHistory.Controls.Add(txtSearchQR);
            filterHistory.Controls.Add(label2);
            filterHistory.Location = new Point(239, 68);
            filterHistory.Name = "filterHistory";
            filterHistory.Size = new Size(529, 67);
            filterHistory.TabIndex = 1;
            // 
            // btnSearchQR
            // 
            btnSearchQR.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSearchQR.Location = new Point(371, 17);
            btnSearchQR.Name = "btnSearchQR";
            btnSearchQR.Size = new Size(145, 40);
            btnSearchQR.TabIndex = 2;
            btnSearchQR.Text = "Search";
            btnSearchQR.UseVisualStyleBackColor = true;
            btnSearchQR.Click += btnSearchQR_Click;
            // 
            // txtSearchQR
            // 
            txtSearchQR.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearchQR.Location = new Point(102, 19);
            txtSearchQR.Name = "txtSearchQR";
            txtSearchQR.Size = new Size(253, 33);
            txtSearchQR.TabIndex = 1;
            txtSearchQR.Text = "Search Student Name or ID";
            txtSearchQR.TextChanged += txtSearchQR_TextChanged;
            txtSearchQR.Enter += txtSearchQR_Enter;
            txtSearchQR.KeyDown += txtSearchQR_KeyDown;
            txtSearchQR.Leave += txtSearchQR_Leave;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(15, 18);
            label2.Name = "label2";
            label2.Size = new Size(81, 30);
            label2.TabIndex = 0;
            label2.Text = "Search:";
            // 
            // dgvGenerateQR
            // 
            dgvGenerateQR.AllowUserToAddRows = false;
            dgvGenerateQR.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGenerateQR.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 });
            dgvGenerateQR.Location = new Point(12, 192);
            dgvGenerateQR.Name = "dgvGenerateQR";
            dgvGenerateQR.ReadOnly = true;
            dgvGenerateQR.Size = new Size(756, 208);
            dgvGenerateQR.TabIndex = 2;
            dgvGenerateQR.CellClick += dgvGenerateQR_CellClick;
            dgvGenerateQR.SelectionChanged += dgvGenerateQR_SelectionChanged;
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
            Column5.HeaderText = "Photo";
            Column5.ImageLayout = DataGridViewImageCellLayout.Stretch;
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            Column5.Resizable = DataGridViewTriState.True;
            Column5.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(318, 149);
            label3.Name = "label3";
            label3.Size = new Size(175, 40);
            label3.TabIndex = 3;
            label3.Text = "QR Records";
            // 
            // QRPanel
            // 
            QRPanel.BackColor = Color.White;
            QRPanel.Controls.Add(pictureQR);
            QRPanel.Location = new Point(50, 406);
            QRPanel.Name = "QRPanel";
            QRPanel.Size = new Size(334, 275);
            QRPanel.TabIndex = 4;
            // 
            // pictureQR
            // 
            pictureQR.BorderStyle = BorderStyle.FixedSingle;
            pictureQR.Location = new Point(44, 13);
            pictureQR.Name = "pictureQR";
            pictureQR.Size = new Size(250, 250);
            pictureQR.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureQR.TabIndex = 0;
            pictureQR.TabStop = false;
            // 
            // btnDownloadQR
            // 
            btnDownloadQR.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDownloadQR.Image = (Image)resources.GetObject("btnDownloadQR.Image");
            btnDownloadQR.ImageAlign = ContentAlignment.MiddleLeft;
            btnDownloadQR.Location = new Point(460, 534);
            btnDownloadQR.Name = "btnDownloadQR";
            btnDownloadQR.Size = new Size(190, 40);
            btnDownloadQR.TabIndex = 5;
            btnDownloadQR.Text = "  Download QR";
            btnDownloadQR.UseVisualStyleBackColor = true;
            btnDownloadQR.Click += btnDownloadQR_Click;
            // 
            // btnDeleteQR
            // 
            btnDeleteQR.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDeleteQR.Image = (Image)resources.GetObject("btnDeleteQR.Image");
            btnDeleteQR.ImageAlign = ContentAlignment.MiddleLeft;
            btnDeleteQR.Location = new Point(460, 589);
            btnDeleteQR.Name = "btnDeleteQR";
            btnDeleteQR.Size = new Size(190, 40);
            btnDeleteQR.TabIndex = 5;
            btnDeleteQR.Text = "  Delete QR\r\n";
            btnDeleteQR.UseVisualStyleBackColor = true;
            btnDeleteQR.Click += btnDeleteQR_Click;
            // 
            // ViewQR
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 69, 214);
            ClientSize = new Size(780, 691);
            Controls.Add(btnDeleteQR);
            Controls.Add(btnDownloadQR);
            Controls.Add(QRPanel);
            Controls.Add(label3);
            Controls.Add(dgvGenerateQR);
            Controls.Add(filterHistory);
            Controls.Add(panel1);
            Name = "ViewQR";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "View QR History";
            Load += ViewQR_Load;
            Shown += ViewQR_Shown;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            filterHistory.ResumeLayout(false);
            filterHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGenerateQR).EndInit();
            QRPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureQR).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Panel filterHistory;
        private Button btnSearchQR;
        private TextBox txtSearchQR;
        private Label label2;
        private DataGridView dgvGenerateQR;
        private Label label3;
        private Panel QRPanel;
        private PictureBox pictureQR;
        private Button btnDownloadQR;
        private Button btnDeleteQR;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewImageColumn Column5;
    }
}