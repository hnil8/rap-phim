namespace CinemaManagement.UI.Forms
{
    partial class frmLogin
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            txtTenDangNhap = new Guna.UI2.WinForms.Guna2TextBox();
            btnLogin = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // guna2PictureBox1
            // 
            guna2PictureBox1.CustomizableEdges = customizableEdges1;
            guna2PictureBox1.Image = Properties.Resources.Gemini_Generated_Image_pgdn9mpgdn9mpgdn;
            guna2PictureBox1.ImageRotate = 0F;
            guna2PictureBox1.Location = new Point(0, 1);
            guna2PictureBox1.Margin = new Padding(3, 4, 3, 4);
            guna2PictureBox1.Name = "guna2PictureBox1";
            guna2PictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2PictureBox1.Size = new Size(642, 627);
            guna2PictureBox1.TabIndex = 4;
            guna2PictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Showcard Gothic", 28.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Transparent;
            label1.Location = new Point(690, 50);
            label1.Name = "label1";
            label1.Size = new Size(577, 59);
            label1.TabIndex = 5;
            label1.Text = "ĐĂNG NHẬP HỆ THỐNG";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Salmon;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label2.ForeColor = SystemColors.ControlLightLight;
            label2.Location = new Point(732, 226);
            label2.Name = "label2";
            label2.Size = new Size(171, 41);
            label2.TabIndex = 6;
            label2.Text = "Tài Khoản :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Salmon;
            label3.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            label3.ForeColor = SystemColors.ControlLightLight;
            label3.Location = new Point(732, 307);
            label3.Name = "label3";
            label3.Size = new Size(170, 41);
            label3.TabIndex = 7;
            label3.Text = "Mật Khẩu :";
            // 
            // txtPassword
            // 
            txtPassword.BorderRadius = 15;
            txtPassword.CustomizableEdges = customizableEdges3;
            txtPassword.DefaultText = "";
            txtPassword.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPassword.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPassword.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPassword.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPassword.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassword.Font = new Font("Segoe UI", 9F);
            txtPassword.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassword.Location = new Point(909, 307);
            txtPassword.Margin = new Padding(3, 4, 3, 4);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Nhập Mật khẩu...";
            txtPassword.SelectedText = "";
            txtPassword.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtPassword.Size = new Size(251, 51);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.TextChanged += guna2TextBox2_TextChanged;
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.BorderRadius = 15;
            txtTenDangNhap.CustomizableEdges = customizableEdges5;
            txtTenDangNhap.DefaultText = "";
            txtTenDangNhap.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtTenDangNhap.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtTenDangNhap.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtTenDangNhap.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtTenDangNhap.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTenDangNhap.Font = new Font("Segoe UI", 9F);
            txtTenDangNhap.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTenDangNhap.Location = new Point(909, 226);
            txtTenDangNhap.Margin = new Padding(3, 4, 3, 4);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.PlaceholderText = "\"Nhập Email hoặc Tài khoản..";
            txtTenDangNhap.SelectedText = "";
            txtTenDangNhap.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtTenDangNhap.Size = new Size(280, 54);
            txtTenDangNhap.TabIndex = 0;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.Salmon;
            btnLogin.BorderRadius = 15;
            btnLogin.CustomizableEdges = customizableEdges7;
            btnLogin.DisabledState.BorderColor = Color.DarkGray;
            btnLogin.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLogin.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLogin.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLogin.FillColor = Color.Maroon;
            btnLogin.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(876, 397);
            btnLogin.Margin = new Padding(3, 4, 3, 4);
            btnLogin.Name = "btnLogin";
            btnLogin.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnLogin.Size = new Size(152, 47);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "ĐĂNG NHẬP";
            btnLogin.Click += btnLogin_Click;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Salmon;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(1301, 627);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(guna2PictureBox1);
            Controls.Add(label1);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(txtTenDangNhap);
            Font = new Font("Times New Roman", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmLogin";
            Load += frmLogin_Load;
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtTenDangNhap;
        private Guna.UI2.WinForms.Guna2Button btnLogin;
    }
}