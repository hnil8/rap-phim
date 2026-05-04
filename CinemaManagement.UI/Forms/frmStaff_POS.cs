using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmStaff_POS : Form
    {
        // Khai báo các DataTable để lưu trữ dữ liệu tạm thời
        private DataTable dtSuatChieu;
        private DataTable dtCombo;
        private DataTable dtGioHang;

        // Biến tính toán tiền tệ
        private decimal _tienVe = 0;
        private decimal _tienCombo = 0;
        private decimal _tienGiamGia = 0;

        public frmStaff_POS()
        {
            InitializeComponent();

            // Đăng ký sự kiện Load form
            this.Load += FrmStaff_POS_Load;

            // Đăng ký sự kiện CellClick để bấm vào đâu trong ô cũng nhận (thay vì chỉ bấm vào chữ)
            this.dgvSuatChieu.CellClick += DgvSuatChieu_CellClick;
            this.dgvCombo.CellClick += DgvCombo_CellClick;
            this.dgvGioHang.CellClick += DgvGioHang_CellClick;
        }

        private void FrmStaff_POS_Load(object sender, EventArgs e)
        {
            SetupDataTables();
            LoadFakeData();
            ConfigureGrids();
            CapNhatTongTien();
        }

        // ════════════════════════════════════════════════════════════
        // 1. CẤU HÌNH BẢNG DỮ LIỆU & LƯỚI GIAO DIỆN
        // ════════════════════════════════════════════════════════════
        private void SetupDataTables()
        {
            // Bảng Suất Chiếu
            dtSuatChieu = new DataTable();
            dtSuatChieu.Columns.Add("Mã SC", typeof(string));
            dtSuatChieu.Columns.Add("Phim", typeof(string));
            dtSuatChieu.Columns.Add("Phòng", typeof(string));
            dtSuatChieu.Columns.Add("Giờ Chiếu", typeof(string));
            dtSuatChieu.Columns.Add("Giá Vé", typeof(decimal));

            // Bảng Bắp Nước (Combo)
            dtCombo = new DataTable();
            dtCombo.Columns.Add("Mã", typeof(string));
            dtCombo.Columns.Add("Tên Combo", typeof(string));
            dtCombo.Columns.Add("Giá", typeof(decimal));

            // Bảng Giỏ Hàng
            dtGioHang = new DataTable();
            dtGioHang.Columns.Add("Mã", typeof(string));
            dtGioHang.Columns.Add("Tên Món", typeof(string));
            dtGioHang.Columns.Add("SL", typeof(int));
            dtGioHang.Columns.Add("Thành Tiền", typeof(decimal));
        }

        private void LoadFakeData()
        {
            // Dữ liệu mẫu Suất Chiếu
            dtSuatChieu.Rows.Add("SC01", "Lật Mặt 7: Một Điều Ước", "Phòng 1", "18:30", 85000);
            dtSuatChieu.Rows.Add("SC02", "Mai", "Phòng 2", "19:00", 90000);
            dtSuatChieu.Rows.Add("SC03", "Godzilla x Kong", "Phòng 3", "20:15", 95000);

            // Dữ liệu mẫu Bắp Nước
            dtCombo.Rows.Add("CB01", "Combo Solo (1 Bắp + 1 Nước)", 75000);
            dtCombo.Rows.Add("CB02", "Combo Couple (1 Bắp + 2 Nước)", 105000);
            dtCombo.Rows.Add("CB03", "Bắp Phô Mai (Lớn)", 55000);

            dgvSuatChieu.DataSource = dtSuatChieu;
            dgvCombo.DataSource = dtCombo;
            dgvGioHang.DataSource = dtGioHang;
        }

        private void ConfigureGrids()
        {
            // Căn chỉnh DataGridView cho đẹp
            foreach (var grid in new[] { dgvSuatChieu, dgvCombo, dgvGioHang })
            {
                grid.ReadOnly = true;
                grid.AllowUserToAddRows = false;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.RowHeadersVisible = false;
            }

            // Ẩn cột Mã
            dgvSuatChieu.Columns["Mã SC"].Visible = false;
            dgvCombo.Columns["Mã"].Visible = false;
            dgvGioHang.Columns["Mã"].Visible = false;

            // Định dạng tiền tệ có dấu chấm ngàn (VD: 85.000)
            dgvCombo.Columns["Giá"].DefaultCellStyle.Format = "N0";
            dgvSuatChieu.Columns["Giá Vé"].DefaultCellStyle.Format = "N0";
            dgvGioHang.Columns["Thành Tiền"].DefaultCellStyle.Format = "N0";

            // Chỉnh lại chiều rộng cột Giỏ Hàng
            dgvGioHang.Columns["Tên Món"].FillWeight = 50;
            dgvGioHang.Columns["SL"].FillWeight = 20;
            dgvGioHang.Columns["Thành Tiền"].FillWeight = 30;
        }

        // ════════════════════════════════════════════════════════════
        // 2. TƯƠNG TÁC: CHỌN PHIM & THÊM BẮP NƯỚC
        // ════════════════════════════════════════════════════════════

        // CHỌN PHIM
        private void DgvSuatChieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSuatChieu.Rows[e.RowIndex];
                lblTenPhim.Text = "Tên phim: " + row.Cells["Phim"].Value.ToString();
                lblSuatChieu.Text = "Suất chiếu: " + row.Cells["Giờ Chiếu"].Value.ToString();
                lblPhong.Text = "Phòng: " + row.Cells["Phòng"].Value.ToString();
                lblGhe.Text = "Ghế: G5, G6"; // Tạm fix cứng, sau này sẽ có form chọn sơ đồ ghế riêng

                // Tạm tính giá vé (Giả sử mua 2 vé G5, G6)
                _tienVe = Convert.ToDecimal(row.Cells["Giá Vé"].Value) * 2;

                CapNhatTongTien();
            }
        }

        // THÊM COMBO VÀO GIỎ HÀNG
        private void DgvCombo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCombo.Rows[e.RowIndex];
                string ma = row.Cells["Mã"].Value.ToString();
                string ten = row.Cells["Tên Combo"].Value.ToString();
                decimal gia = Convert.ToDecimal(row.Cells["Giá"].Value);

                bool isExist = false;
                foreach (DataRow r in dtGioHang.Rows)
                {
                    if (r["Mã"].ToString() == ma)
                    {
                        int sl = Convert.ToInt32(r["SL"]) + 1; // Tăng số lượng
                        r["SL"] = sl;
                        r["Thành Tiền"] = sl * gia;
                        isExist = true;
                        break;
                    }
                }

                if (!isExist) dtGioHang.Rows.Add(ma, ten, 1, gia); // Thêm mới nếu chưa có

                CapNhatTongTien();
            }
        }

        // BỎ COMBO KHỎI GIỎ HÀNG (Nhấp vào giỏ hàng để xóa)
        private void DgvGioHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dtGioHang.Rows.RemoveAt(e.RowIndex);
                CapNhatTongTien();
            }
        }

        // ════════════════════════════════════════════════════════════
        // 3. TÍNH TỔNG TIỀN, ÁP DỤNG VOUCHER & THANH TOÁN
        // ════════════════════════════════════════════════════════════
        private void CapNhatTongTien()
        {
            // Tính tổng tiền Bắp Nước
            _tienCombo = 0;
            foreach (DataRow r in dtGioHang.Rows)
            {
                _tienCombo += Convert.ToDecimal(r["Thành Tiền"]);
            }

            // Tính thành tiền cuối cùng
            decimal tongTruocGiam = _tienVe + _tienCombo;
            decimal thanhTien = tongTruocGiam - _tienGiamGia;
            if (thanhTien < 0) thanhTien = 0; // Tránh âm tiền

            // Hiển thị lên UI bằng Guna2HtmlLabel
            lblTienVe.Text = "Tổng tiền vé: " + _tienVe.ToString("N0") + " VNĐ";
            lblTienCombo.Text = "Tổng tiền bắp nước: " + _tienCombo.ToString("N0") + " VNĐ";
            lblGiamGia.Text = "Giảm giá: - " + _tienGiamGia.ToString("N0") + " VNĐ";
            lblThanhTien.Text = "Thành tiền: " + thanhTien.ToString("N0") + " VNĐ";
        }

        private void btnApDung_Click(object sender, EventArgs e)
        {
            string code = txtUuDai.Text.Trim().ToUpper();

            // Giả lập mã Voucher
            if (code == "KM20")
            {
                _tienGiamGia = (_tienVe + _tienCombo) * 20 / 100;
                MessageBox.Show("Áp dụng mã giảm giá 20% thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _tienGiamGia = 0;
                MessageBox.Show("Mã ưu đãi không hợp lệ hoặc đã hết hạn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            CapNhatTongTien();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (_tienVe == 0 && _tienCombo == 0)
            {
                MessageBox.Show("Vui lòng chọn suất chiếu hoặc thức ăn trước khi thanh toán!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal thanhTienCuoiCung = (_tienVe + _tienCombo) - _tienGiamGia;
            if (thanhTienCuoiCung < 0) thanhTienCuoiCung = 0;

            // GỌI CỬA SỔ THANH TOÁN (Momo, VNPay, Tiền mặt...)
            // Đảm bảo bạn đã tạo form `frmThanhToan` như hướng dẫn ở bước trước
            using (frmThanhToan frm = new frmThanhToan(thanhTienCuoiCung))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Chạy vào đây tức là khách đã thanh toán THÀNH CÔNG
                    MessageBox.Show("Giao dịch hoàn tất! Đang in hóa đơn và vé...", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset lại màn hình POS để đón khách tiếp theo
                    _tienVe = 0;
                    _tienCombo = 0;
                    _tienGiamGia = 0;
                    dtGioHang.Clear();

                    lblTenPhim.Text = "Tên phim: ";
                    lblSuatChieu.Text = "Suất chiếu: ";
                    lblPhong.Text = "Phòng: ";
                    lblGhe.Text = "Ghế: ";
                    txtUuDai.Clear();

                    CapNhatTongTien();
                }
                // Nếu khách bấm nút Hủy thì không làm gì cả, giữ nguyên màn hình POS
            }
        }

        // ════════════════════════════════════════════════════════════
        // 4. TÌM KIẾM THEO TÊN PHIM
        // ════════════════════════════════════════════════════════════
        private void txtPhim_TextChanged(object sender, EventArgs e)
        {
            ThucHienTimKiem();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            ThucHienTimKiem();
        }

        private void ThucHienTimKiem()
        {
            if (dtSuatChieu != null)
            {
                string filter = txtPhim.Text.Trim();
                dtSuatChieu.DefaultView.RowFilter = string.IsNullOrEmpty(filter) ? "" : $"[Phim] LIKE '%{filter}%'";
            }
        }

        // ════════════════════════════════════════════════════════════
        // 5. CÁC HÀM RỖNG ĐỂ TRÁNH LỖI BIÊN DỊCH TỪ DESIGNER
        // ════════════════════════════════════════════════════════════
        private void dgvSuatChieu_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dgvCombo_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dgvGioHang_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}