using System;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmMain : Form
    {
        // ──────────────────────────────────────────────
        //  Thông tin người dùng đăng nhập (được truyền từ frmLogin)
        // ──────────────────────────────────────────────
        public static string TenNguoiDung { get; set; } = "";
        public static string VaiTro { get; set; } = "";   // "Admin" hoặc "NhanVien"

        // Timer để cập nhật thời gian thực
        private System.Windows.Forms.Timer _timer;

        // Form đang được nhúng vào pnlDesktop
        private Form _currentEmbeddedForm;

        public frmMain()
        {
            InitializeComponent();
            SetupTimer();
        }

        // ──────────────────────────────────────────────
        //  KHỞI TẠO
        // ──────────────────────────────────────────────
        private void frmMain_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin người dùng
            lblTenNguoiDung.Text = TenNguoiDung;
            lblVaiTro.Text = VaiTro;

            // Ẩn/hiện các nút theo vai trò
            ApplyRolePermissions();

            // Mặc định hiển thị trang chủ
            LoadHomeContent();
        }

        // ──────────────────────────────────────────────
        //  TIMER THỜI GIAN THỰC
        // ──────────────────────────────────────────────
        private void SetupTimer()
        {
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += (s, e) =>
                lblThoiGian.Text = DateTime.Now.ToString("HH:mm:ss  dd/MM/yyyy");
            _timer.Start();
        }

        // ──────────────────────────────────────────────
        //  NHÚNG FORM VÀO pnlDesktop
        // ──────────────────────────────────────────────
        private void EmbedForm(Form form)
        {
            // Đóng form đang hiển thị (nếu có)
            if (_currentEmbeddedForm != null && !_currentEmbeddedForm.IsDisposed)
            {
                _currentEmbeddedForm.Close();
                _currentEmbeddedForm.Dispose();
            }

            pnlDesktop.Controls.Clear();

            // Cấu hình form mới để nhúng vào panel
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.AutoScroll = true;

            pnlDesktop.Controls.Add(form);
            form.Show();

            _currentEmbeddedForm = form;
        }

        // ──────────────────────────────────────────────
        //  TRANG CHỦ MẶC ĐỊNH
        // ──────────────────────────────────────────────
        private void LoadHomeContent()
        {
            // Thay bằng EmbedForm(new frmTrangChu()) khi đã tạo form đó
            pnlDesktop.Controls.Clear();
            var lbl = new Label
            {
                Text = $"CHÀO MỪNG, {TenNguoiDung.ToUpper()}!",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White,
                BackColor = System.Drawing.Color.Transparent
            };
            pnlDesktop.Controls.Add(lbl);
        }

        // ──────────────────────────────────────────────
        //  PHÂN QUYỀN THEO VAI TRÒ
        // ──────────────────────────────────────────────
        private void ApplyRolePermissions()
        {
            bool isAdmin = VaiTro?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;

            btnWeb.Visible = isAdmin;
            btnPhim.Visible = isAdmin;
            btnLichChieu.Visible = isAdmin;
            btnSanPham.Visible = isAdmin;
            btnUuDai.Visible = isAdmin;
            btnNhanVien.Visible = isAdmin;
            btnDoanhThu.Visible = isAdmin;
            btnQuanLyKhachHang.Visible = isAdmin;

            // Nhân viên luôn thấy 2 nút này
            btnStaffPos.Visible = true;
            btnStaffQuanLyDon.Visible = true;
        }

        // ──────────────────────────────────────────────
        //  EVENT HANDLERS — khớp với Designer
        //  Mỗi nút chỉ có 1 handler, không dùng SetupEventHandlers()
        //  để tránh double-fire
        // ──────────────────────────────────────────────
        private void btnMain_Click(object sender, EventArgs e)
            => LoadHomeContent();

        private void btnWeb_Click(object sender, EventArgs e)
            => EmbedForm(new frmAdmin_WebPreview());

        private void btnPhim_Click(object sender, EventArgs e)
            => EmbedForm(new frmAdmin_Phim());

        private void btnLichChieu_Click(object sender, EventArgs e)
            => EmbedForm(new frmAdmin_LichChieu());

        private void btnSanPham_Click(object sender, EventArgs e)
            => EmbedForm(new frmAdmin_SanPham());

        private void btnUuDai_Click(object sender, EventArgs e)
            => EmbedForm(new frmAdmin_UuDai());

        private void btnStaffPos_Click(object sender, EventArgs e)
            => EmbedForm(new frmStaff_POS());

        private void btnStaffQuanLyDon_Click(object sender, EventArgs e)
            => EmbedForm(new frmStaff_QuanLyDon());

        private void btnNhanVien_Click(object sender, EventArgs e)
            => EmbedForm(new frmAdmin_NhanVien());

        private void btnDoanhThu_Click(object sender, EventArgs e)
            => EmbedForm(new frmAdmin_DoanhThu());

        private void btnQuanLyKhachHang_Click(object sender, EventArgs e)
            => EmbedForm(new frmQuanLyKhachHang());

        // ──────────────────────────────────────────────
        //  ĐĂNG XUẤT
        // ──────────────────────────────────────────────
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _timer?.Stop();
                TenNguoiDung = "";
                VaiTro = "";
                this.Close(); // frmLogin tự hiện lại qua FormClosed event (xem frmLogin.cs)
            }
        }

        // ──────────────────────────────────────────────
        //  SỰ KIỆN DESIGNER — giữ lại để tránh lỗi build
        // ──────────────────────────────────────────────
        private void label1_Click(object sender, EventArgs e) { }
        private void guna2PictureBox1_Click(object sender, EventArgs e) { }
        private void guna2Panel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
    }
}