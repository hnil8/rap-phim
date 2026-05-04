using CinemaManagement.BLL.DTOs;
using CinemaManagement.BLL.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_TheLoai : Form
    {
        // Tiêm (Inject) Service tầng BLL vào Form
        private readonly ITheLoaiService _theLoaiService;

        // Biến lưu trữ ID của thể loại đang được chọn
        private int _selectedId = -1;

        // [CẬP NHẬT KIẾN TRÚC]: Form bắt buộc phải nhận ITheLoaiService qua Constructor
        public frmAdmin_TheLoai(ITheLoaiService theLoaiService)
        {
            InitializeComponent();
            _theLoaiService = theLoaiService;

            // 1. Gán sự kiện cho các nút trên Menu chuột phải (ContextMenuStrip)
            sửaThểLoạiToolStripMenuItem.Click += SửaThểLoạiToolStripMenuItem_Click;
            xóaThểLoạiToolStripMenuItem.Click += XóaThểLoạiToolStripMenuItem_Click;

            // 2. Gán sự kiện click chuột để chọn dòng trên DataGridView
            dgvTheLoai.CellMouseDown += DgvTheLoai_CellMouseDown;
            dgvTheLoai.CellContentClick += dgvTheLoai_CellContentClick;
        }

        private async void frmAdmin_TheLoai_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        #region Xử lý Dữ liệu (Gọi xuống tầng BLL - Database thật)

        private async Task LoadDataAsync()
        {
            try
            {
                // Gọi BLL lấy dữ liệu thật từ SQL Server
                var data = await _theLoaiService.GetAllAsync();
                dgvTheLoai.DataSource = data;

                // Đổi tên cột cho đẹp (Khớp với thuộc tính của TheLoaiDto)
                if (dgvTheLoai.Columns.Contains("TheLoaiId"))
                {
                    dgvTheLoai.Columns["TheLoaiId"].HeaderText = "Mã Thể Loại";
                    dgvTheLoai.Columns["TheLoaiId"].Width = 100;
                    dgvTheLoai.Columns["TenTheLoai"].HeaderText = "Tên Thể Loại";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenTheLoai.Text))
            {
                MessageBox.Show("Vui lòng nhập tên thể loại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenTheLoai.Focus();
                return false;
            }
            return true;
        }

        #endregion

        #region Các sự kiện Click Button Thêm, Sửa, Xóa, Làm Mới

        private async void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            btnThem.Enabled = false; // Chống click đúp (One-Click Shield)

            try
            {
                var dto = new CreateTheLoaiDto { TenTheLoai = txtTenTheLoai.Text.Trim() };
                var result = await _theLoaiService.CreateAsync(dto);

                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    btnLamMoi_Click(sender, e);
                }
                else MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally { btnThem.Enabled = true; }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedId == -1)
            {
                MessageBox.Show("Vui lòng chọn một thể loại để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInput()) return;

            btnSua.Enabled = false;

            try
            {
                var dto = new CreateTheLoaiDto { TenTheLoai = txtTenTheLoai.Text.Trim() };
                var result = await _theLoaiService.UpdateAsync(_selectedId, dto);

                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    btnLamMoi_Click(sender, e);
                }
                else MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally { btnSua.Enabled = true; }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedId == -1)
            {
                MessageBox.Show("Vui lòng chọn một thể loại để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa thể loại này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var result = await _theLoaiService.DeleteAsync(_selectedId);

                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadDataAsync();
                    btnLamMoi_Click(sender, e);
                }
                else MessageBox.Show(result.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTenTheLoai.Clear();
            _selectedId = -1;
            await LoadDataAsync();
            txtTenTheLoai.Focus();
        }

        #endregion

        #region Tương tác DataGridView & ContextMenuStrip (Menu Chuột Phải)

        private void dgvTheLoai_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LayDuLieuTuDongClick(e.RowIndex);
        }

        private void DgvTheLoai_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvTheLoai.ClearSelection();
                dgvTheLoai.Rows[e.RowIndex].Selected = true;
                dgvTheLoai.CurrentCell = dgvTheLoai.Rows[e.RowIndex].Cells[0];

                LayDuLieuTuDongClick(e.RowIndex);
            }
            // Hỗ trợ cả việc click chuột trái bình thường trên DataGridView
            else if (e.Button == MouseButtons.Left && e.RowIndex >= 0)
            {
                LayDuLieuTuDongClick(e.RowIndex);
            }
        }

        private void LayDuLieuTuDongClick(int rowIndex)
        {
            if (rowIndex >= 0)
            {
                DataGridViewRow row = dgvTheLoai.Rows[rowIndex];

                // Thuộc tính lấy từ TheLoaiDto
                if (row.Cells["TheLoaiId"].Value != null)
                {
                    _selectedId = Convert.ToInt32(row.Cells["TheLoaiId"].Value);
                    txtTenTheLoai.Text = row.Cells["TenTheLoai"].Value?.ToString();
                }
            }
        }

        private void SửaThểLoạiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnSua_Click(sender, e);
        }

        private void XóaThểLoạiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnXoa_Click(sender, e);
        }

        #endregion

        private void label1_Click(object sender, EventArgs e) { }
    }
}