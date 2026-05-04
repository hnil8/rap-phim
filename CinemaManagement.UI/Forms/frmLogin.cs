using System;
using System.Drawing;
using System.Windows.Forms;
using CinemaManagement.BLL;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaManagement.UI.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();

            // [UX]: Đăng ký sự kiện nhấn phím Enter để Đăng nhập nhanh
            txtTenDangNhap.KeyDown += TxtInput_KeyDown;
            txtPassword.KeyDown += TxtInput_KeyDown;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            // Focus sẵn vào ô nhập tài khoản khi vừa mở phần mềm
            txtTenDangNhap.Focus();
        }

        // ════════════════════════════════════════════════════════════
        //  1. UX: NHẤN ENTER ĐỂ ĐĂNG NHẬP
        // ════════════════════════════════════════════════════════════
        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Chặn tiếng 'bíp' mặc định của Windows
                btnLogin_Click(sender, e);
            }
        }

        // ════════════════════════════════════════════════════════════
        //  2. XỬ LÝ ĐĂNG NHẬP (N-TIER & DEPENDENCY INJECTION)
        // ════════════════════════════════════════════════════════════
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtPassword.Text;

            // Validate nhanh tại giao diện
            if (string.IsNullOrEmpty(tenDangNhap))
            {
                MessageBox.Show("Vui lòng nhập Email hoặc Tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }
            if (string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            btnLogin.Enabled = false; // Chặn click đúp

            try
            {
                // Gọi tầng BLL thông qua DI Container thay vì new() trực tiếp
                var bll = Program.Services.GetRequiredService<TaiKhoanBLL>();

                // Giả định BLL trả về một Tuple gồm (Thông tin tài khoản, Thông báo lỗi nếu có)
                (var taiKhoan, var thongBaoLoi) = bll.DangNhap(tenDangNhap, matKhau);

                if (taiKhoan != null)
                {
                    // Đăng nhập thành công -> Lưu Session vào biến static của frmMain
                    frmMain.TenNguoiDung = taiKhoan.NhanVien?.HoTen ?? tenDangNhap;
                    frmMain.VaiTro = taiKhoan.VaiTro?.TenVaiTro ?? "";

                    MoFrmMain();
                }
                else
                {
                    MessageBox.Show(thongBaoLoi, "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Clear();
                    txtPassword.Focus(); // Yêu cầu nhập lại mật khẩu
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối hệ thống!\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        private void MoFrmMain()
        {
            // Khởi tạo frmMain thông qua DI Container để tự động tiêm các Service bên trong nó
            var main = Program.Services.GetRequiredService<frmMain>();
            main.Show();
            this.Hide();

            // Lắng nghe sự kiện Form Main đóng (Tức là Đăng xuất) -> Mở lại Login
            main.FormClosed += (s, e) =>
            {
                txtTenDangNhap.Clear();
                txtPassword.Clear();
                this.Show();
                txtTenDangNhap.Focus();
            };
        }

        // ════════════════════════════════════════════════════════════
        //  3. UX: KÉO THẢ FORM (Dành cho Form ẩn viền - Borderless)
        // ════════════════════════════════════════════════════════════
        private bool _dragging;
        private Point _dragStart;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                _dragging = true;
                _dragStart = e.Location;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_dragging)
            {
                var screen = PointToScreen(e.Location);
                Location = new Point(screen.X - _dragStart.X, screen.Y - _dragStart.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _dragging = false;
        }

        // ════════════════════════════════════════════════════════════
        //  4. CÁC SỰ KIỆN RỖNG CỦA DESIGNER
        // ════════════════════════════════════════════════════════════
        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            // Sự kiện này do Designer sinh ra, giữ nguyên để không bị lỗi Build
        }
    }
}