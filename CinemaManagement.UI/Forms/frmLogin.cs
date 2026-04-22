// FILE: frmLogin.cs  —  Tầng UI

using System;
using System.Windows.Forms;
using CinemaManagement.BLL;             // TaiKhoanBLL
using CinemaManagement.DAL.db;          // TaiKhoanDAL  ← namespace thực tế
using CinemaManagement.DAL.Context;     // CinemaDbContext
using Microsoft.Extensions.DependencyInjection;

namespace CinemaManagement.UI.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        // Designer đã gán Load += frmLogin_Load
        private void frmLogin_Load(object sender, EventArgs e)
        {
            // KHÔNG gán btnLogin.Click ở đây — Designer đã gán sẵn → tránh double-fire
            txtEmail.Focus();
        }

        // Designer đã gán txtPassword.TextChanged += guna2TextBox2_TextChanged
        private void guna2TextBox2_TextChanged(object sender, EventArgs e) { }

        // Designer đã gán btnLogin.Click += btnLogin_Click
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtEmail.Text.Trim();
            string matKhau = txtPassword.Text;

            // Validate nhanh tại UI trước khi gọi BLL
            if (string.IsNullOrEmpty(tenDangNhap))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }
            if (string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                // Lấy DbContext từ DI container, tạo DAL → BLL
                var ctx = Program.Services.GetRequiredService<CinemaDbContext>();
                var dal = new TaiKhoanDAL(ctx);
                var bll = new TaiKhoanBLL(dal);

                (var taiKhoan, var thongBaoLoi) = bll.DangNhap(tenDangNhap, matKhau);

                if (taiKhoan != null)
                {
                    frmMain.TenNguoiDung = taiKhoan.NhanVien?.HoTen ?? tenDangNhap;
                    frmMain.VaiTro = taiKhoan.VaiTro?.TenVaiTro ?? "";
                    MoFrmMain();
                }
                else
                {
                    MessageBox.Show(thongBaoLoi, "Đăng nhập thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối hệ thống!\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MoFrmMain()
        {
            var main = new frmMain();
            main.Show();
            this.Hide();

            main.FormClosed += (s, e) =>
            {
                txtEmail.Clear();
                txtPassword.Clear();
                txtEmail.Focus();
                this.Show();
            };
        }

        // Kéo form (FormBorderStyle = None)
        private bool _dragging;
        private System.Drawing.Point _dragStart;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left) { _dragging = true; _dragStart = e.Location; }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_dragging)
            {
                var screen = PointToScreen(e.Location);
                Location = new System.Drawing.Point(screen.X - _dragStart.X, screen.Y - _dragStart.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _dragging = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}