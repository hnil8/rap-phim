using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CinemaManagement.UI
{
    public partial class frmQuanLyKhachHang : Form
    {
        private DataTable dtKhachHang;
        private int _selectedRowIndex = -1;

        public frmQuanLyKhachHang()
        {
            InitializeComponent();

            // CHỐNG VỠ GIAO DIỆN
            this.AutoScaleMode = AutoScaleMode.None;
            this.MinimumSize = this.Size;

            // Đăng ký các sự kiện bổ sung
            this.Load += FrmQuanLyKhachHang_Load;

            // Xử lý logic tìm kiếm Live (Gõ đến đâu lọc đến đó)
            this.txtTimKiem.TextChanged += TxtTimKiem_TextChanged;

            // Xử lý tự động nhảy Hạng khi gõ Điểm
            this.txtDiem.TextChanged += TxtDiem_TextChanged;

            // Xử lý click vào dòng trên lưới để lấy dữ liệu
            this.dvgKhachHang.CellClick += DvgKhachHang_CellClick;

            // Format màu sắc Hạng thẻ (Kim cương, Vàng, Bạc...)
            this.dvgKhachHang.CellFormatting += DvgKhachHang_CellFormatting;
        }

        private void FrmQuanLyKhachHang_Load(object sender, EventArgs e)
        {
            // Thiết lập ComboBox Hạng thành viên
            if (cbHang.Items.Count == 0)
            {
                cbHang.Items.AddRange(new string[] { "Đồng", "Bạc", "Vàng", "Kim cương" });
            }
            cbHang.SelectedIndex = 0;
            cbHang.Enabled = false; // Khóa lại vì Hạng sẽ tự động nhảy theo Điểm

            SetupDataTable();
            LoadFakeData();
            ConfigureGrid();
            TaoMaKhachHangTuDong();
        }

        // ════════════════════════════════════════════════════════════
        // 1. CẤU HÌNH DỮ LIỆU VÀ LƯỚI
        // ════════════════════════════════════════════════════════════
        private void SetupDataTable()
        {
            dtKhachHang = new DataTable();
            dtKhachHang.Columns.Add("Mã KH", typeof(string));
            dtKhachHang.Columns.Add("Họ Tên", typeof(string));
            dtKhachHang.Columns.Add("Số Điện Thoại", typeof(string));
            dtKhachHang.Columns.Add("Email", typeof(string));
            dtKhachHang.Columns.Add("Điểm", typeof(int));
            dtKhachHang.Columns.Add("Hạng", typeof(string));
        }

        private void LoadFakeData()
        {
            // Fake data bám sát theo hình ảnh thiết kế
            dtKhachHang.Rows.Add("01", "Nguyễn Hằng", "09724567875", "cinema2@gmail.com", 2000, "Kim cương");
            dtKhachHang.Rows.Add("02", "Họ Tên", "09724567873", "nncang47@gmail.com", 500, "Vàng");
            dtKhachHang.Rows.Add("03", "Hành Phân", "09724567884", "martinn2@gmail.com", 200, "Vàng");
            dtKhachHang.Rows.Add("04", "Tính Tihi", "09724567979", "emails12@gmail.com", 350, "Bạc");
            dtKhachHang.Rows.Add("05", "Tham Thang", "09724567933", "thaen67@gmail.com", 200, "Vàng");
            dtKhachHang.Rows.Add("06", "Thạch Huy", "09724567877", "qhaung7@gmail.com", 200, "Bạc");
            dtKhachHang.Rows.Add("07", "Hịnh Kinh", "09724563924", "mahrag6@gmail.com", 150, "Bạc");
            dtKhachHang.Rows.Add("08", "Ngân Bia", "09724567958", "mgnn61@gmail.com", 250, "Đồng");

            dvgKhachHang.DataSource = dtKhachHang;
        }

        private void ConfigureGrid()
        {
            dvgKhachHang.ReadOnly = true;
            dvgKhachHang.AllowUserToAddRows = false;
            dvgKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dvgKhachHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dvgKhachHang.MultiSelect = false;
            dvgKhachHang.RowHeadersVisible = false;
        }

        private void DvgKhachHang_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Tô màu các Hạng thẻ cho đẹp mắt giống trong ảnh
            if (dvgKhachHang.Columns[e.ColumnIndex].Name == "Hạng" && e.Value != null)
            {
                string hang = e.Value.ToString();
                if (hang == "Kim cương")
                {
                    e.CellStyle.ForeColor = Color.LightSeaGreen;
                    e.CellStyle.Font = new Font(dvgKhachHang.Font, FontStyle.Bold);
                }
                else if (hang == "Vàng")
                {
                    e.CellStyle.ForeColor = Color.Goldenrod;
                    e.CellStyle.Font = new Font(dvgKhachHang.Font, FontStyle.Bold);
                }
                else if (hang == "Bạc")
                {
                    e.CellStyle.ForeColor = Color.DimGray;
                    e.CellStyle.Font = new Font(dvgKhachHang.Font, FontStyle.Bold);
                }
                else // Đồng
                {
                    e.CellStyle.ForeColor = Color.Peru;
                }
            }
        }

        // ════════════════════════════════════════════════════════════
        // 2. TỰ ĐỘNG HÓA VÀ RÀNG BUỘC
        // ════════════════════════════════════════════════════════════
        private void TaoMaKhachHangTuDong()
        {
            int soLuongKH = dtKhachHang.Rows.Count + 1;
            txtMaKhachHang.Text = soLuongKH.ToString("D2"); // Format: 01, 02, 03...
            txtMaKhachHang.Enabled = false; // Khóa lại không cho tự gõ mã
        }

        private void TxtDiem_TextChanged(object sender, EventArgs e)
        {
            // Tự động xếp hạng khi gõ điểm
            if (int.TryParse(txtDiem.Text, out int diem))
            {
                if (diem >= 1000) cbHang.SelectedItem = "Kim cương";
                else if (diem >= 500) cbHang.SelectedItem = "Vàng";
                else if (diem >= 200) cbHang.SelectedItem = "Bạc";
                else cbHang.SelectedItem = "Đồng";
            }
        }

        // ════════════════════════════════════════════════════════════
        // 3. TƯƠNG TÁC LƯỚI VÀ NÚT BẤM (ĐÃ CHUẨN HÓA THEO DESIGNER)
        // ════════════════════════════════════════════════════════════
        private void DvgKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedRowIndex = e.RowIndex;
                DataGridViewRow row = dvgKhachHang.Rows[e.RowIndex];

                txtMaKhachHang.Text = row.Cells["Mã KH"].Value?.ToString();
                txtHoTen.Text = row.Cells["Họ Tên"].Value?.ToString();
                txtSoDienThoai.Text = row.Cells["Số Điện Thoại"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtDiem.Text = row.Cells["Điểm"].Value?.ToString();
                cbHang.SelectedItem = row.Cells["Hạng"].Value?.ToString();
            }
        }

        private void btnLamMoi_Click_1(object sender, EventArgs e)
        {
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            txtEmail.Clear();
            txtDiem.Text = "0";
            txtTimKiem.Clear();

            TaoMaKhachHangTuDong();

            _selectedRowIndex = -1;
            dvgKhachHang.ClearSelection();
            dtKhachHang.DefaultView.RowFilter = "";
        }

        private void brnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            foreach (DataRow r in dtKhachHang.Rows)
            {
                if (r["Số Điện Thoại"].ToString() == txtSoDienThoai.Text.Trim())
                {
                    MessageBox.Show("Số điện thoại này đã được đăng ký!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            dtKhachHang.Rows.Add(
                txtMaKhachHang.Text,
                txtHoTen.Text.Trim(),
                txtSoDienThoai.Text.Trim(),
                txtEmail.Text.Trim(),
                int.Parse(txtDiem.Text),
                cbHang.SelectedItem.ToString()
            );

            MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnLamMoi_Click_1(sender, e);
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (_selectedRowIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            dtKhachHang.Rows[_selectedRowIndex]["Họ Tên"] = txtHoTen.Text.Trim();
            dtKhachHang.Rows[_selectedRowIndex]["Số Điện Thoại"] = txtSoDienThoai.Text.Trim();
            dtKhachHang.Rows[_selectedRowIndex]["Email"] = txtEmail.Text.Trim();
            dtKhachHang.Rows[_selectedRowIndex]["Điểm"] = int.Parse(txtDiem.Text);
            dtKhachHang.Rows[_selectedRowIndex]["Hạng"] = cbHang.SelectedItem.ToString();

            MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnLamMoi_Click_1(sender, e);
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            if (_selectedRowIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Xóa dữ liệu của khách hàng '{txtHoTen.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dtKhachHang.Rows.RemoveAt(_selectedRowIndex);
                btnLamMoi_Click_1(sender, e);
            }
        }

        // ════════════════════════════════════════════════════════════
        // 4. TÌM KIẾM VÀ KIỂM TRA ĐẦU VÀO
        // ════════════════════════════════════════════════════════════
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            ThucHienTimKiem();
        }

        private void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            ThucHienTimKiem();
        }

        private void ThucHienTimKiem()
        {
            if (dtKhachHang == null) return;
            string filter = txtTimKiem.Text.Trim();

            dtKhachHang.DefaultView.RowFilter = string.IsNullOrEmpty(filter)
                ? ""
                : $"[Họ Tên] LIKE '%{filter}%' OR [Số Điện Thoại] LIKE '%{filter}%'";
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                MessageBox.Show("Họ tên và Số điện thoại không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!Regex.IsMatch(txtSoDienThoai.Text, @"^\d{9,11}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ (Phải từ 9-11 số)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtDiem.Text, out int diem) || diem < 0)
            {
                MessageBox.Show("Điểm tích lũy phải là số nguyên dương!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // ════════════════════════════════════════════════════════════
        // 5. CÁC HÀM RỖNG ĐỂ CHỐNG LỖI DESIGNER (CS0103)
        // ════════════════════════════════════════════════════════════
        private void cbHang_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dvgKhachHang_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}