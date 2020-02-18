using Windows10LokkIn.Controls;
using Windows10LokkIn.Interfaces;
using Windows10LokkIn.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Windows10LokkIn.Classes
{
    /// <summary>
    /// Creates controls needed
    /// </summary>
    internal static class ControlFactory
    {
        /// <summary>
        /// create a "other user" control with given properties, then place it on screen
        /// </summary>
        /// <param name="user"></param>
        /// <param name="panelCount"></param>
        /// <param name="displayName"></param>
        /// <param name="userIcon"></param>
        /// <returns></returns>
        public static Panel CreateUserPanel(string user, int panelCount, string displayName, Image userIcon)
        {
            // specifiy size and position
            int panelX = 30;
            int panelY = 380;
            int panelWidth = 220;
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

            // generate controls
            Panel p = CreateUserPanel(user, displayName, panelX, panelY, panelHeight, panelWidth, panelCount);
            RoundPictureBox pb = CreateUserPictureBox(pictureBoxX, pictureBoxY, pictureBoxWidth, pictureBoxHeight, p, userIcon);
            Button b = CreateUserButton(user, buttonWidth, buttonHeight, buttonX, buttonY, panelCount, p);

            p.Controls.Add(pb);
            p.Controls.Add(b);

            return p;
        }

        private static Panel CreateUserPanel(string user, string displayName, int panelX, int panelY, int panelHeight, int panelWidth, int panelCount)

        {
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

            if (!(user is null) && user == displayName) p.BackgroundImage = Properties.Resources.defaultButtonBackground;

            // set hover over colors
            p.MouseEnter += (s, e) =>
            {
                p.BackColor = Color.LightGray;
            };

            p.MouseLeave += (s, e) =>
            {
                p.BackColor = Color.Transparent;
            };

            return p;
        }

        private static RoundPictureBox CreateUserPictureBox(int pictureBoxX, int pictureBoxY, int pictureBoxWidth, int pictureBoxHeight, Panel p, Image userIcon)
        {
            RoundPictureBox pb = new RoundPictureBox()
            {
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                BackgroundImageLayout = ImageLayout.Stretch,
                Size = new Size(pictureBoxWidth, pictureBoxHeight),
                Location = new Point(pictureBoxX, pictureBoxY),
                TabStop = false,
                Image = userIcon
            };

            // set hover over colors
            pb.MouseEnter += (s, e) =>
            {
                p.BackColor = Color.LightGray;
            };

            pb.MouseLeave += (s, e) =>
            {
                p.BackColor = Color.Transparent;
            };

            return pb;
        }

        private static Button CreateUserButton(string user, int buttonWidth, int buttonHeight, int buttonX, int buttonY, int panelCount, Panel p)
        {
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
                TabStop = false,
                Name = panelCount.ToString()
            };

            // set hover over colors
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

            return b;
        }

        /// <summary>
        /// Creats the panel that can bee seen if the language button is pressed
        /// </summary>
        /// <param name="name"></param>
        /// <param name="languages"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="currentLanguage"></param>
        /// <returns></returns>
        public static Panel CreateLanguagePanel(string name, List<ILanguage> languages, int x, int y, Language currentLanguage)
        {
            // specify size and position
            int panelWidth = 300;
            int panelSingleHeight = 40;
            int panelSingleOffset = 5;
            int panelHeight = panelSingleHeight * languages.Count() + panelSingleOffset * 2;
            x -= panelWidth;
            y -= panelHeight;

            Panel p = new Panel()
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(x, y),
                Size = new Size(panelWidth, panelHeight),
                BorderStyle = BorderStyle.None,
                BackColor = SystemColors.WindowFrame,
                BackgroundImageLayout = ImageLayout.Stretch,
                TabStop = false,
                Name = name
            };

            languages.Reverse(); // reverse, so that current language is on top

            for (int i = 0; i < languages.Count(); i++)
            {
                p.Controls.Add(CreateSingleLanguagePanel(languages[i], i, panelWidth, panelSingleHeight, panelSingleOffset, currentLanguage));
            }

            return p;
        }

        private static Panel CreateSingleLanguagePanel(ILanguage language, int count, int width, int height, int offset, Language currentLanguage)
        {
            Panel p = new Panel()
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(0, offset + count * height),
                Size = new Size(width, height),
                BorderStyle = BorderStyle.None,
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch,
                TabStop = false
            };

            if (language.Identifier == currentLanguage.LanguageCode) p.BackgroundImage = Properties.Resources.defaultButtonBackground;

            CultureInfo c = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(i => i.ThreeLetterWindowsLanguageName == language.Identifier).FirstOrDefault();

            if (c is null) c = CultureInfo.CurrentCulture;

            p.Controls.Add(GetSingleLanguageLabel(p, (int)(width * 0.2), height, 0, 0, c.ThreeLetterWindowsLanguageName, true, language.Identifier + "1"));
            p.Controls.Add(GetSingleLanguageLabel(p, (int)(width * 0.8), height, (int)(width * 0.2), 0, $"{c.NativeName}{Environment.NewLine}{InputLanguage.CurrentInputLanguage.LayoutName}-{currentLanguage.Keyboard}", false, language.Identifier + "2"));

            return p;
        }

        private static Label GetSingleLanguageLabel(Panel parent, int width, int height, int x, int y, string text, bool bold, string name)
        {

            Label l = new Label()
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(x + 1, y),
                Size = new Size(width - 2, height),
                BorderStyle = BorderStyle.None,
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch,
                TabStop = false,
                Font = new Font("Segoe UI", 11.25f),
                ForeColor = Color.White,
                Text = text,
                Name = name
            };

            // hover over colors
            l.MouseEnter += (s, e) =>
            {
                parent.BackColor = Color.LightGray;
            };

            l.MouseLeave += (s, e) =>
            {
                parent.BackColor = Color.Transparent;
            };

            if (bold) l.Font = new Font("Segoe UI", 11.25f, FontStyle.Bold);

            return l;
        }

    }
}
