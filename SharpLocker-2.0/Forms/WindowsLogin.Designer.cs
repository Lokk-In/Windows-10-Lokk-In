namespace SharpLocker_2._0
{
    partial class WindowsLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowsLogin));
            this.UserIconPictureBox = new System.Windows.Forms.PictureBox();
            this.UserNameLabel = new System.Windows.Forms.Label();
            this.LockedLabel = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.LoginButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UserIconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // UserIconPictureBox
            // 
            this.UserIconPictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UserIconPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.UserIconPictureBox.Location = new System.Drawing.Point(345, 31);
            this.UserIconPictureBox.Name = "UserIconPictureBox";
            this.UserIconPictureBox.Size = new System.Drawing.Size(199, 199);
            this.UserIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.UserIconPictureBox.TabIndex = 0;
            this.UserIconPictureBox.TabStop = false;
            // 
            // UserNameLabel
            // 
            this.UserNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UserNameLabel.AutoSize = true;
            this.UserNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.UserNameLabel.Font = new System.Drawing.Font("Segoe UI", 33F);
            this.UserNameLabel.ForeColor = System.Drawing.Color.White;
            this.UserNameLabel.Location = new System.Drawing.Point(244, 233);
            this.UserNameLabel.MinimumSize = new System.Drawing.Size(403, 0);
            this.UserNameLabel.Name = "UserNameLabel";
            this.UserNameLabel.Size = new System.Drawing.Size(403, 60);
            this.UserNameLabel.TabIndex = 5;
            this.UserNameLabel.Text = "UserName";
            this.UserNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LockedLabel
            // 
            this.LockedLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LockedLabel.AutoSize = true;
            this.LockedLabel.BackColor = System.Drawing.Color.Transparent;
            this.LockedLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LockedLabel.ForeColor = System.Drawing.Color.White;
            this.LockedLabel.Location = new System.Drawing.Point(404, 293);
            this.LockedLabel.Name = "LockedLabel";
            this.LockedLabel.Size = new System.Drawing.Size(71, 25);
            this.LockedLabel.TabIndex = 9;
            this.LockedLabel.Text = "Locked";
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PasswordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordTextBox.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.PasswordTextBox.Location = new System.Drawing.Point(252, 324);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(326, 31);
            this.PasswordTextBox.TabIndex = 10;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            this.PasswordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordTextBox_KeyDown);
            // 
            // LoginButton
            // 
            this.LoginButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LoginButton.AutoSize = true;
            this.LoginButton.BackColor = System.Drawing.Color.Transparent;
            this.LoginButton.BackgroundImage = global::SharpLocker_2._0.Properties.Resources.login;
            this.LoginButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LoginButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.LoginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginButton.Location = new System.Drawing.Point(580, 321);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(38, 38);
            this.LoginButton.TabIndex = 11;
            this.LoginButton.UseVisualStyleBackColor = false;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // WindowsLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.LockedLabel);
            this.Controls.Add(this.UserNameLabel);
            this.Controls.Add(this.UserIconPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WindowsLogin";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Windows Login";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WindowsLogin_FormClosing);
            this.Load += new System.EventHandler(this.WindowsLogin_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.WindowsLogin_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.UserIconPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox UserIconPictureBox;
        private System.Windows.Forms.Label UserNameLabel;
        private System.Windows.Forms.Label LockedLabel;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Button LoginButton;
    }
}

