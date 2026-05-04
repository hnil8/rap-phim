using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_UuDai : Form
    {
        private DataTable dtUuDai;
        private int _selectedRowIndex = -1;

        public frmAdmin_UuDai()
        {
            InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.None;
            this.MinimumSize = this.Size;

            this.Load += FrmAdmin_UuDai_Load;
            this.brnThem.Click += BtnThem_Click;
            this.btnSua.Click += BtnSua_Click;
            this.btnXoa.Click += BtnXoa_Click;
            this.btnLamMoi.Click += BtnLamMoi_Click;
            this.dgvUuDai.CellClick += DgvUuDai_CellClick;
        }

        private void FrmAdmin_UuDai_Load(object sender, EventArgs e)
        {
            if (cboLoaiGiam.Items.Count == 0)
            {
                cboLoaiGiam.Items.Add("%");
                cboLoaiGiam.Items.Add("VNĐ");
            }
            cboLoaiGiam.SelectedIndex = 0;

            SetupDataTable();
            LoadFakeData();
            ConfigureGrid();
        }

        // ════════════════════════════════════════════════════════════
        // 1. CẤU HÌNH DỮ LIỆU VÀ LƯỚI
        // ════════════════════════════════════════════════════════════
        private void SetupDataTable()
        {
            dtUuDai = new DataTable();
            dtUuDai.Columns.Add("Mã Code", typeof(string));
            dtUuDai.Columns.Add("Tên Chương Trình", typeof(string));
            dtUuDai.Columns.Add("Loại Giảm", typeof(string));
            dtUuDai.Columns.Add("Mức Giảm", typeof(string));
            dtUuDai.Columns.Add("Số Lượng", typeof(int));
            dtUuDai.Columns.Add("Từ Ngày", typeof(string));
            dtUuDai.Columns.Add("Đến Ngày", typeof(string));
            dtUuDai.Columns.Add("Trạng Thái", typeof(string));
        }

        private void LoadFakeData()
        {
            dtUuDai.Rows.Add("KM001", "Khai Trương Rạp", "VNĐ", "50.000", 1000, "20/04/2026", "30/04/2026", "Đang chạy");
            dtUuDai.Rows.Add("KM002", "Vé Cuối Tuần", "%", "10", 500, "01/04/2026", "15/04/2026", "Đã hết hạn");
            dtUuDai.Rows.Add("KM003", "Chờ Lễ 30/4", "VNĐ", "100.000", 200, "25/04/2026", "05/05/2026", "Chưa kích hoạt"); // <--- Thêm trạng thái chờ

            dgvUuDai.DataSource = dtUuDai;
        }

        private void ConfigureGrid()
        {
            dgvUuDai.ReadOnly = true;
            dgvUuDai.AllowUserToAddRows = false;
            dgvUuDai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUuDai.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUuDai.MultiSelect = false;
            dgvUuDai.RowHeadersVisible = false;

            dgvUuDai.CellFormatting += DgvUuDai_CellFormatting;
        }

        private void DgvUuDai_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUuDai.Columns[e.ColumnIndex].Name == "Trạng Thái" && e.Value != null)
            {
                string trangThai = e.Value.ToString();
                if (trangThai == "Đã hết hạn")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 192, 192); // Đỏ nhạt
                    e.CellStyle.ForeColor = Color.DarkRed;
                }
                else if (trangThai == "Đang chạy")
                {
                    e.CellStyle.BackColor = Color.FromArgb(192, 255, 192); // Xanh nhạt
                    e.CellStyle.ForeColor = Color.DarkGreen;
                }
                else if (trangThai == "Chưa kích hoạt")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 224, 192); // Cam nhạt/Vàng
                    e.CellStyle.ForeColor = Color.DarkOrange;
                }
            }
        }

        // ════════════════════════════════════════════════════════════
        // 2. XỬ LÝ SỰ KIỆN CLICK VÀO LƯỚI
        // ════════════════════════════════════════════════════════════
        private void DgvUuDai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedRowIndex = e.RowIndex;
                DataGridViewRow row = dgvUuDai.Rows[e.RowIndex];

                txtMaCode.Text = row.Cells["Mã Code"].Value?.ToString();
                txtTenChuongTrinh.Text = row.Cells["Tên Chương Trình"].Value?.ToString();
                cboLoaiGiam.SelectedItem = row.Cells["Loại Giảm"].Value?.ToString();

                txtMucGiam.Text = row.Cells["Mức Giảm"].Value?.ToString().Replace(".", "");
                txtSoLuong.Text = row.Cells["Số Lượng"].Value?.ToString();

                if (DateTime.TryParseExact(row.Cells["Từ Ngày"].Value?.ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime tuNgay))
                    dtpTuNgay.Value = tuNgay;

                if (DateTime.TryParseExact(row.Cells["Đến Ngày"].Value?.ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime denNgay))
                    dtpDenNgay.Value = denNgay;

                // [CẬP NHẬT] Đọc trạng thái để tích/bỏ tích Checkbox
                string currentStatus = row.Cells["Trạng Thái"].Value?.ToString();
                chkKichHoat.Checked = (currentStatus == "Đang chạy" || currentStatus == "Đã hết hạn");

                txtMaCode.Enabled = false;
            }
        }

        // ════════════════════════════════════════════════════════════
        // 3. XỬ LÝ CÁC NÚT CHỨC NĂNG
        // ════════════════════════════════════════════════════════════
        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaCode.Clear();
            txtTenChuongTrinh.Clear();
            txtMucGiam.Clear();
            txtSoLuong.Clear();
            cboLoaiGiam.SelectedIndex = 0;
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now.AddDays(7);

            chkKichHoat.Checked = true; // Mặc định là có bật kích hoạt
            txtMaCode.Enabled = true;
            _selectedRowIndex = -1;
            dgvUuDai.ClearSelection();
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            foreach (DataRow r in dtUuDai.Rows)
            {
                if (r["Mã Code"].ToString().Equals(txtMaCode.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Mã Code này đã tồn tại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            int soLuong = string.IsNullOrWhiteSpace(txtSoLuong.Text) ? 9999 : Convert.ToInt32(txtSoLuong.Text);

            // [CẬP NHẬT LOGIC TRẠNG THÁI]
            string trangThai = "Chưa kích hoạt";
            if (chkKichHoat.Checked)
            {
                trangThai = (dtpDenNgay.Value.Date >= DateTime.Now.Date && dtpTuNgay.Value.Date <= DateTime.Now.Date) ? "Đang chạy" : "Đã hết hạn";
            }

            decimal mucGiamNum = Convert.ToDecimal(txtMucGiam.Text);
            string mucGiamStr = cboLoaiGiam.SelectedItem.ToString() == "VNĐ" ? mucGiamNum.ToString("N0").Replace(",", ".") : mucGiamNum.ToString();

            dtUuDai.Rows.Add(
                txtMaCode.Text.Trim().ToUpper(),
                txtTenChuongTrinh.Text.Trim(),
                cboLoaiGiam.SelectedItem.ToString(),
                mucGiamStr,
                soLuong,
                dtpTuNgay.Value.ToString("dd/MM/yyyy"),
                dtpDenNgay.Value.ToString("dd/MM/yyyy"),
                trangThai
            );

            MessageBox.Show("Thêm Voucher thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            BtnLamMoi_Click(sender, e);
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (_selectedRowIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn một Voucher trên danh sách để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            int soLuong = string.IsNullOrWhiteSpace(txtSoLuong.Text) ? 9999 : Convert.ToInt32(txtSoLuong.Text);

            // [CẬP NHẬT LOGIC TRẠNG THÁI]
            string trangThai = "Chưa kích hoạt";
            if (chkKichHoat.Checked)
            {
                trangThai = (dtpDenNgay.Value.Date >= DateTime.Now.Date && dtpTuNgay.Value.Date <= DateTime.Now.Date) ? "Đang chạy" : "Đã hết hạn";
            }

            decimal mucGiamNum = Convert.ToDecimal(txtMucGiam.Text);
            string mucGiamStr = cboLoaiGiam.SelectedItem.ToString() == "VNĐ" ? mucGiamNum.ToString("N0").Replace(",", ".") : mucGiamNum.ToString();

            dtUuDai.Rows[_selectedRowIndex]["Tên Chương Trình"] = txtTenChuongTrinh.Text.Trim();
            dtUuDai.Rows[_selectedRowIndex]["Loại Giảm"] = cboLoaiGiam.SelectedItem.ToString();
            dtUuDai.Rows[_selectedRowIndex]["Mức Giảm"] = mucGiamStr;
            dtUuDai.Rows[_selectedRowIndex]["Số Lượng"] = soLuong;
            dtUuDai.Rows[_selectedRowIndex]["Từ Ngày"] = dtpTuNgay.Value.ToString("dd/MM/yyyy");
            dtUuDai.Rows[_selectedRowIndex]["Đến Ngày"] = dtpDenNgay.Value.ToString("dd/MM/yyyy");
            dtUuDai.Rows[_selectedRowIndex]["Trạng Thái"] = trangThai;

            MessageBox.Show("Cập nhật Voucher thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            BtnLamMoi_Click(sender, e);
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedRowIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn một Voucher để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc muốn xóa Voucher '{txtMaCode.Text}' không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dtUuDai.Rows.RemoveAt(_selectedRowIndex);
                BtnLamMoi_Click(sender, e);
            }
        }

        // ════════════════════════════════════════════════════════════
        // 4. KIỂM TRA TÍNH HỢP LỆ
        // ════════════════════════════════════════════════════════════
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaCode.Text) || string.IsNullOrWhiteSpace(txtTenChuongTrinh.Text) || string.IsNullOrWhiteSpace(txtMucGiam.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã Code, Tên Chương Trình và Mức Giảm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtMucGiam.Text, out decimal mucGiam) || mucGiam <= 0)
            {
                MessageBox.Show("Mức Giảm phải là một số lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboLoaiGiam.SelectedItem.ToString() == "%" && mucGiam > 100)
            {
                MessageBox.Show("Mức giảm theo phần trăm (%) không được vượt quá 100!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtSoLuong.Text) && (!int.TryParse(txtSoLuong.Text, out int sl) || sl <= 0))
            {
                MessageBox.Show("Số Lượng phải là số nguyên lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpDenNgay.Value.Date < dtpTuNgay.Value.Date)
            {
                MessageBox.Show("Ngày kết thúc không được nhỏ hơn ngày bắt đầu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // HÀM RỖNG CHIỀU LÒNG DESIGNER ĐỂ KHÔNG BỊ LỖI
        private void cboLoaiGiam_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}