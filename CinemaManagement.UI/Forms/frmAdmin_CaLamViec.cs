using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_CaLamViec : Form
    {
        private DataTable dtCaLamViec;
        private int _selectedRowIndex = -1;

        public frmAdmin_CaLamViec()
        {
            InitializeComponent();

            // Đăng ký sự kiện Load Form
            this.Load += FrmAdmin_CaLamViec_Load;

            // Đăng ký thêm sự kiện CellClick để bấm vào đâu trên lưới cũng nhận dữ liệu
            this.dgvCaLamViec.CellClick += DgvCaLamViec_CellClick;
        }

        private void FrmAdmin_CaLamViec_Load(object sender, EventArgs e)
        {
            // Ép format thời gian cho DateTimePicker (trường hợp bên designer chưa set)
            dtpTuGio.Format = DateTimePickerFormat.Custom;
            dtpTuGio.CustomFormat = "HH:mm";
            dtpTuGio.ShowUpDown = true;

            dtpDenGio.Format = DateTimePickerFormat.Custom;
            dtpDenGio.CustomFormat = "HH:mm";
            dtpDenGio.ShowUpDown = true;

            SetupDataTable();
            LoadFakeData();
            ConfigureGrid();
            TaoMaCaTuDong();
        }

        // ════════════════════════════════════════════════════════════
        // 1. CẤU HÌNH DỮ LIỆU & LƯỚI
        // ════════════════════════════════════════════════════════════
        private void SetupDataTable()
        {
            dtCaLamViec = new DataTable();
            dtCaLamViec.Columns.Add("Mã Ca", typeof(string));
            dtCaLamViec.Columns.Add("Tên Ca", typeof(string));
            dtCaLamViec.Columns.Add("Từ Giờ", typeof(string));
            dtCaLamViec.Columns.Add("Đến Giờ", typeof(string));
        }

        private void LoadFakeData()
        {
            dtCaLamViec.Rows.Add("CA01", "Ca Sáng", "08:00", "15:00");
            dtCaLamViec.Rows.Add("CA02", "Ca Tối", "15:00", "22:00");
            dtCaLamViec.Rows.Add("CA03", "Ca Gãy Trưa", "10:00", "14:00");

            dgvCaLamViec.DataSource = dtCaLamViec;
        }

        private void ConfigureGrid()
        {
            dgvCaLamViec.ReadOnly = true;
            dgvCaLamViec.AllowUserToAddRows = false;
            dgvCaLamViec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCaLamViec.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCaLamViec.MultiSelect = false;
        }

        private void TaoMaCaTuDong()
        {
            int soLuong = dtCaLamViec.Rows.Count + 1;
            txtMaCa.Text = "CA" + soLuong.ToString("D2");
        }

        // ════════════════════════════════════════════════════════════
        // 2. TƯƠNG TÁC LƯỚI & TÌM KIẾM
        // ════════════════════════════════════════════════════════════
        private void DgvCaLamViec_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _selectedRowIndex = e.RowIndex;
                DataGridViewRow row = dgvCaLamViec.Rows[e.RowIndex];

                txtMaCa.Text = row.Cells["Mã Ca"].Value?.ToString();
                txtTenCa.Text = row.Cells["Tên Ca"].Value?.ToString();

                // Lấy giờ gán ngược lại cho DateTimePicker
                if (DateTime.TryParseExact(row.Cells["Từ Giờ"].Value?.ToString(), "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime tuGio))
                    dtpTuGio.Value = tuGio;

                if (DateTime.TryParseExact(row.Cells["Đến Giờ"].Value?.ToString(), "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime denGio))
                    dtpDenGio.Value = denGio;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (dtCaLamViec != null)
            {
                dtCaLamViec.DefaultView.RowFilter = string.IsNullOrEmpty(keyword) ? "" : $"[Tên Ca] LIKE '%{keyword}%'";
            }
        }

        // ════════════════════════════════════════════════════════════
        // 3. XỬ LÝ CÁC NÚT CHỨC NĂNG (Đã khớp tên với Designer)
        // ════════════════════════════════════════════════════════════
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTenCa.Clear();
            txtTimKiem.Clear();
            dtpTuGio.Value = DateTime.Today.AddHours(8);
            dtpDenGio.Value = DateTime.Today.AddHours(15);

            _selectedRowIndex = -1;
            dgvCaLamViec.ClearSelection();
            if (dtCaLamViec != null) dtCaLamViec.DefaultView.RowFilter = "";

            TaoMaCaTuDong();
        }

        // Tên nút "Thêm" bị gõ nhầm chữ r trong designer (brnThem)
        private void brnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenCa.Text))
            {
                MessageBox.Show("Vui lòng nhập tên Ca làm việc!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tuGio = dtpTuGio.Value.ToString("HH:mm");
            string denGio = dtpDenGio.Value.ToString("HH:mm");

            dtCaLamViec.Rows.Add(txtMaCa.Text, txtTenCa.Text.Trim(), tuGio, denGio);

            MessageBox.Show("Thêm ca làm việc mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnLamMoi_Click(sender, e);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedRowIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn 1 dòng trên danh sách để cập nhật!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenCa.Text)) return;

            dtCaLamViec.Rows[_selectedRowIndex]["Tên Ca"] = txtTenCa.Text.Trim();
            dtCaLamViec.Rows[_selectedRowIndex]["Từ Giờ"] = dtpTuGio.Value.ToString("HH:mm");
            dtCaLamViec.Rows[_selectedRowIndex]["Đến Giờ"] = dtpDenGio.Value.ToString("HH:mm");

            MessageBox.Show("Cập nhật thông tin Ca làm việc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnLamMoi_Click(sender, e);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedRowIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn ca làm việc để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenCa = dtCaLamViec.Rows[_selectedRowIndex]["Tên Ca"].ToString();
            if (MessageBox.Show($"Bạn có chắc muốn xóa '{tenCa}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dtCaLamViec.Rows.RemoveAt(_selectedRowIndex);
                btnLamMoi_Click(sender, e);
            }
        }

        // ════════════════════════════════════════════════════════════
        // 4. CÁC HÀM RỖNG CHỐNG LỖI DESIGNER (CS0103)
        // ════════════════════════════════════════════════════════════
        private void dgvCaLamViec_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dtpTuGio_ValueChanged(object sender, EventArgs e) { }
    }
}