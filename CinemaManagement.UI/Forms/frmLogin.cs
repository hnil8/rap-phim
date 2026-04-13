using System;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        // ── Sửa lỗi CS0103: 'frmLogin_Load' ──
        private void frmLogin_Load(object sender, EventArgs e)
        {
            // Khởi tạo khi form mở (để trống hoặc thêm logic sau)
        }

        // ── Sửa lỗi CS0103: 'guna2TextBox2_TextChanged' ──
        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            // Xử lý khi nội dung txtPassword thay đổi (để trống)
        }

        // ── Nút ĐĂNG NHẬP ──
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập Email hoặc Tài khoản!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập Mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // TODO: Thay bằng truy vấn database thực tế
            if (email == "admin@cinema.com" && password == "123456")
            {
                MessageBox.Show("Đăng nhập thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mở form chính:
                // var main = new frmMain();
                // main.Show();
                // this.Hide();
            }
            else
            {
                MessageBox.Show("Email hoặc Mật khẩu không đúng!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        // ── Kéo form (vì FormBorderStyle = None) ──
        private bool _dragging;
        private System.Drawing.Point _dragStart;

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
                Location = new System.Drawing.Point(
                    screen.X - _dragStart.X,
                    screen.Y - _dragStart.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _dragging = false;
        }
    }
}