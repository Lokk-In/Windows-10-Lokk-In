using SharpLocker_2._0.Controls;
using SharpLocker_2._0.Interfaces;
using SharpLocker_2._0.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpLocker_2._0.Classes
{
    internal static class ControlFactory
    {
        // create a "other user" control with given properties, then place it on screen
        public static Panel CreateUserPanel(string user, int panelCount, string displayName, Image userIcon)
        {
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

            Panel p = CreateUserPanel(user, displayName, panelX, panelY, panelHeight, panelWidth, panelCount);
            RoundPictureBox pb = CreateUserPictureBox(pictureBoxX, pictureBoxY, pictureBoxWidth, pictureBoxHeight, p, userIcon);
            Button b = CreateUserButton(user, buttonWidth, buttonHeight, buttonX, buttonY, panelCount, p);

            //SetUserIconByName(user, pb);

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

        public static Panel CreateLanguagePanel(List<ILanguage> languages, int x, int y, Language currentLanguage)
        {
            int panelWidth = 300;
            int panelSingleHeight = 40;
            int panelSingleOffset = 5;
            int panelHeight = panelSingleHeight * languages.Count() + panelSingleOffset * 2;
            x = x - panelWidth;
            y = y - panelHeight;

            Panel p = new Panel()
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(x, y),
                Size = new Size(panelWidth, panelHeight),
                BorderStyle = BorderStyle.None,
                BackColor = SystemColors.WindowFrame,
                BackgroundImageLayout = ImageLayout.Stretch,
                TabStop = false
            };

            languages.Reverse();

            for (int i = 0; i < languages.Count(); i++)
            {
                p.Controls.Add(CreateSingleLanguagePanel(languages[i], i, panelWidth, panelSingleHeight, panelSingleOffset, currentLanguage, p));
            }

            p.MouseLeave += (s, e) =>
            {
                Rectangle hitbox = new Rectangle(x, y, panelWidth, panelHeight);
                if (!hitbox.Contains(Cursor.Position.X, Cursor.Position.Y)) p.Dispose();
            };

            return p;
        }

        private static Panel CreateSingleLanguagePanel(ILanguage language, int count, int width, int height, int offset, Language currentLanguage, Panel parent)
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

            p.MouseLeave += (s, e) =>
            {
                Rectangle hitbox = new Rectangle(parent.Location.X, parent.Location.Y, parent.Width, parent.Height);
                if (!hitbox.Contains(Cursor.Position.X, Cursor.Position.Y)) parent.Dispose();
            };

            p.Controls.Add(GetSingleLanguageLabel(width, height, language, currentLanguage));

            return p;
        }

        private static Label GetSingleLanguageLabel(int width, int height, ILanguage language, Language currentLanguage)
        {

            CultureInfo c = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x => x.ThreeLetterWindowsLanguageName == language.Identifier).FirstOrDefault();

            if (c is null) c = CultureInfo.CurrentCulture;

            Label l = new Label()
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(0, 0),
                Size = new Size(width, height),
                BorderStyle = BorderStyle.None,
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch,
                TabStop = false,
                Font = new Font("Segoe UI", 11.25f, FontStyle.Bold),
                ForeColor = Color.White,
                Text = $"{c.ThreeLetterWindowsLanguageName}      {c.NativeName}{Environment.NewLine}         { c.KeyboardLayoutId}"
            };

            return l;
        }

    }
}
