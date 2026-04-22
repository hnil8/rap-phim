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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            cbTieuChi = new Guna.UI2.WinForms.Guna2ComboBox();
            dtpTuNgay = new Guna.UI2.WinForms.Guna2DateTimePicker();
            dtpDenNgay = new Guna.UI2.WinForms.Guna2DateTimePicker();
            btnThongKe = new Guna.UI2.WinForms.Guna2Button();
            lblTongDoanhThu = new Label();
            pnlChartArea = new Guna.UI2.WinForms.Guna2Panel();
            label1 = new Label();
            dgvDoanhThu = new Guna.UI2.WinForms.Guna2DataGridView();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            pnlChartArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDoanhThu).BeginInit();
            SuspendLayout();
            // 
            // cbTieuChi
            // 
            cbTieuChi.BackColor = Color.Transparent;
            cbTieuChi.BorderRadius = 10;
            cbTieuChi.CustomizableEdges = customizableEdges11;
            cbTieuChi.DrawMode = DrawMode.OwnerDrawFixed;
            cbTieuChi.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTieuChi.FillColor = Color.Linen;
            cbTieuChi.FocusedColor = Color.FromArgb(94, 148, 255);
            cbTieuChi.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cbTieuChi.Font = new Font("Segoe UI", 10F);
            cbTieuChi.ForeColor = Color.FromArgb(68, 88, 112);
            cbTieuChi.ItemHeight = 30;
            cbTieuChi.Items.AddRange(new object[] { "Thống kê theo ngày", "Thống kê theo phim" });
            cbTieuChi.Location = new Point(106, 120);
            cbTieuChi.Name = "cbTieuChi";
            cbTieuChi.ShadowDecoration.CustomizableEdges = customizableEdges12;
            cbTieuChi.Size = new Size(200, 36);
            cbTieuChi.StartIndex = 0;
            cbTieuChi.TabIndex = 0;
            // 
            // dtpTuNgay
            // 
            dtpTuNgay.BorderColor = SystemColors.ControlLightLight;
            dtpTuNgay.BorderRadius = 10;
            dtpTuNgay.Checked = true;
            dtpTuNgay.CustomizableEdges = customizableEdges13;
            dtpTuNgay.FillColor = Color.Linen;
            dtpTuNgay.Font = new Font("Segoe UI", 9F);
            dtpTuNgay.Format = DateTimePickerFormat.Long;
            dtpTuNgay.Location = new Point(106, 162);
            dtpTuNgay.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpTuNgay.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.ShadowDecoration.CustomizableEdges = customizableEdges14;
            dtpTuNgay.Size = new Size(200, 36);
            dtpTuNgay.TabIndex = 1;
            dtpTuNgay.Value = new DateTime(2026, 4, 12, 16, 55, 20, 2);
            // 
            // dtpDenNgay
            // 
            dtpDenNgay.BorderColor = Color.White;
            dtpDenNgay.BorderRadius = 10;
            dtpDenNgay.Checked = true;
            dtpDenNgay.CustomizableEdges = customizableEdges15;
            dtpDenNgay.FillColor = Color.Linen;
            dtpDenNgay.Font = new Font("Segoe UI", 9F);
            dtpDenNgay.Format = DateTimePickerFormat.Long;
            dtpDenNgay.Location = new Point(106, 204);
            dtpDenNgay.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
            dtpDenNgay.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.ShadowDecoration.CustomizableEdges = customizableEdges16;
            dtpDenNgay.Size = new Size(200, 37);
            dtpDenNgay.TabIndex = 2;
            dtpDenNgay.Value = new DateTime(2026, 4, 12, 16, 55, 32, 366);
            // 
            // btnThongKe
            // 
            btnThongKe.BorderRadius = 10;
            btnThongKe.CustomizableEdges = customizableEdges17;
            btnThongKe.DisabledState.BorderColor = Color.DarkGray;
            btnThongKe.DisabledState.CustomBorderColor = Color.DarkGray;
            btnThongKe.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnThongKe.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnThongKe.FillColor = Color.DarkViolet;
            btnThongKe.Font = new Font("Segoe UI", 9F);
            btnThongKe.ForeColor = Color.White;
            btnThongKe.Location = new Point(324, 162);
            btnThongKe.Name = "btnThongKe";
            btnThongKe.ShadowDecoration.CustomizableEdges = customizableEdges18;
            btnThongKe.Size = new Size(129, 38);
            btnThongKe.TabIndex = 3;
            btnThongKe.Text = "XEM BÁO CÁO";
            btnThongKe.Click += btnThongKe_Click;
            // 
            // lblTongDoanhThu
            // 
            lblTongDoanhThu.AutoSize = true;
            lblTongDoanhThu.Font = new Font("Segoe UI Semibold", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTongDoanhThu.ForeColor = SystemColors.ControlLightLight;
            lblTongDoanhThu.Location = new Point(540, 132);
            lblTongDoanhThu.Name = "lblTongDoanhThu";
            lblTongDoanhThu.Size = new Size(366, 38);
            lblTongDoanhThu.TabIndex = 4;
            lblTongDoanhThu.Text = "TỔNG DOANH THU: 0 VNĐ";
            // 
            // pnlChartArea
            // 
            pnlChartArea.Controls.Add(label1);
            pnlChartArea.CustomizableEdges = customizableEdges19;
            pnlChartArea.Location = new Point(10, 247);
            pnlChartArea.Name = "pnlChartArea";
            pnlChartArea.ShadowDecoration.CustomizableEdges = customizableEdges20;
            pnlChartArea.Size = new Size(458, 279);
            pnlChartArea.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(168, 17);
            label1.TabIndex = 7;
            label1.Text = "Biểu đồ Doanh thu (VND)";
            // 
            // dgvDoanhThu
            // 
            dataGridViewCellStyle4.BackColor = Color.White;
            dgvDoanhThu.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvDoanhThu.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvDoanhThu.ColumnHeadersHeight = 4;
            dgvDoanhThu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvDoanhThu.DefaultCellStyle = dataGridViewCellStyle6;
            dgvDoanhThu.GridColor = Color.FromArgb(231, 229, 255);
            dgvDoanhThu.Location = new Point(540, 247);
            dgvDoanhThu.Name = "dgvDoanhThu";
            dgvDoanhThu.RowHeadersVisible = false;
            dgvDoanhThu.RowHeadersWidth = 51;
            dgvDoanhThu.Size = new Size(389, 279);
            dgvDoanhThu.TabIndex = 6;
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
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ControlLightLight;
            label2.Location = new Point(7, 120);
            label2.Name = "label2";
            label2.Size = new Size(98, 28);
            label2.TabIndex = 8;
            label2.Text = "Tiêu chí :";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Showcard Gothic", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = SystemColors.ControlLightLight;
            label3.Location = new Point(376, 32);
            label3.Name = "label3";
            label3.Size = new Size(188, 35);
            label3.TabIndex = 9;
            label3.Text = "DOANH THU";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = SystemColors.ControlLightLight;
            label4.Location = new Point(23, 162);
            label4.Name = "label4";
            label4.Size = new Size(69, 20);
            label4.TabIndex = 10;
            label4.Text = "Từ ngày :";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = SystemColors.ControlLightLight;
            label5.Location = new Point(23, 204);
            label5.Name = "label5";
            label5.Size = new Size(79, 20);
            label5.TabIndex = 11;
            label5.Text = "Đến ngày :";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(941, 560);
            tableLayoutPanel1.TabIndex = 12;
            // 
            // frmAdmin_DoanhThu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Salmon;
            ClientSize = new Size(941, 560);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(dgvDoanhThu);
            Controls.Add(pnlChartArea);
            Controls.Add(lblTongDoanhThu);
            Controls.Add(btnThongKe);
            Controls.Add(dtpDenNgay);
            Controls.Add(dtpTuNgay);
            Controls.Add(cbTieuChi);
            Controls.Add(tableLayoutPanel1);
            Name = "frmAdmin_DoanhThu";
            Text = "frmAdmin_DoanhThu";
            Load += frmAdmin_DoanhThu_Load_1;
            pnlChartArea.ResumeLayout(false);
            pnlChartArea.PerformLayout();
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
        private Guna.UI2.WinForms.Guna2DataGridView dgvDoanhThu;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TableLayoutPanel tableLayoutPanel1;
    }
}