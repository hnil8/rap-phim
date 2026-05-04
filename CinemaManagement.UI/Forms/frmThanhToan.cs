using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmThanhToan : Form
    {
        private decimal _tongTienCanThanhToan;

        // Constructor mặc định (Để Designer không báo lỗi)
        public frmThanhToan()
        {
            InitializeComponent();
        }

        // Constructor nhận số tiền từ Form POS truyền sang
        public frmThanhToan(decimal tongTien)
        {
            InitializeComponent();

            // Ép form luôn hiện ở giữa màn hình
            this.StartPosition = FormStartPosition.CenterParent;

            _tongTienCanThanhToan = tongTien;

            // Cập nhật số tiền lên Label (Định dạng có dấu chấm ngàn)
            lblTongTien.Text = $"CẦN THANH TOÁN: {_tongTienCanThanhToan.ToString("N0")} VNĐ";
        }

        // ════════════════════════════════════════════════════════════
        // HÀM XỬ LÝ THANH TOÁN CHUNG
        // ════════════════════════════════════════════════════════════
        private void XuLyThanhToan(string phuongThuc)
        {
            // Ở dự án thực tế, chỗ này sẽ gọi API tạo mã QR Momo/VNPay hoặc kết nối máy POS.
            // Trong khuôn khổ phần mềm quản lý, ta hiện thông báo xác nhận:
            DialogResult rs = MessageBox.Show(
                $"Thu ngân xác nhận khách hàng thanh toán {_tongTienCanThanhToan.ToString("N0")} VNĐ \nBằng hình thức: {phuongThuc}?",
                "Xác nhận thanh toán",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (rs == DialogResult.Yes)
            {
                // Truyền tín hiệu "OK" về lại cho frmStaff_POS biết là đã thu tiền xong
                this.DialogResult = DialogResult.OK;
                this.Close(); // Tự động đóng cửa sổ này lại
            }
        }

        // ════════════════════════════════════════════════════════════
        // BẮT CÁC SỰ KIỆN NÚT BẤM (Đã đăng ký trong Designer)
        // ════════════════════════════════════════════════════════════

        private void btnTienMat_Click(object sender, EventArgs e)
        {
            XuLyThanhToan("Tiền mặt");
        }

        private void btnMomo_Click(object sender, EventArgs e)
        {
            XuLyThanhToan("Ví điện tử Momo");
        }

        private void btnVNPay_Click(object sender, EventArgs e)
        {
            XuLyThanhToan("Cổng thanh toán VNPay");
        }

        private void btnQuetThe_Click(object sender, EventArgs e)
        {
            XuLyThanhToan("Quẹt thẻ ngân hàng (POS)");
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            // Báo cho frmStaff_POS biết là giao dịch bị hủy, không làm sạch giỏ hàng
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Hàm rỗng chống lỗi Designer khi lỡ tay nhấp đúp vào Label
        private void lblTongTien_Click(object sender, EventArgs e) { }
    }
}