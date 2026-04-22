using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_DoanhThu : Form
    {
        // ── Dữ liệu biểu đồ ──
        private List<string> _chartLabels = new();
        private List<double> _chartValues = new();
        private string _chartTitle = "";

        public frmAdmin_DoanhThu()
        {
            InitializeComponent();
            // Gắn sự kiện vẽ biểu đồ vào pnlChartArea
            pnlChartArea.Paint += PnlChartArea_Paint;
        }

        // ════════════════════════════════════════
        //  LOAD
        // ════════════════════════════════════════
        private void frmAdmin_DoanhThu_Load(object sender, EventArgs e)
        {
            cbTieuChi.SelectedIndex = 0;
            lblTongDoanhThu.Text = "TỔNG DOANH THU: 0 VNĐ";
        }

        // ════════════════════════════════════════
        //  NÚT XEM BÁO CÁO
        // ════════════════════════════════════════
        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (cbTieuChi.SelectedIndex == 0)
                ThongKeTheoNgay();
            else
                ThongKeTheoPhim();
        }

        // ── Thống kê theo ngày ──
        private void ThongKeTheoNgay()
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date;

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _chartLabels.Clear();
            _chartValues.Clear();
            long tongDT = 0;

            // TODO: Thay bằng truy vấn database thực tế
            for (DateTime d = tuNgay; d <= denNgay; d = d.AddDays(1))
            {
                _chartLabels.Add(d.ToString("dd/MM"));
                double dt = new Random(d.Day * d.Month + d.Year).Next(1_000_000, 50_000_000);
                _chartValues.Add(dt);
                tongDT += (long)dt;
            }

            _chartTitle = "Doanh thu theo ngày (VNĐ)";
            HienThiDataGrid("Ngày");
            lblTongDoanhThu.Text = "TỔNG DOANH THU: " + tongDT.ToString("N0") + " VNĐ";
            pnlChartArea.Invalidate(); // Vẽ lại biểu đồ
        }

        // ── Thống kê theo phim ──
        private void ThongKeTheoPhim()
        {
            _chartLabels = new List<string> { "Avengers", "Spider-Man", "Interstellar", "Inception", "Dune" };
            _chartValues.Clear();
            long tongDT = 0;

            // TODO: Thay bằng truy vấn database thực tế
            var rnd = new Random(42);
            foreach (var _ in _chartLabels)
            {
                double dt = rnd.Next(5_000_000, 200_000_000);
                _chartValues.Add(dt);
                tongDT += (long)dt;
            }

            _chartTitle = "Doanh thu theo phim (VNĐ)";
            HienThiDataGrid("Phim");
            lblTongDoanhThu.Text = "TỔNG DOANH THU: " + tongDT.ToString("N0") + " VNĐ";
            pnlChartArea.Invalidate();
        }

        // ════════════════════════════════════════
        //  VẼ BIỂU ĐỒ CỘT bằng GDI+
        // ════════════════════════════════════════
        private void PnlChartArea_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = pnlChartArea.Width;
            int h = pnlChartArea.Height;

            // Nền trắng
            g.Clear(Color.White);

            if (_chartValues.Count == 0) return;

            // Lề
            int padLeft = 55, padRight = 15, padTop = 35, padBottom = 45;
            int chartW = w - padLeft - padRight;
            int chartH = h - padTop - padBottom;

            double maxVal = 0;
            foreach (var v in _chartValues) if (v > maxVal) maxVal = v;
            if (maxVal == 0) maxVal = 1;

            // Tiêu đề biểu đồ
            using var titleFont = new Font("Segoe UI", 8f, FontStyle.Bold);
            g.DrawString(_chartTitle, titleFont, Brushes.Gray, padLeft, 8);

            // Đường lưới ngang
            using var gridPen = new Pen(Color.FromArgb(220, 220, 220), 1f);
            int gridLines = 5;
            for (int i = 0; i <= gridLines; i++)
            {
                int y = padTop + chartH - (int)(chartH * i / (double)gridLines);
                g.DrawLine(gridPen, padLeft, y, padLeft + chartW, y);

                long gridVal = (long)(maxVal * i / gridLines);
                string label = gridVal >= 1_000_000
                    ? (gridVal / 1_000_000).ToString("N0") + "M"
                    : gridVal.ToString("N0");
                using var axisFont = new Font("Segoe UI", 7f);
                g.DrawString(label, axisFont, Brushes.Gray, 0, y - 8);
            }

            // Cột
            int n = _chartValues.Count;
            float barW = Math.Max(8, (float)chartW / n * 0.6f);
            float step = (float)chartW / n;

            Color[] palette = {
                Color.FromArgb(229,  9,  20),
                Color.FromArgb(100, 88, 255),
                Color.FromArgb(  0,180,120),
                Color.FromArgb(255,160,  0),
                Color.FromArgb( 30,144,255)
            };

            for (int i = 0; i < n; i++)
            {
                float barH = (float)(_chartValues[i] / maxVal * chartH);
                float x = padLeft + i * step + (step - barW) / 2f;
                float y = padTop + chartH - barH;

                Color col = palette[i % palette.Length];
                using var brush = new SolidBrush(col);
                g.FillRectangle(brush, x, y, barW, barH);

                // Label trục X
                using var lblFont = new Font("Segoe UI", 7f);
                SizeF ts = g.MeasureString(_chartLabels[i], lblFont);
                g.DrawString(_chartLabels[i], lblFont, Brushes.DimGray,
                    x + barW / 2f - ts.Width / 2f, padTop + chartH + 4);
            }

            // Trục X và Y
            using var axisPen = new Pen(Color.DimGray, 1.5f);
            g.DrawLine(axisPen, padLeft, padTop, padLeft, padTop + chartH);
            g.DrawLine(axisPen, padLeft, padTop + chartH, padLeft + chartW, padTop + chartH);
        }

        // ════════════════════════════════════════
        //  HIỂN THỊ DATA GRID
        // ════════════════════════════════════════
        private void HienThiDataGrid(string colHeader)
        {
            dgvDoanhThu.Rows.Clear();
            dgvDoanhThu.Columns.Clear();

            dgvDoanhThu.Columns.Add("col1", colHeader);
            dgvDoanhThu.Columns.Add("col2", "Doanh thu (VNĐ)");
            dgvDoanhThu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            for (int i = 0; i < _chartLabels.Count; i++)
                dgvDoanhThu.Rows.Add(_chartLabels[i], ((long)_chartValues[i]).ToString("N0"));
        }

        // ── Placeholder cho label click events (Designer tự tạo) ──
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }

        private void frmAdmin_DoanhThu_Load_1(object sender, EventArgs e)
        {

        }
    }
}