namespace CinemaManagement.UI.Forms
{
    partial class frmAdmin_DoanhThu
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            cbTieuChi = new Guna.UI2.WinForms.Guna2ComboBox();
            dtpTuNgay = new Guna.UI2.WinForms.Guna2DateTimePicker();
            dtpDenNgay = new Guna.UI2.WinForms.Guna2DateTimePicker();
            btnThongKe = new Guna.UI2.WinForms.Guna2Button();
            lblTongDoanhThu = new Label();
            pnlChartArea = new Guna.UI2.WinForms.Guna2Panel();
            label1 = new Label();
            label4 = new Label();
            label5 = new Label();
            label2 = new Label();
            dgvDoanhThu = new Guna.UI2.WinForms.Guna2DataGridView();
            btnXuatExcel = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)dgvDoanhThu).BeginInit();
            SuspendLayout();
            // 
            // cbTieuChi
            // 
            cbTieuChi.BackColor = Color.Transparent;
            cbTieuChi.BorderRadius = 10;
            cbTieuChi.CustomizableEdges = customizableEdges1;
            cbTieuChi.DrawMode = DrawMode.OwnerDrawFixed;
            cbTieuChi.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTieuChi.FillColor = Color.Linen;
            cbTieuChi.FocusedColor = Color.FromArgb(94, 148, 255);
            cbTieuChi.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbTieuChi.Font = new Font("Segoe UI", 10F);
            cbTieuChi.ForeColor = Color.FromArgb(68, 88, 112);
            cbTieuChi.ItemHeight = 30;
            cbTieuChi.Items.AddRange(new object[] { "Thống kê theo ngày", "Thống kê theo phim" });
            cbTieuChi.Location = new Point(109, 18);
            cbTieuChi.Name = "cbTieuChi";
            cbTieuChi.ShadowDecoration.CustomizableEdges = customizableEdges2;
            cbTieuChi.Size = new Size(200, 36);
            cbTieuChi.StartIndex = 0;
            cbTieuChi.TabIndex = 0;
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.BorderColor = SystemColors.ControlLightLight;
            dtpTuNgay.BorderRadius = 10;
            dtpTuNgay.Checked = true;
            dtpTuNgay.CustomizableEdges = customizableEdges3;
            dtpTuNgay.FillColor = Color.Linen;
            dtpTuNgay.Font = new Font("Segoe UI", 9F);
            dtpTuNgay.Format = DateTimePickerFormat.Long;
            dtpTuNgay.Location = new Point(109, 60);
            dtpTuNgay.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpTuNgay.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.ShadowDecoration.CustomizableEdges = customizableEdges4;
            dtpTuNgay.Size = new Size(200, 36);
            dtpTuNgay.TabIndex = 1;
            dtpTuNgay.Value = new DateTime(2026, 4, 12, 16, 55, 20, 2);
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.BorderColor = Color.White;
            dtpDenNgay.BorderRadius = 10;
            dtpDenNgay.Checked = true;
            dtpDenNgay.CustomizableEdges = customizableEdges5;
            dtpDenNgay.FillColor = Color.Linen;
            dtpDenNgay.Font = new Font("Segoe UI", 9F);
            dtpDenNgay.Format = DateTimePickerFormat.Long;
            dtpDenNgay.Location = new Point(109, 102);
            dtpDenNgay.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpDenNgay.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.ShadowDecoration.CustomizableEdges = customizableEdges6;
            dtpDenNgay.Size = new Size(200, 37);
            dtpDenNgay.TabIndex = 2;
            dtpDenNgay.Value = new DateTime(2026, 4, 12, 16, 55, 32, 366);
            // 
            // btnThongKe
            // 
            btnThongKe.BorderRadius = 10;
            btnThongKe.CustomizableEdges = customizableEdges7;
            btnThongKe.DisabledState.BorderColor = Color.DarkGray;
            btnThongKe.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThongKe.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThongKe.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThongKe.FillColor = Color.Maroon;
            btnThongKe.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThongKe.ForeColor = Color.White;
            btnThongKe.Location = new Point(327, 102);
            btnThongKe.Name = "btnThongKe";
            btnThongKe.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnThongKe.Size = new Size(135, 38);
            btnThongKe.TabIndex = 3;
            btnThongKe.Text = "XEM BÁO CÁO";
            btnThongKe.Click += btnThongKe_Click;
            // 
            // lblTongDoanhThu
            // 
            lblTongDoanhThu.AutoSize = true;
            lblTongDoanhThu.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongDoanhThu.ForeColor = SystemColors.ControlLightLight;
            lblTongDoanhThu.Location = new Point(717, 60);
            lblTongDoanhThu.Name = "lblTongDoanhThu";
            lblTongDoanhThu.Size = new Size(366, 38);
            lblTongDoanhThu.TabIndex = 4;
            lblTongDoanhThu.Text = "TỔNG DOANH THU: 0 VNĐ";
            // 
            // pnlChartArea
            // 
            pnlChartArea.CustomizableEdges = customizableEdges9;
            pnlChartArea.Location = new Point(10, 188);
            pnlChartArea.Name = "pnlChartArea";
            pnlChartArea.ShadowDecoration.CustomizableEdges = customizableEdges10;
            pnlChartArea.Size = new Size(668, 444);
            pnlChartArea.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(10, 165);
            label1.Name = "label1";
            label1.Size = new Size(189, 20);
            label1.TabIndex = 7;
            label1.Text = "Biểu đồ Doanh thu (VND)";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.ControlLightLight;
            label4.Location = new Point(26, 60);
            label4.Name = "label4";
            label4.Size = new Size(74, 20);
            label4.TabIndex = 10;
            label4.Text = "Từ ngày :";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = SystemColors.ControlLightLight;
            label5.Location = new Point(26, 102);
            label5.Name = "label5";
            label5.Size = new Size(83, 20);
            label5.TabIndex = 11;
            label5.Text = "Đến ngày :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ControlLightLight;
            label2.Location = new Point(10, 18);
            label2.Name = "label2";
            label2.Size = new Size(98, 28);
            label2.TabIndex = 8;
            label2.Text = "Tiêu chí :";
            label2.Click += label2_Click;
            // 
            // dgvDoanhThu
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvDoanhThu.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvDoanhThu.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvDoanhThu.ColumnHeadersHeight = 4;
            dgvDoanhThu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvDoanhThu.DefaultCellStyle = dataGridViewCellStyle3;
            dgvDoanhThu.GridColor = Color.FromArgb(231, 229, 255);
            dgvDoanhThu.Location = new Point(684, 188);
            dgvDoanhThu.Name = "dgvDoanhThu";
            dgvDoanhThu.RowHeadersVisible = false;
            dgvDoanhThu.RowHeadersWidth = 51;
            dgvDoanhThu.Size = new Size(602, 444);
            dgvDoanhThu.TabIndex = 12;
            dgvDoanhThu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvDoanhThu.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvDoanhThu.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvDoanhThu.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvDoanhThu.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvDoanhThu.ThemeStyle.BackColor = Color.White;
            dgvDoanhThu.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvDoanhThu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvDoanhThu.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvDoanhThu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgvDoanhThu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDoanhThu.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvDoanhThu.ThemeStyle.HeaderStyle.Height = 4;
            dgvDoanhThu.ThemeStyle.ReadOnly = false;
            dgvDoanhThu.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvDoanhThu.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDoanhThu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvDoanhThu.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvDoanhThu.ThemeStyle.RowsStyle.Height = 29;
            dgvDoanhThu.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvDoanhThu.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // btnXuatExcel
            // 
            btnXuatExcel.BorderRadius = 10;
            btnXuatExcel.CustomizableEdges = customizableEdges11;
            btnXuatExcel.DisabledState.BorderColor = Color.DarkGray;
            btnXuatExcel.DisabledState.CustomBorderColor = Color.DarkGray;
            btnXuatExcel.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnXuatExcel.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnXuatExcel.FillColor = Color.Maroon;
            btnXuatExcel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXuatExcel.ForeColor = Color.White;
            btnXuatExcel.Location = new Point(1166, 144);
            btnXuatExcel.Name = "btnXuatExcel";
            btnXuatExcel.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnXuatExcel.Size = new Size(120, 38);
            btnXuatExcel.TabIndex = 13;
            btnXuatExcel.Text = "Xuất báo cáo";
            btnXuatExcel.Click += btnXuatExcel_Click;
            // 
            // frmAdmin_DoanhThu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Salmon;
            ClientSize = new Size(1291, 644);
            Controls.Add(btnXuatExcel);
            Controls.Add(label1);
            Controls.Add(dgvDoanhThu);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(pnlChartArea);
            Controls.Add(lblTongDoanhThu);
            Controls.Add(btnThongKe);
            Controls.Add(dtpDenNgay);
            Controls.Add(dtpTuNgay);
            Controls.Add(cbTieuChi);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmAdmin_DoanhThu";
            Text = "frmAdmin_DoanhThu";
            ((System.ComponentModel.ISupportInitialize)dgvDoanhThu).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2ComboBox cbTieuChi;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpTuNgay;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpDenNgay;
        private Guna.UI2.WinForms.Guna2Button btnThongKe;
        private Label lblTongDoanhThu;
        private Guna.UI2.WinForms.Guna2Panel pnlChartArea;
        private Label label1;
        private Label label4;
        private Label label5;
        private Label label2;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDoanhThu;
        private Guna.UI2.WinForms.Guna2Button btnXuatExcel;
    }
}