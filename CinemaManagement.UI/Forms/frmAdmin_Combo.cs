using CinemaManagement.BLL.DTOs;
using CinemaManagement.BLL.Services;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_Combo : Form
    {
        // ── Tiêm Service (Dependency Injection) ──
        private readonly IComboService _comboService;
        private readonly ISanPhamService _sanPhamService;

        // ── Biến State ──
        private int _currentComboId = -1;
        private string _selectedImagePath = "";

        // [TẤM KHIÊN 1]: Chống Form Load bị gọi 2 lần do Designer
        private bool _isFirstTimeLoad = true;

        // [TẤM KHIÊN 2]: Chống sự kiện TextChanged nhảy lung tung khi đang Load
        private bool _isLoading = false;

        // Danh sách lưu tạm các món ăn được chọn vào Combo (Giỏ hàng)
        private BindingList<ChiTietComboDto> _gioHang;

        public frmAdmin_Combo(IComboService comboService, ISanPhamService sanPhamService)
        {
            InitializeComponent();
            _comboService = comboService;
            _sanPhamService = sanPhamService;

            _gioHang = new BindingList<ChiTietComboDto>();

            // Gắn sự kiện (Nếu Designer đã gắn rồi, Tấm khiên 1 sẽ lo việc chặn trùng lặp)
            this.Load += frmAdmin_Combo_Load;
            tsmiSua.Click += TsmiSua_Click;
            tsmiXoa.Click += TsmiXoa_Click;
        }

        private async void frmAdmin_Combo_Load(object sender, EventArgs e)
        {
            // Bức tường thép: Nếu đã chạy Load rồi thì luồng thứ 2 sẽ bị đuổi về ngay lập tức!
            if (!_isFirstTimeLoad) return;
            _isFirstTimeLoad = false;

            _isLoading = true; // Khóa các sự kiện UI

            if (this.Controls.ContainsKey("dgvChiTietCombo"))
            {
                var dgvChiTiet = this.Controls["dgvChiTietCombo"] as DataGridView;
                if (dgvChiTiet != null) dgvChiTiet.DataSource = _gioHang;
            }

            // Giờ đây 2 lệnh này được an toàn chạy tuần tự
            await LoadDanhSachSanPhamAsync();
            await LoadDanhSachComboAsync();
            ResetInputs();

            _isLoading = false; // Mở khóa UI
        }

        #region Các hàm xử lý Dữ liệu (Gọi xuống BLL)

        private async Task LoadDanhSachSanPhamAsync()
        {
            try
            {
                var sanPhams = await _sanPhamService.GetAllAsync();
                dgvSanPham.DataSource = sanPhams;

                if (dgvSanPham.Columns.Contains("SanPhamId"))
                {
                    dgvSanPham.Columns["SanPhamId"].HeaderText = "Mã SP";
                    dgvSanPham.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
                    dgvSanPham.Columns["GiaBan"].HeaderText = "Giá Bán";
                    dgvSanPham.Columns["TonKho"].HeaderText = "Tồn Kho";

                    if (dgvSanPham.Columns.Contains("NhomId")) dgvSanPham.Columns["NhomId"].Visible = false;
                    if (dgvSanPham.Columns.Contains("TenNhom")) dgvSanPham.Columns["TenNhom"].Visible = false;
                    if (dgvSanPham.Columns.Contains("MoTa")) dgvSanPham.Columns["MoTa"].Visible = false;
                    if (dgvSanPham.Columns.Contains("HinhAnhUrl")) dgvSanPham.Columns["HinhAnhUrl"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách Sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDanhSachComboAsync(string keyword = "")
        {
            try
            {
                var combos = await _comboService.GetAllAsync(keyword);
                dgvCOMBO.DataSource = combos;

                if (dgvCOMBO.Columns.Contains("ComboId"))
                {
                    dgvCOMBO.Columns["ComboId"].HeaderText = "Mã Combo";
                    dgvCOMBO.Columns["TenCombo"].HeaderText = "Tên Combo";
                    dgvCOMBO.Columns["GiaCombo"].HeaderText = "Giá Bán";
                    dgvCOMBO.Columns["MoTa"].HeaderText = "Mô Tả";

                    if (dgvCOMBO.Columns.Contains("HinhAnhUrl")) dgvCOMBO.Columns["HinhAnhUrl"].Visible = false;
                    if (dgvCOMBO.Columns.Contains("IsActive")) dgvCOMBO.Columns["IsActive"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách Combo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Xử lý Hình ảnh (Dùng URL thay vì Byte Array)

        private void btnChonAnh_Click(object sender, EventArgs e) => ChonHinhAnh();
        private void picHinhAnh_Click(object sender, EventArgs e) => ChonHinhAnh();

        private void ChonHinhAnh()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn hình ảnh Combo";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedImagePath = ofd.FileName;
                    picHinhAnh.ImageLocation = _selectedImagePath;
                    picHinhAnh.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        #endregion

        #region CRUD: Thêm, Sửa, Xóa, Làm Mới

        private async void btnLamMoi_Click(object sender, EventArgs e)
        {
            _isLoading = true; // Chặn sự kiện TextChanged

            ResetInputs();
            txtTimKiem.Clear();

            _isLoading = false;

            await LoadDanhSachComboAsync();
        }

        private void ResetInputs()
        {
            _currentComboId = -1;
            _selectedImagePath = "";
            txtTenCombo.Clear();
            txtGiaBan.Clear();
            txtMota.Clear();
            picHinhAnh.Image = null;
            _gioHang.Clear();

            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;
            btnThem.Enabled = false;

            try
            {
                var dto = new CreateComboDto
                {
                    TenCombo = txtTenCombo.Text.Trim(),
                    GiaCombo = Convert.ToDecimal(txtGiaBan.Text.Trim()),
                    MoTa = txtMota.Text.Trim(),
                    HinhAnhUrl = _selectedImagePath,
                    IsActive = true,
                    ChiTietCombos = _gioHang.ToList()
                };

                var result = await _comboService.CreateAsync(dto);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetInputs();
                    await LoadDanhSachComboAsync();
                }
                else MessageBox.Show(result.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally { btnThem.Enabled = true; }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            if (_currentComboId <= 0) return;
            if (!ValidateInputs()) return;
            btnSua.Enabled = false;

            try
            {
                var dto = new CreateComboDto
                {
                    TenCombo = txtTenCombo.Text.Trim(),
                    GiaCombo = Convert.ToDecimal(txtGiaBan.Text.Trim()),
                    MoTa = txtMota.Text.Trim(),
                    HinhAnhUrl = _selectedImagePath,
                    IsActive = true,
                    ChiTietCombos = _gioHang.ToList()
                };

                var result = await _comboService.UpdateAsync(_currentComboId, dto);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetInputs();
                    await LoadDanhSachComboAsync();
                }
                else MessageBox.Show(result.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally { btnSua.Enabled = true; }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (_currentComboId <= 0)
            {
                MessageBox.Show("Vui lòng chọn một Combo để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa Combo này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var result = await _comboService.DeleteAsync(_currentComboId);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetInputs();
                    await LoadDanhSachComboAsync();
                }
                else MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTenCombo.Text))
            {
                MessageBox.Show("Vui lòng nhập tên Combo!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenCombo.Focus();
                return false;
            }
            if (!decimal.TryParse(txtGiaBan.Text, out _))
            {
                MessageBox.Show("Giá bán không hợp lệ. Vui lòng nhập số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaBan.Focus();
                return false;
            }
            if (_gioHang.Count == 0)
            {
                MessageBox.Show("Combo phải có ít nhất 1 sản phẩm bên trong!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        #endregion

        #region Xử lý GridView và Tìm kiếm

        private async void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCOMBO.Rows[e.RowIndex];
                _currentComboId = Convert.ToInt32(row.Cells["ComboId"].Value);

                var comboDetail = await _comboService.GetByIdAsync(_currentComboId);
                if (comboDetail != null)
                {
                    txtTenCombo.Text = comboDetail.TenCombo;
                    txtGiaBan.Text = comboDetail.GiaCombo.ToString("0");
                    txtMota.Text = comboDetail.MoTa;

                    _selectedImagePath = comboDetail.HinhAnhUrl;
                    if (!string.IsNullOrEmpty(_selectedImagePath) && File.Exists(_selectedImagePath))
                    {
                        picHinhAnh.ImageLocation = _selectedImagePath;
                        picHinhAnh.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else picHinhAnh.Image = null;

                    _gioHang.Clear();
                    foreach (var item in comboDetail.ChiTietCombos)
                    {
                        _gioHang.Add(item);
                    }

                    btnThem.Enabled = false;
                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                }
            }
        }

        private void dgvSanPham_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];
                int spId = Convert.ToInt32(row.Cells["SanPhamId"].Value);
                string tenSp = row.Cells["TenSanPham"].Value.ToString();

                var existingItem = _gioHang.FirstOrDefault(x => x.SanPhamId == spId);
                if (existingItem != null)
                {
                    existingItem.SoLuong++;
                    _gioHang.ResetBindings();
                }
                else
                {
                    _gioHang.Add(new ChiTietComboDto
                    {
                        SanPhamId = spId,
                        TenSanPham = tenSp,
                        SoLuong = 1
                    });
                }
            }
        }

        private void dgvSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadDanhSachComboAsync(txtTimKiem.Text.Trim());
        }

        private async void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            await LoadDanhSachComboAsync(txtTimKiem.Text.Trim());
        }

        #endregion

        #region Context Menu Strip (Chuột phải vào dgvCOMBO)

        private void TsmiSua_Click(object sender, EventArgs e)
        {
            if (_currentComboId > 0) txtTenCombo.Focus();
        }

        private void TsmiXoa_Click(object sender, EventArgs e)
        {
            btnXoa_Click(sender, e);
        }

        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}