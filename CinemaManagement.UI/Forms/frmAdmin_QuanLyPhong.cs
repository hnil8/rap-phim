using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities; // Hãy đảm bảo namespace này khớp với project của bạn
using System;
using System.Linq;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_QuanLyPhong : Form
    {
        private readonly CinemaDbContext _dbContext;
        private int _phongIdDuocChon = 0; // Biến lưu ID phòng đang chọn

        public frmAdmin_QuanLyPhong(CinemaDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;

            // Đăng ký sự kiện khi Form tải lên
            this.Load += FrmAdmin_QuanLyPhong_Load;

            // Đăng ký sự kiện cho DataGridView
            dgvPhong.CellClick += DgvPhong_CellClick;
            dgvPhong.CellMouseDown += DgvPhong_CellMouseDown; // Hỗ trợ click chuột phải

            // Đăng ký sự kiện cho Context Menu (Chuột phải)
            xóaToolStripMenuItem.Click += btnXoa_Click; // Tái sử dụng hàm xóa
            sửaToolStripMenuItem.Click += SửaToolStripMenuItem_Click;
        }

        private void FrmAdmin_QuanLyPhong_Load(object sender, EventArgs e)
        {
            LoadDanhSachPhong();
            cboLoaiPhong.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;
        }

        // ══════════════════════════════════════════════
        //  LOAD DỮ LIỆU LÊN LƯỚI
        // ══════════════════════════════════════════════
        private void LoadDanhSachPhong(string keyword = "")
        {
            try
            {
                // Lấy danh sách phòng chưa bị xóa và lọc theo từ khóa tìm kiếm (nếu có)
                var dsPhong = _dbContext.PhongChieus
                    .Where(p => p.IsDeleted == false &&
                               (string.IsNullOrEmpty(keyword) || p.TenPhong.Contains(keyword) || p.LoaiPhong.Contains(keyword)))
                    .Select(p => new
                    {
                        p.PhongId,
                        p.TenPhong,
                        p.SucChua,
                        p.LoaiPhong,
                        p.TrangThai
                    }).ToList();

                dgvPhong.DataSource = dsPhong;

                if (dgvPhong.Columns.Count > 0)
                {
                    dgvPhong.Columns["PhongId"].Visible = false;
                    dgvPhong.Columns["TenPhong"].HeaderText = "Tên Phòng";
                    dgvPhong.Columns["SucChua"].HeaderText = "Sức Chứa";
                    dgvPhong.Columns["LoaiPhong"].HeaderText = "Loại Phòng";
                    dgvPhong.Columns["TrangThai"].HeaderText = "Trạng Thái";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════
        //  TÌM KIẾM
        // ══════════════════════════════════════════════
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            LoadDanhSachPhong(keyword);
        }

        // ══════════════════════════════════════════════
        //  SỰ KIỆN CHỌN DÒNG (CHUỘT TRÁI & CHUỘT PHẢI)
        // ══════════════════════════════════════════════
        private void DgvPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            HienThiDuLieuLenForm(e.RowIndex);
        }

        // Xử lý khi click chuột phải để chọn dòng trước khi menu hiện ra
        private void DgvPhong_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                dgvPhong.ClearSelection();
                dgvPhong.Rows[e.RowIndex].Selected = true;
                HienThiDuLieuLenForm(e.RowIndex);
            }
        }

        private void HienThiDuLieuLenForm(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < dgvPhong.Rows.Count)
            {
                DataGridViewRow row = dgvPhong.Rows[rowIndex];
                _phongIdDuocChon = Convert.ToInt32(row.Cells["PhongId"].Value);
                txtTenPhong.Text = row.Cells["TenPhong"].Value?.ToString();
                txtSucChua.Text = row.Cells["SucChua"].Value?.ToString();
                cboLoaiPhong.Text = row.Cells["LoaiPhong"].Value?.ToString();
                cboTrangThai.Text = row.Cells["TrangThai"].Value?.ToString();
            }
        }

        // Xử lý khi chọn "Sửa" trên menu chuột phải
        private void SửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Dữ liệu đã được đưa lên các ô nhập liệu bên trái.\nHãy chỉnh sửa thông tin và bấm nút 'SỬA' màu xanh nhé!", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtTenPhong.Focus();
        }

        // ══════════════════════════════════════════════
        //  THÊM PHÒNG MỚI
        // ══════════════════════════════════════════════
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenPhong.Text))
            {
                MessageBox.Show("Vui lòng nhập tên phòng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                PhongChieu phongMoi = new PhongChieu
                {
                    TenPhong = txtTenPhong.Text.Trim(),
                    SucChua = int.TryParse(txtSucChua.Text, out int sc) ? sc : 0,
                    LoaiPhong = cboLoaiPhong.Text,
                    TrangThai = cboTrangThai.Text,
                    IsDeleted = false
                };

                _dbContext.PhongChieus.Add(phongMoi);
                _dbContext.SaveChanges();

                MessageBox.Show("Thêm phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachPhong();
                btnLamMoi_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm dữ liệu. Có thể tên phòng bị trùng!\nChi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════
        //  SỬA THÔNG TIN PHÒNG
        // ══════════════════════════════════════════════
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_phongIdDuocChon == 0)
            {
                MessageBox.Show("Vui lòng chọn một phòng từ danh sách để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var phongCuaDB = _dbContext.PhongChieus.Find(_phongIdDuocChon);
                if (phongCuaDB != null)
                {
                    phongCuaDB.TenPhong = txtTenPhong.Text.Trim();
                    phongCuaDB.SucChua = int.TryParse(txtSucChua.Text, out int sc) ? sc : 0;
                    phongCuaDB.LoaiPhong = cboLoaiPhong.Text;
                    phongCuaDB.TrangThai = cboTrangThai.Text;

                    _dbContext.SaveChanges();
                    MessageBox.Show("Cập nhật phòng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachPhong(txtTimKiem.Text.Trim()); // Load lại và giữ nguyên trạng thái tìm kiếm
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════
        //  XÓA PHÒNG (XÓA MỀM)
        // ══════════════════════════════════════════════
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_phongIdDuocChon == 0)
            {
                MessageBox.Show("Vui lòng chọn một phòng để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa phòng này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var phongCuaDB = _dbContext.PhongChieus.Find(_phongIdDuocChon);
                    if (phongCuaDB != null)
                    {
                        phongCuaDB.IsDeleted = true;
                        _dbContext.SaveChanges();

                        MessageBox.Show("Đã xóa phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDanhSachPhong(txtTimKiem.Text.Trim());
                        btnLamMoi_Click(null, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ══════════════════════════════════════════════
        //  LÀM MỚI
        // ══════════════════════════════════════════════
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTenPhong.Clear();
            txtSucChua.Clear();
            txtTimKiem.Clear();
            cboLoaiPhong.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;
            _phongIdDuocChon = 0;
            LoadDanhSachPhong(); // Load lại toàn bộ
        }

        // ── HÀM TRỐNG ĐỂ TRÁNH LỖI TỪ DESIGNER ──
        private void dgvPhong_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void cmsPhong_Opening(object sender, System.ComponentModel.CancelEventArgs e) { }
    }
}