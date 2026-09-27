using System.Drawing.Drawing2D;
using static System.Windows.Forms.DataFormats;

namespace FinalProject_EventAttendance02
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            LoadFromDatabase();
        }

        private void btnStudent_Click(object sender, EventArgs e)
        {
            Student s = new Student();
            s.Show();
            this.Hide();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Hide();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            Color normal = ColorTranslator.FromHtml("#FFFBF1"); // white
            Color hover = ColorTranslator.FromHtml("#DED3B0"); // slightly darker

            CurvePanel(userPanel, 25);

            CurveButton(btnAdmin);
            CurveButton(btnStudent);

            StyleFlatButton(btnAdmin);
            StyleFlatButton(btnStudent);

            AddHoverEffect(btnAdmin, normal, hover);
            AddHoverEffect(btnStudent, normal, hover);
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

        private void LoadFromDatabase()
        {
            if (!DbHelper.TestConnection())
            {
                MessageBox.Show("Cannot connect to MySQL. Running in offline mode.",
                                "DB Offline", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            generateQR.QRList = StudentDB.GetAll();

            Event.EventList = EventDB.GetAll();

            Attendance.AttendanceList = AttendanceDB.GetAll();
        }

    }
}
