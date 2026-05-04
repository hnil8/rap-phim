using CinemaManagement.BLL.DTOs;
using CinemaManagement.BLL.Services;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_NhapKhoFnB : Form
    {
        // ── Tiêm Service (Dependency Injection) ──
        private readonly ISanPhamService _sanPhamService;
        private readonly IPhieuNhapService _phieuNhapService;

        // Dùng BindingList thay cho DataTable để tự động đồng bộ với GridView
        private BindingList<ChiTietPhieuNhapDto> _gioHangNhap;

        public frmAdmin_NhapKhoFnB(ISanPhamService sanPhamService, IPhieuNhapService phieuNhapService)
        {
            InitializeComponent();
            _sanPhamService = sanPhamService;
            _phieuNhapService = phieuNhapService;

            _gioHangNhap = new BindingList<ChiTietPhieuNhapDto>();

            // Gắn sự kiện tính lại Thành Tiền khi người dùng tự sửa số trên lưới
            dgvPhieuNhap.CellValueChanged += DgvPhieuNhap_CellValueChanged;

            // Cấu hình Responsive
            SetupResponsiveLayout();
        }

        private async void frmAdmin_NhapKhoFnB_Load(object sender, EventArgs e)
        {
            // 1. Thông tin mặc định (Lấy Tên người dùng từ Form Main đã lưu lúc đăng nhập)
            txtNguoiNhap.Text = frmMain.TenNguoiDung;
            if (string.IsNullOrEmpty(txtNguoiNhap.Text)) txtNguoiNhap.Text = "Admin";

            dtpNgayNhap.Value = DateTime.Now;

            // 2. Gắn Giỏ hàng nhập vào Lưới
            dgvPhieuNhap.DataSource = _gioHangNhap;
            CauHinhLuoiPhieuNhap();

            // 3. Tải danh sách sản phẩm thật từ Database
            await LoadComboBoxSanPhamAsync();
        }

        #region RESPONSIVE & Cấu hình dữ liệu

        private void SetupResponsiveLayout()
        {
            // Các Panel đã được Dock chuẩn trong Designer (Left, Fill, Top)
            // Ta chỉ cần cho các cột trong DataGridView tự động dàn đều lấp kín màn hình
            if (dgvPhieuNhap != null)
            {
                dgvPhieuNhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void CauHinhLuoiPhieuNhap()
        {
            if (dgvPhieuNhap.Columns.Contains("SanPhamId")) dgvPhieuNhap.Columns["SanPhamId"].HeaderText = "Mã SP";
            if (dgvPhieuNhap.Columns.Contains("TenSanPham")) dgvPhieuNhap.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
            if (dgvPhieuNhap.Columns.Contains("SoLuong")) dgvPhieuNhap.Columns["SoLuong"].HeaderText = "Số Lượng";
            if (dgvPhieuNhap.Columns.Contains("GiaNhap")) dgvPhieuNhap.Columns["GiaNhap"].HeaderText = "Giá Nhập";
            if (dgvPhieuNhap.Columns.Contains("ThanhTien")) dgvPhieuNhap.Columns["ThanhTien"].HeaderText = "Thành Tiền";

            // [TÍNH NĂNG ĐẶC BIỆT] Cho phép thủ kho sửa tay Số lượng và Giá nhập ngay trên Lưới
            dgvPhieuNhap.ReadOnly = false;
            foreach (DataGridViewColumn col in dgvPhieuNhap.Columns)
            {
                if (col.Name == "SoLuong" || col.Name == "GiaNhap")
                    col.ReadOnly = false; // Mở khóa cho phép sửa
                else
                    col.ReadOnly = true;  // Khóa các cột còn lại (Mã SP, Tên SP, Thành tiền)
            }
        }

        private async Task LoadComboBoxSanPhamAsync()
        {
            try
            {
                var listSanPham = await _sanPhamService.GetAllAsync();
                cboSanPham.DataSource = listSanPham;
                cboSanPham.DisplayMember = "TenSanPham"; // Tên hiển thị
                cboSanPham.ValueMember = "SanPhamId";    // Giá trị ngầm
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Xử lý logic Nhập kho

        private void btnThemVaoPhieu_Click(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên > 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            // txtGiaNhap tương ứng với TextBox bên giao diện
            if (!decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap) || giaNhap < 0)
            {
                MessageBox.Show("Giá nhập không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap.Focus();
                return;
            }

            int maSP = Convert.ToInt32(cboSanPham.SelectedValue);
            string tenSP = cboSanPham.Text;

            // Kiểm tra trùng lặp trong giỏ hàng
            var existingItem = _gioHangNhap.FirstOrDefault(x => x.SanPhamId == maSP);
            if (existingItem != null)
            {
                // Nếu món đã có trên lưới -> Cộng dồn số lượng và cập nhật giá mới
                existingItem.SoLuong += soLuong;
                existingItem.GiaNhap = giaNhap;
                _gioHangNhap.ResetBindings(); // Yêu cầu lưới vẽ lại để tính Thành Tiền tự động
            }
            else
            {
                // Thêm dòng mới vào lưới
                _gioHangNhap.Add(new ChiTietPhieuNhapDto
                {
                    SanPhamId = maSP,
                    TenSanPham = tenSP,
                    SoLuong = soLuong,
                    GiaNhap = giaNhap
                });
            }

            // Xóa rỗng ô nhập để thủ kho bắn mã vạch/chọn món tiếp theo
            txtSoLuong.Clear();
            txtGiaNhap.Clear();
            cboSanPham.Focus();
        }

        // Sự kiện tự động tính lại Thành Tiền khi người dùng gõ sửa Số lượng/Giá trực tiếp trên Lưới
        private void DgvPhieuNhap_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _gioHangNhap.ResetBindings();
            }
        }

        private void btnHuyPhieu_Click(object sender, EventArgs e)
        {
            if (_gioHangNhap.Count > 0)
            {
                if (MessageBox.Show("Hủy phiếu nhập đang lập?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ClearPhieuNhap();
                }
            }
        }

        private async void btnLuuPhieuNhap_Click(object sender, EventArgs e)
        {
            if (_gioHangNhap.Count == 0)
            {
                MessageBox.Show("Phiếu nhập đang trống! Hãy thêm sản phẩm.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Khóa nút để tránh click đúp (Tạo 2 phiếu giống nhau)
            btnLuuPhieuNhap.Enabled = false;
            try
            {
                var phieuMoi = new CreatePhieuNhapDto
                {
                    NguoiNhap = txtNguoiNhap.Text,
                    NgayNhap = dtpNgayNhap.Value,
                    GhiChu = txtGhiChu.Text.Trim(),
                    ChiTietPhieu = _gioHangNhap.ToList() // Gói hàng gửi xuống BLL
                };

                // Đẩy xuống tầng BLL xử lý Transaction (Insert Phiếu + Cộng Tồn Kho)
                var result = await _phieuNhapService.CreatePhieuNhapAsync(phieuMoi);

                if (result.IsSuccess)
                {
                    MessageBox.Show("Chốt kho thành công! Số lượng đã được cộng vào kho thực tế.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearPhieuNhap();
                }
                else MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLuuPhieuNhap.Enabled = true;
            }
        }

        private void ClearPhieuNhap()
        {
            _gioHangNhap.Clear();
            txtGhiChu.Clear();
            txtSoLuong.Clear();
            txtGiaNhap.Clear();
            dtpNgayNhap.Value = DateTime.Now;
        }

        #endregion

        #region Các sự kiện rỗng sinh ra từ Designer (Giữ lại để không báo lỗi)
        private void txtNguoiNhap_TextChanged(object sender, EventArgs e) { }
        private void dtpNgayNhap_ValueChanged(object sender, EventArgs e) { }
        private void txtGhiChu_TextChanged(object sender, EventArgs e) { }
        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtSoLuong_TextChanged(object sender, EventArgs e) { }
        private void guna2TextBox4_TextChanged(object sender, EventArgs e) { } // txtGiaNhap
        private void dgvPhieuNhap_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        #endregion
    }
}