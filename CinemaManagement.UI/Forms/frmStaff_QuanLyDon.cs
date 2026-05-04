using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmStaff_QuanLyDon : Form
    {
        // Hai bảng dữ liệu giả lập cho mô hình Master - Detail
        private DataTable dtHoaDon;
        private DataTable dtChiTiet;

        private string _selectedMaHD = "";

        public frmStaff_QuanLyDon()
        {
            InitializeComponent();

            this.Load += FrmStaff_QuanLyDon_Load;

            // Bắt sự kiện CellClick trên lưới Hóa Đơn (Bấm vào dòng nào bên trái, hiển thị chi tiết bên phải)
            this.dgvHoaDon.CellClick += DgvHoaDon_CellClick;
        }

        private void FrmStaff_QuanLyDon_Load(object sender, EventArgs e)
        {
            // Set ngày mặc định (Lọc trong tháng hiện tại)
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDenNgay.Value = DateTime.Now;

            SetupDataTables();
            LoadFakeData();
            ConfigureGrids();

            // Khóa nút in khi chưa chọn hóa đơn
            btnInVe.Enabled = false;
        }

        // ════════════════════════════════════════════════════════════
        // 1. CẤU HÌNH DỮ LIỆU & LƯỚI
        // ════════════════════════════════════════════════════════════
        private void SetupDataTables()
        {
            // Bảng Hóa Đơn Tổng
            dtHoaDon = new DataTable();
            dtHoaDon.Columns.Add("Mã HD", typeof(string));
            dtHoaDon.Columns.Add("Thời Gian", typeof(DateTime));
            dtHoaDon.Columns.Add("Khách Hàng", typeof(string));
            dtHoaDon.Columns.Add("SĐT", typeof(string));
            dtHoaDon.Columns.Add("Tổng Tiền", typeof(decimal));
            dtHoaDon.Columns.Add("Trạng Thái", typeof(string)); // Đã thanh toán / Đã hoàn tiền

            // Bảng Chi Tiết Món
            dtChiTiet = new DataTable();
            dtChiTiet.Columns.Add("Mã HD", typeof(string));
            dtChiTiet.Columns.Add("Tên Mục", typeof(string));
            dtChiTiet.Columns.Add("SL", typeof(int));
            dtChiTiet.Columns.Add("Đơn Giá", typeof(decimal));
            dtChiTiet.Columns.Add("Thành Tiền", typeof(decimal), "SL * [Đơn Giá]"); // Tự động nhân tiền
        }

        private void LoadFakeData()
        {
            dtHoaDon.Rows.Add("INV0812", new DateTime(2026, 3, 5, 14, 30, 0), "John Doe", "0911223344", 450000, "Đã thu tiền");
            dtHoaDon.Rows.Add("INV0811", new DateTime(2026, 3, 5, 10, 15, 0), "Jane Smith", "0988776655", 285000, "Đã thu tiền");
            dtHoaDon.Rows.Add("INV0810", new DateTime(2026, 2, 5, 20, 00, 0), "Bob Johnson", "0900110011", 620000, "Đã hoàn tiền");

            // Chi tiết hóa đơn INV0812
            dtChiTiet.Rows.Add("INV0812", "Vé: The Batman - 2D", 2, 150000);
            dtChiTiet.Rows.Add("INV0812", "Bắp rang bơ (Lớn)", 1, 80000);
            dtChiTiet.Rows.Add("INV0812", "Nước ngọt (Vừa)", 1, 70000);

            // Chi tiết hóa đơn INV0811
            dtChiTiet.Rows.Add("INV0811", "Vé: Spirited Away - 2D", 1, 120000);
            dtChiTiet.Rows.Add("INV0811", "Combo Couple", 1, 165000);

            dgvHoaDon.DataSource = dtHoaDon;
        }

        private void ConfigureGrids()
        {
            foreach (var grid in new[] { dgvHoaDon, dgvChiTiet })
            {
                grid.ReadOnly = true;
                grid.AllowUserToAddRows = false;
                grid.RowHeadersVisible = false;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            dgvHoaDon.Columns["Thời Gian"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgvHoaDon.Columns["Tổng Tiền"].DefaultCellStyle.Format = "N0";

            // Mới mở form chưa có dữ liệu chi tiết
            dgvChiTiet.DataSource = null;
        }

        // ════════════════════════════════════════════════════════════
        // 2. TƯƠNG TÁC LƯỚI - XEM CHI TIẾT
        // ════════════════════════════════════════════════════════════
        private void DgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvHoaDon.Rows[e.RowIndex];
                _selectedMaHD = row.Cells["Mã HD"].Value.ToString();

                lblMaHD.Text = "Mã hóa đơn: " + _selectedMaHD;
                lblThoiGian.Text = "Thời gian: " + Convert.ToDateTime(row.Cells["Thời Gian"].Value).ToString("dd/MM/yyyy HH:mm");

                string tenKhach = row.Cells["Khách Hàng"].Value.ToString();
                string sdt = row.Cells["SĐT"].Value.ToString();
                lblKhachHang.Text = string.IsNullOrEmpty(sdt) ? $"Khách hàng: {tenKhach}" : $"Khách hàng: {tenKhach} - {sdt}";

                lblTongTien.Text = "Tổng tiền: " + Convert.ToDecimal(row.Cells["Tổng Tiền"].Value).ToString("N0") + " VNĐ";

                // Lọc lưới chi tiết
                DataView dv = new DataView(dtChiTiet);
                dv.RowFilter = $"[Mã HD] = '{_selectedMaHD}'";
                dgvChiTiet.DataSource = dv;

                dgvChiTiet.Columns["Mã HD"].Visible = false;
                dgvChiTiet.Columns["Đơn Giá"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["Thành Tiền"].DefaultCellStyle.Format = "N0";

                btnInVe.Enabled = true;
            }
        }

        // ════════════════════════════════════════════════════════════
        // 3. TÌM KIẾM & LÀM MỚI
        // ════════════════════════════════════════════════════════════
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            ThucHienTimKiem();
        }

        // Tự động lọc mỗi khi khách gõ vào ô tìm kiếm
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            ThucHienTimKiem();
        }

        // Lọc lại dữ liệu mỗi khi khách đổi ngày
        private void dtpTuNgay_ValueChanged(object sender, EventArgs e) { ThucHienTimKiem(); }
        private void dtpDenNgay_ValueChanged(object sender, EventArgs e) { ThucHienTimKiem(); }

        private void ThucHienTimKiem()
        {
            if (dtHoaDon == null) return;

            string keyword = txtTimKiem.Text.Trim();
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

            // Cú pháp lọc của DataTable
            string filter = $"[Thời Gian] >= '{tuNgay:yyyy-MM-dd HH:mm:ss}' AND [Thời Gian] <= '{denNgay:yyyy-MM-dd HH:mm:ss}'";

            if (!string.IsNullOrEmpty(keyword))
            {
                filter += $" AND ([Mã HD] LIKE '%{keyword}%' OR [SĐT] LIKE '%{keyword}%' OR [Khách Hàng] LIKE '%{keyword}%')";
            }

            dtHoaDon.DefaultView.RowFilter = filter;
            ResetChiTiet();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDenNgay.Value = DateTime.Now;

            dtHoaDon.DefaultView.RowFilter = "";
            ResetChiTiet();
        }

        private void ResetChiTiet()
        {
            _selectedMaHD = "";
            lblMaHD.Text = "Mã hóa đơn: ";
            lblKhachHang.Text = "Khách hàng: ";
            lblThoiGian.Text = "Thời gian: ";
            lblTongTien.Text = "Tổng tiền: 0 VNĐ";
            dgvChiTiet.DataSource = null;
            btnInVe.Enabled = false;
        }

        // ════════════════════════════════════════════════════════════
        // 4. IN VÉ VÀ HÀM RỖNG CHỐNG LỖI DESIGNER
        // ════════════════════════════════════════════════════════════
        private void btnInVe_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedMaHD)) return;
            MessageBox.Show($"Đang kết nối máy in...\nĐã in lại vé và hóa đơn cho giao dịch {_selectedMaHD}.", "In lại vé", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dgvChiTiet_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}