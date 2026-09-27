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

namespace FinalProject_EventAttendance02
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

       
        private void Login_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;

            this.AcceptButton = btnLogin;

            
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }

        private void Login_Shown(object sender, EventArgs e)
        {
            Color normal = ColorTranslator.FromHtml("#FFFBF1"); // white
            Color hover = ColorTranslator.FromHtml("#DED3B0"); // slightly darker

            CurvePanel(loginPanel, 25);

            CurveButton(btnLogin);
            StyleFlatButton(btnLogin);
            AddHoverEffect(btnLogin, normal, hover);

            MakeCirclePanel(iconPanel);
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

        private void MakeCirclePanel(Panel pnl)
        {
            pnl.Region?.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, pnl.Width - 1, pnl.Height - 1);
            pnl.Region = new Region(path);
        }

        private void backUser_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }

        private void showPass_CheckedChanged(object sender, EventArgs e)
        {
            if (showPass.Checked)
            {
                txtPassword.UseSystemPasswordChar = false; // show password
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true; // hide password
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
        string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter username and password.", "Missing Info",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AdminDB.Login(txtUsername.Text.Trim(), txtPassword.Text.Trim()))
            {
               
                MessageBox.Show("Login successful!", "Welcome",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                Admin a = new Admin();
                a.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password!", "Login Failed",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
