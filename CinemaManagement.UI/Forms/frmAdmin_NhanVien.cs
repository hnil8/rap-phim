using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_NhanVien : Form
    {
        private DataTable dtNhanVien;

        public frmAdmin_NhanVien()
        {
            InitializeComponent();

            // Đăng ký sự kiện (Events)
            this.Load += new EventHandler(frmAdmin_NhanVien_Load);
            this.btnThem.Click += new EventHandler(btnThem_Click);
            this.btnSua.Click += new EventHandler(btnSua_Click);
            this.btnXoa.Click += new EventHandler(btnXoa_Click); // Xóa 1 dòng
            this.btnXoaNV.Click += new EventHandler(btnXoaNV_Click); // Xóa nhiều dòng bằng Checkbox
            this.btnLamMoi.Click += new EventHandler(btnLamMoi_Click);

            this.dgvNhanVien.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);

            // Xóa sự kiện TextChanged cũ vì chúng ta sẽ chuyển sang tìm kiếm bằng Nút Bấm
            // this.txtTimKiemNV.TextChanged += new EventHandler(txtTimKiemNV_TextChanged); 
        }

        private void frmAdmin_NhanVien_Load(object sender, EventArgs e)
        {
            // 1. Cài đặt ComboBox
            cbVaiTro.Items.Add("Admin");      // Chỉ Chủ rạp
            cbVaiTro.Items.Add("Quản lý");    // Người quản lý rạp
            cbVaiTro.Items.Add("Nhân viên");  // Bán vé, bắp nước

            // 2. Cài đặt cấu trúc dữ liệu
            dtNhanVien = new DataTable();
            dtNhanVien.Columns.Add("Mã NV", typeof(int));
            dtNhanVien.Columns.Add("Họ Tên", typeof(string));
            dtNhanVien.Columns.Add("Email", typeof(string));
            dtNhanVien.Columns.Add("Số Điện Thoại", typeof(string));
            dtNhanVien.Columns.Add("Vai Trò", typeof(string));

            // 3. Dữ liệu mẫu mồi
            dtNhanVien.Rows.Add(1, "Hoàng Linh", "admin@betacinema.vn", "0988123456", "Admin");
            dtNhanVien.Rows.Add(2, "Nguyễn Văn A", "nva@betacinema.vn", "0912333444", "Nhân viên");
            dtNhanVien.Rows.Add(3, "Trần Thị B", "ttb@betacinema.vn", "0977888999", "Nhân viên");

            dgvNhanVien.DataSource = dtNhanVien;

            // 4. Thiết lập bảo mật cho Lưới (GridView)
            dgvNhanVien.ReadOnly = false; // Mở khóa tổng thể để bấm được Checkbox
            dgvNhanVien.AllowUserToAddRows = false; // Tắt dòng trống ở cuối

            foreach (DataGridViewColumn col in dgvNhanVien.Columns)
            {
                // Khóa tất cả các cột chữ, chỉ cho phép tương tác với cột Checkbox
                if (col.Name != "colCheck") col.ReadOnly = true;
            }
        }

        // --- CÁC NÚT CHỨC NĂNG CƠ BẢN ---

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int newId = dtNhanVien.Rows.Count > 0 ? Convert.ToInt32(dtNhanVien.Rows[dtNhanVien.Rows.Count - 1]["Mã NV"]) + 1 : 1;
            dtNhanVien.Rows.Add(newId, txtHoTen.Text.Trim(), txtEmail.Text.Trim(), txtSoDienThoai.Text.Trim(), cbVaiTro.SelectedItem.ToString());

            MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnLamMoi_Click(sender, e);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow != null)
            {
                int rowIndex = dgvNhanVien.CurrentRow.Index;
                dtNhanVien.Rows[rowIndex]["Họ Tên"] = txtHoTen.Text.Trim();
                dtNhanVien.Rows[rowIndex]["Email"] = txtEmail.Text.Trim();
                dtNhanVien.Rows[rowIndex]["Số Điện Thoại"] = txtSoDienThoai.Text.Trim();
                dtNhanVien.Rows[rowIndex]["Vai Trò"] = cbVaiTro.SelectedItem.ToString();

                MessageBox.Show("Đã cập nhật thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLamMoi_Click(sender, e);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Tính năng: Xóa 1 dòng đang được click chọn
            if (dgvNhanVien.CurrentRow != null)
            {
                if (dgvNhanVien.CurrentRow.Cells["Vai Trò"].Value.ToString() == "Admin")
                {
                    MessageBox.Show("Không thể xóa Admin!", "Lỗi bảo mật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show("Xóa nhân viên đang chọn?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dtNhanVien.Rows.RemoveAt(dgvNhanVien.CurrentRow.Index);
                    btnLamMoi_Click(sender, e);
                }
            }
        }

        // --- TÍNH NĂNG XÓA HÀNG LOẠT (CHECKBOX) ---

        private void btnXoaNV_Click(object sender, EventArgs e)
        {
            dgvNhanVien.EndEdit(); // Ép hệ thống cập nhật dấu tích Checkbox mới nhất

            List<DataRow> listDelete = new List<DataRow>();
            int adminCount = 0;

            // Quét tìm các dòng được tích
            foreach (DataGridViewRow row in dgvNhanVien.Rows)
            {
                if (row.IsNewRow) continue;

                bool isChecked = row.Cells["colCheck"].Value != null && Convert.ToBoolean(row.Cells["colCheck"].Value);

                if (isChecked)
                {
                    DataRowView drv = (DataRowView)row.DataBoundItem;
                    if (drv != null)
                    {
                        if (drv.Row["Vai Trò"].ToString() == "Admin")
                        {
                            adminCount++;
                            continue; // Bỏ qua Admin, không cho vào danh sách tử hình
                        }
                        listDelete.Add(drv.Row);
                    }
                }
            }

            // Thực thi xóa
            if (listDelete.Count > 0)
            {
                string message = adminCount > 0
                    ? $"Xác nhận xóa {listDelete.Count} nhân viên đã chọn? (Hệ thống tự động bỏ qua {adminCount} tài khoản Admin)"
                    : $"Bạn có chắc chắn muốn xóa {listDelete.Count} nhân viên đã chọn?";

                if (MessageBox.Show(message, "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (DataRow r in listDelete)
                    {
                        dtNhanVien.Rows.Remove(r);
                    }
                    MessageBox.Show("Đã xóa thành công các nhân viên được chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnLamMoi_Click(sender, e);
                }
            }
            else
            {
                string warning = adminCount > 0
                    ? "Không thể xóa tài khoản Admin!"
                    : "Vui lòng tích vào ô Checkbox ít nhất một nhân viên để xóa.";
                MessageBox.Show(warning, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- CÁC SỰ KIỆN TƯƠNG TÁC GIAO DIỆN ---

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                txtHoTen.Text = row.Cells["Họ Tên"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtSoDienThoai.Text = row.Cells["Số Điện Thoại"].Value?.ToString();
                cbVaiTro.SelectedItem = row.Cells["Vai Trò"].Value?.ToString();
                txtMatKhau.Clear();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtHoTen.Clear();
            txtEmail.Clear();
            txtSoDienThoai.Clear();
            txtMatKhau.Clear();
            cbVaiTro.SelectedIndex = 1;
            dgvNhanVien.ClearSelection();

            // Xóa bộ lọc tìm kiếm để hiện lại toàn bộ danh sách
            txtTimKiemNV.Clear();
            if (dtNhanVien != null)
            {
                dtNhanVien.DefaultView.RowFilter = "";
            }

            // Xóa tất cả các dấu tích trên lưới
            foreach (DataGridViewRow row in dgvNhanVien.Rows)
            {
                if (row.Cells["colCheck"].Value != null)
                {
                    row.Cells["colCheck"].Value = false;
                }
            }
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        // ════════════════════════════════════════════════════════════
        //  TÍNH NĂNG TÌM KIẾM NHÂN VIÊN BẰNG NÚT BẤM
        // ════════════════════════════════════════════════════════════
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtNhanVien == null) return;

                string filter = txtTimKiemNV.Text.Trim();

                // Lọc trực tiếp trên DataTable bằng cú pháp SQL nội bộ
                dtNhanVien.DefaultView.RowFilter = string.IsNullOrEmpty(filter)
                    ? ""
                    : $"[Họ Tên] LIKE '%{filter}%' OR [Email] LIKE '%{filter}%' OR [Số Điện Thoại] LIKE '%{filter}%'";

                // Hiển thị thông báo nếu không tìm thấy ai
                if (dtNhanVien.DefaultView.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào khớp với từ khóa!", "Kết quả tìm kiếm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}