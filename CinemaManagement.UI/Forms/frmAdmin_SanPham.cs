using CinemaManagement.BLL.DTOs;
using CinemaManagement.BLL.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_SanPham : Form
    {
        // ── KIẾN TRÚC N-TIER: Tiêm Service qua Constructor ──
        private readonly ISanPhamService _sanPhamService;
        private readonly INhomFnBService _nhomFnBService;

        private int _selectedId = -1;
        private string _selectedImagePath = "";

        // Constructor nhận Dependency Injection
        public frmAdmin_SanPham(ISanPhamService sanPhamService, INhomFnBService nhomFnBService)
        {
            InitializeComponent();
            _sanPhamService = sanPhamService;
            _nhomFnBService = nhomFnBService;

            // Chú ý: Các sự kiện khác bạn đã gán trong Designer,
            // mình chỉ giữ lại những sự kiện bạn gán bằng tay ở đây.
            dvgSanPham.CellMouseDown += dvgSanPham_CellMouseDown;
            sửaSảnPhẩmToolStripMenuItem.Click += sửaSảnPhẩmToolStripMenuItem_Click;
            sửaXóaToolStripMenuItem.Click += sửaXóaToolStripMenuItem_Click;
        }

        // Sự kiện Form Load đổi thành async
        private async void frmAdmin_SanPham_Load(object sender, EventArgs e)
        {
            await LoadComboBoxDataAsync();
            await LoadDataGridAsync();
        }

        #region Khởi tạo & Load Dữ Liệu (Từ Database Thật)

        private async Task LoadComboBoxDataAsync()
        {
            try
            {
                var danhSachNhom = await _nhomFnBService.GetAllAsync();
                cboNhomSanPham.DataSource = danhSachNhom;
                cboNhomSanPham.DisplayMember = "TenNhom"; // Chữ hiển thị cho người dùng
                cboNhomSanPham.ValueMember = "NhomId";    // Mã ẩn bên dưới để lưu DB
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục nhóm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDataGridAsync(string keyword = "")
        {
            try
            {
                var data = await _sanPhamService.GetAllAsync(keyword);
                dvgSanPham.DataSource = data;

                // Đổi tên cột cho đẹp dựa trên DTO
                if (dvgSanPham.Columns.Contains("SanPhamId"))
                {
                    dvgSanPham.Columns["SanPhamId"].HeaderText = "Mã SP";
                    dvgSanPham.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
                    dvgSanPham.Columns["TenNhom"].HeaderText = "Nhóm SP";
                    dvgSanPham.Columns["GiaBan"].HeaderText = "Giá Bán";
                    dvgSanPham.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                    dvgSanPham.Columns["TonKho"].HeaderText = "Tồn Kho";
                    dvgSanPham.Columns["MoTa"].HeaderText = "Mô Tả";

                    // Ẩn các cột khóa ngoại và đường dẫn ảnh
                    if (dvgSanPham.Columns.Contains("NhomId")) dvgSanPham.Columns["NhomId"].Visible = false;
                    if (dvgSanPham.Columns.Contains("HinhAnhUrl")) dvgSanPham.Columns["HinhAnhUrl"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Các sự kiện Button do bạn gán trên giao diện

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn hình ảnh sản phẩm";
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedImagePath = ofd.FileName;
                    picHinhAnh.ImageLocation = _selectedImagePath;
                }
            }
        }

        private async void brnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput(out decimal giaBan, out int tonKho)) return;

            // Chống double-click
            brnThem.Enabled = false;

            try
            {
                var dto = new CreateSanPhamDto
                {
                    TenSanPham = tctTenSanPham.Text.Trim(),
                    NhomId = Convert.ToInt32(cboNhomSanPham.SelectedValue), // Lấy NhomId từ ComboBox
                    GiaBan = giaBan,
                    TonKho = tonKho,
                    MoTa = txtMoTa.Text.Trim(),
                    HinhAnhUrl = _selectedImagePath
                };

                var result = await _sanPhamService.CreateAsync(dto);

                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnLamMoi_Click(sender, e);
                }
                else MessageBox.Show(result.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally { brnThem.Enabled = true; }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedId == -1)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput(out decimal giaBan, out int tonKho)) return;
            btnSua.Enabled = false;

            try
            {
                var dto = new CreateSanPhamDto
                {
                    TenSanPham = tctTenSanPham.Text.Trim(),
                    NhomId = Convert.ToInt32(cboNhomSanPham.SelectedValue),
                    GiaBan = giaBan,
                    TonKho = tonKho,
                    MoTa = txtMoTa.Text.Trim(),
                    HinhAnhUrl = _selectedImagePath
                };

                var result = await _sanPhamService.UpdateAsync(_selectedId, dto);

                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnLamMoi_Click(sender, e);
                }
                else MessageBox.Show(result.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally { btnSua.Enabled = true; }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedId == -1)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var result = await _sanPhamService.DeleteAsync(_selectedId);

                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnLamMoi_Click(sender, e);
                }
                else MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLamMoi_Click(object sender, EventArgs e)
        {
            tctTenSanPham.Clear();
            txtGiaBan.Clear();
            txtTonKho.Clear();
            txtMoTa.Clear();
            txtTimKiem.Clear();
            if (cboNhomSanPham.Items.Count > 0) cboNhomSanPham.SelectedIndex = 0;

            picHinhAnh.Image = null;
            _selectedImagePath = "";
            _selectedId = -1;

            await LoadDataGridAsync();
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            await LoadDataGridAsync(keyword);
        }

        private void picHinhAnh_Click(object sender, EventArgs e) { }

        #endregion

        #region Helper: Kiểm tra Validation

        private bool ValidateInput(out decimal giaBan, out int tonKho)
        {
            giaBan = 0;
            tonKho = 0;

            if (string.IsNullOrWhiteSpace(tctTenSanPham.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tctTenSanPham.Focus();
                return false;
            }

            if (cboNhomSanPham.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhóm sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboNhomSanPham.Focus();
                return false;
            }

            if (!decimal.TryParse(txtGiaBan.Text, out giaBan) || giaBan < 0)
            {
                MessageBox.Show("Giá bán không hợp lệ (Phải là số >= 0)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGiaBan.Focus();
                return false;
            }

            if (!int.TryParse(txtTonKho.Text, out tonKho) || tonKho < 0)
            {
                MessageBox.Show("Tồn kho không hợp lệ (Phải là số nguyên >= 0)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTonKho.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #region DataGridView & ContextMenuStrip (Chuột phải)

        private void dvgSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LayDuLieuTuDong(e.RowIndex);
        }

        private void dvgSanPham_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dvgSanPham.ClearSelection();
                dvgSanPham.Rows[e.RowIndex].Selected = true;
                dvgSanPham.CurrentCell = dvgSanPham.Rows[e.RowIndex].Cells[0];

                LayDuLieuTuDong(e.RowIndex);
            }
            // Hỗ trợ click chuột trái nếu CellContentClick không bắt được do click ra ngoài text
            else if (e.Button == MouseButtons.Left && e.RowIndex >= 0)
            {
                LayDuLieuTuDong(e.RowIndex);
            }
        }

        private void LayDuLieuTuDong(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < dvgSanPham.Rows.Count)
            {
                DataGridViewRow row = dvgSanPham.Rows[rowIndex];

                // Đọc dữ liệu dựa trên thuộc tính của DTO
                if (row.Cells["SanPhamId"].Value != null)
                {
                    _selectedId = Convert.ToInt32(row.Cells["SanPhamId"].Value);
                    tctTenSanPham.Text = row.Cells["TenSanPham"].Value?.ToString();
                    cboNhomSanPham.SelectedValue = row.Cells["NhomId"].Value; // Auto chọn combobox
                    txtGiaBan.Text = Convert.ToDecimal(row.Cells["GiaBan"].Value).ToString("0");
                    txtTonKho.Text = row.Cells["TonKho"].Value?.ToString();
                    txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();

                    _selectedImagePath = row.Cells["HinhAnhUrl"].Value?.ToString() ?? "";

                    if (!string.IsNullOrEmpty(_selectedImagePath) && File.Exists(_selectedImagePath))
                    {
                        picHinhAnh.ImageLocation = _selectedImagePath;
                    }
                    else
                    {
                        picHinhAnh.Image = null;
                    }
                }
            }
        }

        private void sửaSảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnSua_Click(sender, e);
        }

        private void sửaXóaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnXoa_Click(sender, e);
        }

        #endregion
    }
}