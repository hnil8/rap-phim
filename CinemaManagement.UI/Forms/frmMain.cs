using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaManagement.UI.Forms
{
    public partial class frmMain : Form
    {
        // ════════════════════════════════════════════════════════════
        //  1. BIẾN TOÀN CỤC (SESSION) VÀ DEPENDENCY INJECTION
        // ════════════════════════════════════════════════════════════
        public static string TenNguoiDung { get; set; } = "Admin";
        public static string VaiTro { get; set; } = "Admin";

        private readonly IServiceProvider _serviceProvider;
        private System.Windows.Forms.Timer _timer;

        private Form _currentEmbeddedForm;

        public frmMain(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            SetupTimer();
        }

        // ════════════════════════════════════════════════════════════
        //  2. SỰ KIỆN LOAD VÀ PHÂN QUYỀN GIAO DIỆN
        // ════════════════════════════════════════════════════════════
        private void frmMain_Load(object sender, EventArgs e)
        {
            lblTenNguoiDung.Text = TenNguoiDung;
            lblVaiTro.Text = VaiTro;

            ApplyRolePermissions();
            LoadHomeContent();
        }

        private void SetupTimer()
        {
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += (s, e) =>
                lblThoiGian.Text = DateTime.Now.ToString("HH:mm:ss  dd/MM/yyyy");
            _timer.Start();
        }

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
            btnfrmAdmin_Combo.Visible = isAdmin;
            btnfrmAdmin_NhapKhoFnB.Visible = isAdmin;

            // Đã thêm phân quyền hiển thị nút Ca Làm Việc cho Admin
            btnCaLamViec.Visible = isAdmin;

            btnStaffPos.Visible = true;
            btnStaffQuanLyDon.Visible = true;
        }

        // ════════════════════════════════════════════════════════════
        //  3. CƠ CHẾ NHÚNG FORM CON VÀO PANEL DESKTOP 
        // ════════════════════════════════════════════════════════════
        private void EmbedForm(Form form, string customTitle = "")
        {
            if (_currentEmbeddedForm != null && !_currentEmbeddedForm.IsDisposed)
            {
                _currentEmbeddedForm.Close();
                _currentEmbeddedForm.Dispose();
            }

            pnlDesktop.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;

            form.Dock = DockStyle.None;
            form.Location = new Point(0, 0);

            pnlDesktop.AutoScroll = true;

            pnlDesktop.Controls.Add(form);
            form.Show();
            _currentEmbeddedForm = form;

            label1.Text = string.IsNullOrEmpty(customTitle) ? form.Text.ToUpper() : customTitle.ToUpper();
        }

        private void LoadHomeContent()
        {
            pnlDesktop.Controls.Clear();
            var lbl = new Label
            {
                Text = $"HỆ THỐNG QUẢN LÝ RẠP PHIM\nCHÀO MỪNG {TenNguoiDung.ToUpper()}!",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            pnlDesktop.Controls.Add(lbl);
            label1.Text = "TRANG CHỦ";
        }

        // ════════════════════════════════════════════════════════════
        //  4. CÁC SỰ KIỆN CLICK MENU 
        // ════════════════════════════════════════════════════════════
        private void btnMain_Click(object sender, EventArgs e) => LoadHomeContent();

        private void btnWeb_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_WebPreview>(), "WebPreview");
        private void btnPhim_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_Phim>(), "QUẢN LÝ PHIM");
        private void btnLichChieu_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_LichChieu>(), "QUẢN LÝ LỊCH CHIẾU ");
        private void btnSanPham_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_SanPham>(), "QUẢN LÝ SẢN PHẨM ");
        private void btnUuDai_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_UuDai>(), "QUẢN LÝ ƯU ĐÃI");
        private void btnStaffPos_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmStaff_POS>(), "QUẢN LÝ POS");
        private void btnStaffQuanLyDon_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmStaff_QuanLyDon>(), "QUẢN LÝ ĐƠN HÀNG");
        private void btnNhanVien_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_NhanVien>(), "QUẢN LÝ NHÂN VIÊN");
        private void btnDoanhThu_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_DoanhThu>(), "QUẢN LÝ DOANH THU");
        private void btnQuanLyKhachHang_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmQuanLyKhachHang>(), "QUẢN LÝ KHÁCH HÀNG");

        private void btnfrmAdmin_Combo_Click_1(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_Combo>(), "QUẢN LÝ COMBO SẢN PHẨM");
        private void btnfrmAdmin_NhapKhoFnB_Click_1(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_NhapKhoFnB>(), "QUẢN LÝ KHO HÀNG");
        private void btnFrmAdmin_TheLoai_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_TheLoai>(), "QUẢN LÝ THỂ LOẠI PHIM");

        private void btnThietKePhong_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_ThietKePhong>(), "THIÊT KẾ PHÒNG");

        // Đã cập nhật sự kiện Click cho nút Ca Làm Việc (Sử dụng ServiceProvider)
        private void btnCaLamViec_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_CaLamViec>(), "QUẢN LÝ CA LÀM VIỆC");
        private void btnPhong_Click(object sender, EventArgs e) => EmbedForm(_serviceProvider.GetRequiredService<frmAdmin_QuanLyPhong>(), "QUẢN LÝ PHÒNG CHIẾU");
        // ════════════════════════════════════════════════════════════
        //  5. ĐĂNG XUẤT
        // ════════════════════════════════════════════════════════════
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _timer?.Stop();
                TenNguoiDung = "";
                VaiTro = "";
                this.Close();
            }
        }

       
       
    }
}