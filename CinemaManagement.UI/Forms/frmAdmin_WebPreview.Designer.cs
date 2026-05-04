namespace CinemaManagement.UI.Forms
{
    partial class frmAdmin_WebPreview
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlTop = new Guna.UI2.WinForms.Guna2Panel();
            btnQuayLai = new Guna.UI2.WinForms.Guna2Button();
            btnTienLen = new Guna.UI2.WinForms.Guna2Button();
            btnTaiLai = new Guna.UI2.WinForms.Guna2Button();
            txtUrl = new Guna.UI2.WinForms.Guna2TextBox();
            btnDiToi = new Guna.UI2.WinForms.Guna2Button();
            webBrowser = new WebBrowser();
            pnlTop.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.WhiteSmoke;
            pnlTop.Controls.Add(btnQuayLai);
            pnlTop.Controls.Add(btnTienLen);
            pnlTop.Controls.Add(btnTaiLai);
            pnlTop.Controls.Add(txtUrl);
            pnlTop.Controls.Add(btnDiToi);
            pnlTop.CustomizableEdges = customizableEdges11;
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.ShadowDecoration.CustomizableEdges = customizableEdges12;
            pnlTop.Size = new Size(1302, 70);
            pnlTop.TabIndex = 0;
            // 
            // btnQuayLai
            // 
            btnQuayLai.BorderRadius = 5;
            btnQuayLai.CustomizableEdges = customizableEdges1;
            btnQuayLai.FillColor = Color.Teal;
            btnQuayLai.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnQuayLai.ForeColor = Color.White;
            btnQuayLai.Location = new Point(12, 12);
            btnQuayLai.Name = "btnQuayLai";
            btnQuayLai.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnQuayLai.Size = new Size(50, 45);
            btnQuayLai.TabIndex = 0;
            btnQuayLai.Text = "<";
            // 
            // btnTienLen
            // 
            btnTienLen.BorderRadius = 5;
            btnTienLen.CustomizableEdges = customizableEdges3;
            btnTienLen.FillColor = Color.Teal;
            btnTienLen.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnTienLen.ForeColor = Color.White;
            btnTienLen.Location = new Point(70, 12);
            btnTienLen.Name = "btnTienLen";
            btnTienLen.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnTienLen.Size = new Size(50, 45);
            btnTienLen.TabIndex = 1;
            btnTienLen.Text = ">";
            // 
            // btnTaiLai
            // 
            btnTaiLai.BorderRadius = 5;
            btnTaiLai.CustomizableEdges = customizableEdges5;
            btnTaiLai.FillColor = Color.DarkGoldenrod;
            btnTaiLai.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnTaiLai.ForeColor = Color.White;
            btnTaiLai.Location = new Point(128, 12);
            btnTaiLai.Name = "btnTaiLai";
            btnTaiLai.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnTaiLai.Size = new Size(50, 45);
            btnTaiLai.TabIndex = 2;
            btnTaiLai.Text = "↻";
            // 
            // txtUrl
            // 
            txtUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUrl.BorderRadius = 5;
            txtUrl.CustomizableEdges = customizableEdges7;
            txtUrl.DefaultText = "";
            txtUrl.Font = new Font("Segoe UI", 10F);
            txtUrl.Location = new Point(190, 12);
            txtUrl.Margin = new Padding(3, 4, 3, 4);
            txtUrl.Name = "txtUrl";
            txtUrl.PlaceholderText = "Nhập địa chỉ website rạp phim (Ví dụ: https://cgv.vn)";
            txtUrl.SelectedText = "";
            txtUrl.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtUrl.Size = new Size(992, 45);
            txtUrl.TabIndex = 3;
            // 
            // btnDiToi
            // 
            btnDiToi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDiToi.BorderRadius = 5;
            btnDiToi.CustomizableEdges = customizableEdges9;
            btnDiToi.FillColor = Color.Maroon;
            btnDiToi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDiToi.ForeColor = Color.White;
            btnDiToi.Location = new Point(1192, 12);
            btnDiToi.Name = "btnDiToi";
            btnDiToi.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnDiToi.Size = new Size(98, 45);
            btnDiToi.TabIndex = 4;
            btnDiToi.Text = "Truy cập";
            // 
            // webBrowser
            // 
            webBrowser.Dock = DockStyle.Fill;
            webBrowser.Location = new Point(0, 70);
            webBrowser.MinimumSize = new Size(20, 20);
            webBrowser.Name = "webBrowser";
            webBrowser.ScriptErrorsSuppressed = true;
            webBrowser.Size = new Size(1302, 630);
            webBrowser.TabIndex = 1;
            // 
            // frmAdmin_WebPreview
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Salmon;
            ClientSize = new Size(1302, 700);
            Controls.Add(webBrowser);
            Controls.Add(pnlTop);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmAdmin_WebPreview";
            Text = "Web Preview - Rạp phim ";
            pnlTop.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlTop;
        private Guna.UI2.WinForms.Guna2Button btnQuayLai;
        private Guna.UI2.WinForms.Guna2Button btnTienLen;
        private Guna.UI2.WinForms.Guna2Button btnTaiLai;
        private Guna.UI2.WinForms.Guna2TextBox txtUrl;
        private Guna.UI2.WinForms.Guna2Button btnDiToi;
        private System.Windows.Forms.WebBrowser webBrowser;
    }
}