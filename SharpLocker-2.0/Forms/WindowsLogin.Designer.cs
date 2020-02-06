﻿namespace SharpLocker_2._0
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
            this.UserNameLabel = new System.Windows.Forms.Label();
            this.CapsLockLabel = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.EaseOfAccessButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.LoginButton = new System.Windows.Forms.Button();
            this.UserIconPictureBox = new SharpLocker_2._0.Classes.RoundPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.UserIconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // UserNameLabel
            // 
            this.UserNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UserNameLabel.AutoSize = true;
            this.UserNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.UserNameLabel.Font = new System.Drawing.Font("Segoe UI", 33F);
            this.UserNameLabel.ForeColor = System.Drawing.Color.White;
            this.UserNameLabel.Location = new System.Drawing.Point(260, 326);
            this.UserNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UserNameLabel.MinimumSize = new System.Drawing.Size(537, 0);
            this.UserNameLabel.Name = "UserNameLabel";
            this.UserNameLabel.Size = new System.Drawing.Size(537, 74);
            this.UserNameLabel.TabIndex = 5;
            this.UserNameLabel.Text = "UserName";
            this.UserNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CapsLockLabel
            // 
            this.CapsLockLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CapsLockLabel.AutoSize = true;
            this.CapsLockLabel.BackColor = System.Drawing.Color.Transparent;
            this.CapsLockLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CapsLockLabel.ForeColor = System.Drawing.Color.White;
            this.CapsLockLabel.Location = new System.Drawing.Point(456, 470);
            this.CapsLockLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CapsLockLabel.Name = "CapsLockLabel";
            this.CapsLockLabel.Size = new System.Drawing.Size(148, 25);
            this.CapsLockLabel.TabIndex = 9;
            this.CapsLockLabel.Text = "Caps Lock is on";
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PasswordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordTextBox.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.PasswordTextBox.Location = new System.Drawing.Point(285, 422);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PasswordTextBox.MinimumSize = new System.Drawing.Size(436, 32);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.ShortcutsEnabled = false;
            this.PasswordTextBox.Size = new System.Drawing.Size(436, 44);
            this.PasswordTextBox.TabIndex = 0;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            this.PasswordTextBox.Click += new System.EventHandler(this.PasswordTextBox_Click);
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            this.PasswordTextBox.DoubleClick += new System.EventHandler(this.PasswordTextBox_Click);
            this.PasswordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordTextBox_KeyDown);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.AutoSize = true;
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.BackgroundImage = global::SharpLocker_2._0.Properties.Resources.ethernet;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(843, 478);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(47, 43);
            this.button3.TabIndex = 12;
            this.button3.UseVisualStyleBackColor = false;
            // 
            // EaseOfAccessButton
            // 
            this.EaseOfAccessButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EaseOfAccessButton.AutoSize = true;
            this.EaseOfAccessButton.BackColor = System.Drawing.Color.Transparent;
            this.EaseOfAccessButton.BackgroundImage = global::SharpLocker_2._0.Properties.Resources.easeOfAcess;
            this.EaseOfAccessButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.EaseOfAccessButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.EaseOfAccessButton.FlatAppearance.BorderSize = 0;
            this.EaseOfAccessButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EaseOfAccessButton.Font = new System.Drawing.Font("Segoe MDL2 Assets", 23.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EaseOfAccessButton.ForeColor = System.Drawing.Color.White;
            this.EaseOfAccessButton.Location = new System.Drawing.Point(913, 478);
            this.EaseOfAccessButton.Margin = new System.Windows.Forms.Padding(4);
            this.EaseOfAccessButton.Name = "EaseOfAccessButton";
            this.EaseOfAccessButton.Size = new System.Drawing.Size(47, 43);
            this.EaseOfAccessButton.TabIndex = 11;
            this.EaseOfAccessButton.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::SharpLocker_2._0.Properties.Resources.power;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(984, 478);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(47, 43);
            this.button1.TabIndex = 10;
            this.button1.UseVisualStyleBackColor = false;
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
            this.LoginButton.Location = new System.Drawing.Point(723, 421);
            this.LoginButton.Margin = new System.Windows.Forms.Padding(4);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(51, 47);
            this.LoginButton.TabIndex = 1;
            this.LoginButton.UseVisualStyleBackColor = false;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            this.LoginButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordTextBox_KeyDown);
            // 
            // UserIconPictureBox
            // 
            this.UserIconPictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UserIconPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.UserIconPictureBox.Location = new System.Drawing.Point(400, 64);
            this.UserIconPictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.UserIconPictureBox.Name = "UserIconPictureBox";
            this.UserIconPictureBox.Size = new System.Drawing.Size(265, 245);
            this.UserIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.UserIconPictureBox.TabIndex = 13;
            this.UserIconPictureBox.TabStop = false;
            // 
            // WindowsLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.UserIconPictureBox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.EaseOfAccessButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.CapsLockLabel);
            this.Controls.Add(this.UserNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
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
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordTextBox_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.UserIconPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label UserNameLabel;
        private System.Windows.Forms.Label CapsLockLabel;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button EaseOfAccessButton;
        private System.Windows.Forms.Button button3;
        private Classes.RoundPictureBox UserIconPictureBox;
    }
}

