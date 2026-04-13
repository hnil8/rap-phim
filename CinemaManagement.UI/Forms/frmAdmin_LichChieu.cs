using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_LichChieu : Form
    {
        private int _selectedId = -1;

        public frmAdmin_LichChieu()
        {
            InitializeComponent();
            DangKySuKien();
        }

        // ════════════════════════════════════════
        //  ĐĂNG KÝ SỰ KIỆN
        // ════════════════════════════════════════
        private void DangKySuKien()
        {
            this.Load += frmAdmin_LichChieu_Load;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;

            // Nút TÌM KIẾM liên kết với ComboBox
            btnTimKiem.Click += btnTimKiem_Click;

            // btnXoaLich đã đăng ký trong Designer: btnXoaLich.Click += btnXoaLich_Click
            dgvLichChieu.CellClick += dgvLichChieu_CellClick;
        }

        // ════════════════════════════════════════
        //  LOAD FORM
        // ════════════════════════════════════════
        private void frmAdmin_LichChieu_Load(object sender, EventArgs e)
        {
            NapDanhSachPhim();
            NapDanhSachRap();
            NapLuoiLichChieu();
            DatTrangThaiBanDau();
        }

        // ════════════════════════════════════════
        //  NẠP COMBOBOX (Thêm mục "Tất cả" để phục vụ tìm kiếm)
        // ════════════════════════════════════════
        private void NapDanhSachPhim()
        {
            cbChonPhim.Items.Clear();
            cbChonPhim.Items.Add("--- Tất cả phim ---"); // Index = 0

            // TODO: Thay bằng truy vấn CSDL thực tế
            cbChonPhim.Items.AddRange(new object[]
            {
                "Avengers: Endgame",
                "Spider-Man: No Way Home",
                "Interstellar",
                "Inception",
                "Dune: Part Two"
            });
            cbChonPhim.SelectedIndex = 0;
        }

        private void NapDanhSachRap()
        {
            cbChonRap.Items.Clear();
            cbChonRap.Items.Add("--- Tất cả rạp ---"); // Index = 0

            // TODO: Thay bằng truy vấn CSDL thực tế
            cbChonRap.Items.AddRange(new object[]
            {
                "Rạp CGV Vincom",
                "Rạp Galaxy Nguyễn Du",
                "Rạp Lotte Cinema"
            });
            cbChonRap.SelectedIndex = 0;
        }

        // ════════════════════════════════════════
        //  NẠP LƯỚI
        // ════════════════════════════════════════
        private void NapLuoiLichChieu()
        {
            dgvLichChieu.Rows.Clear();

            if (!dgvLichChieu.Columns.Contains("colId")) dgvLichChieu.Columns.Add("colId", "ID");
            if (!dgvLichChieu.Columns.Contains("colPhim")) dgvLichChieu.Columns.Add("colPhim", "Phim");
            if (!dgvLichChieu.Columns.Contains("colRap")) dgvLichChieu.Columns.Add("colRap", "Rạp");
            if (!dgvLichChieu.Columns.Contains("colNgay")) dgvLichChieu.Columns.Add("colNgay", "Ngày chiếu");
            if (!dgvLichChieu.Columns.Contains("colGio")) dgvLichChieu.Columns.Add("colGio", "Giờ chiếu");

            dgvLichChieu.Columns["colId"].Visible = false;
            dgvLichChieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvLichChieu.Columns["colCheck"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvLichChieu.Columns["colCheck"].Width = 50;
            dgvLichChieu.Columns["colCheck"].ReadOnly = false;

            // Dữ liệu mẫu
            dgvLichChieu.Rows.Add(false, 1, "Avengers: Endgame", "Rạp CGV Vincom", "12/04/2026", "09:00");
            dgvLichChieu.Rows.Add(false, 2, "Spider-Man: No Way Home", "Rạp Galaxy Nguyễn Du", "12/04/2026", "14:30");
            dgvLichChieu.Rows.Add(false, 3, "Interstellar", "Rạp Lotte Cinema", "13/04/2026", "19:00");
            dgvLichChieu.Rows.Add(false, 4, "Avengers: Endgame", "Rạp Lotte Cinema", "15/04/2026", "20:00");
        }

        private void DatTrangThaiBanDau()
        {
            _selectedId = -1;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        // ════════════════════════════════════════
        //  TÌM KIẾM THEO COMBOBOX
        // ════════════════════════════════════════
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            // Bắt buộc: Xóa trạng thái đang chọn dòng để tránh lỗi (Crash) khi ẩn dòng
            dgvLichChieu.CurrentCell = null;

            // Lấy từ khóa: Nếu Index > 0 (Tức là chọn phim cụ thể) thì lấy tên phim, còn = 0 thì chuỗi rỗng
            string timPhim = cbChonPhim.SelectedIndex > 0 ? cbChonPhim.SelectedItem.ToString() : "";
            string timRap = cbChonRap.SelectedIndex > 0 ? cbChonRap.SelectedItem.ToString() : "";

            foreach (DataGridViewRow row in dgvLichChieu.Rows)
            {
                if (row.IsNewRow) continue;

                string tenPhim = row.Cells["colPhim"].Value?.ToString() ?? "";
                string tenRap = row.Cells["colRap"].Value?.ToString() ?? "";

                // Logic: Nếu ô tìm kiếm rỗng thì luôn đúng, nếu có giá trị thì phải khớp với tên trong lưới
                bool matchPhim = string.IsNullOrEmpty(timPhim) || tenPhim == timPhim;
                bool matchRap = string.IsNullOrEmpty(timRap) || tenRap == timRap;

                // Chỉ hiển thị dòng nào thỏa mãn cả 2 điều kiện
                row.Visible = matchPhim && matchRap;
            }
        }

        // ════════════════════════════════════════
        //  CLICK LƯỚI ĐỔ DỮ LIỆU
        // ════════════════════════════════════════
        private void dgvLichChieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex == dgvLichChieu.Columns["colCheck"].Index) return;

            var row = dgvLichChieu.Rows[e.RowIndex];
            _selectedId = Convert.ToInt32(row.Cells["colId"].Value);

            string tenPhim = row.Cells["colPhim"].Value?.ToString() ?? "";
            int idxPhim = cbChonPhim.Items.IndexOf(tenPhim);
            if (idxPhim >= 0) cbChonPhim.SelectedIndex = idxPhim;

            string tenRap = row.Cells["colRap"].Value?.ToString() ?? "";
            int idxRap = cbChonRap.Items.IndexOf(tenRap);
            if (idxRap >= 0) cbChonRap.SelectedIndex = idxRap;

            if (DateTime.TryParseExact(row.Cells["colNgay"].Value?.ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime ngay))
            {
                dtpNgayChieu.Value = ngay;
            }

            txtGioChieu.Text = row.Cells["colGio"].Value?.ToString() ?? "";

            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        // ════════════════════════════════════════
        //  CRUD - THÊM / SỬA / XÓA
        // ════════════════════════════════════════
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            int newId = dgvLichChieu.Rows.Count + 1;
            dgvLichChieu.Rows.Add(false, newId, cbChonPhim.SelectedItem.ToString(), cbChonRap.SelectedItem.ToString(), dtpNgayChieu.Value.ToString("dd/MM/yyyy"), txtGioChieu.Text.Trim());

            MessageBox.Show("Thêm lịch chiếu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            XoaForm();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedId < 0) return;
            if (!KiemTraDuLieu()) return;

            foreach (DataGridViewRow row in dgvLichChieu.Rows)
            {
                if (Convert.ToInt32(row.Cells["colId"].Value) == _selectedId)
                {
                    row.Cells["colPhim"].Value = cbChonPhim.SelectedItem.ToString();
                    row.Cells["colRap"].Value = cbChonRap.SelectedItem.ToString();
                    row.Cells["colNgay"].Value = dtpNgayChieu.Value.ToString("dd/MM/yyyy");
                    row.Cells["colGio"].Value = txtGioChieu.Text.Trim();
                    break;
                }
            }
            MessageBox.Show("Cập nhật lịch chiếu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            XoaForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedId < 0) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa lịch chiếu này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dgvLichChieu.Rows)
                {
                    if (Convert.ToInt32(row.Cells["colId"].Value) == _selectedId)
                    {
                        dgvLichChieu.Rows.Remove(row);
                        break;
                    }
                }
                MessageBox.Show("Xóa lịch chiếu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                XoaForm();
            }
        }

        private void btnXoaLich_Click(object sender, EventArgs e)
        {
            dgvLichChieu.CommitEdit(DataGridViewDataErrorContexts.Commit);
            var rowsToDelete = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in dgvLichChieu.Rows)
            {
                if (row.IsNewRow) continue;
                if (Convert.ToBoolean(row.Cells["colCheck"].Value)) rowsToDelete.Add(row);
            }

            if (rowsToDelete.Count == 0)
            {
                MessageBox.Show("Vui lòng tích chọn ít nhất một lịch chiếu để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc muốn xóa {rowsToDelete.Count} lịch chiếu đã chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (var row in rowsToDelete) dgvLichChieu.Rows.Remove(row);
                MessageBox.Show($"Đã xóa {rowsToDelete.Count} lịch chiếu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                XoaForm();
            }
        }

        // ════════════════════════════════════════
        //  KIỂM TRA & LÀM SẠCH GIAO DIỆN
        // ════════════════════════════════════════
        private bool KiemTraDuLieu()
        {
            // Kiểm tra <= 0 vì Index 0 là "--- Tất cả ---", không phải là 1 phim/rạp hợp lệ để Thêm/Sửa
            if (cbChonPhim.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn một bộ phim cụ thể!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbChonRap.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn một rạp cụ thể!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtGioChieu.Text.Trim(), @"^\d{1,2}:\d{2}$"))
            {
                MessageBox.Show("Giờ chiếu không đúng (VD: 19:30)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void XoaForm()
        {
            if (cbChonPhim.Items.Count > 0) cbChonPhim.SelectedIndex = 0;
            if (cbChonRap.Items.Count > 0) cbChonRap.SelectedIndex = 0;
            dtpNgayChieu.Value = DateTime.Today;
            txtGioChieu.Text = string.Empty;

            // Hiển thị lại toàn bộ dòng bị ẩn do tìm kiếm trước đó
            dgvLichChieu.CurrentCell = null;
            foreach (DataGridViewRow row in dgvLichChieu.Rows)
            {
                if (!row.IsNewRow) row.Visible = true;
            }

            DatTrangThaiBanDau();
        }

        // Placeholder Event Designer
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2GroupBox1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void dgvLichChieu_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}