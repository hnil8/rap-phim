using System;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_WebPreview : Form
    {
        // Bạn có thể đổi đường link này thành đường link Localhost (đồ án web của bạn)
        private readonly string DEFAULT_URL = "https://www.cgv.vn/";

        public frmAdmin_WebPreview()
        {
            InitializeComponent();

            // Đăng ký sự kiện
            this.Load += FrmAdmin_WebPreview_Load;
            this.btnDiToi.Click += BtnDiToi_Click;
            this.btnQuayLai.Click += BtnQuayLai_Click;
            this.btnTienLen.Click += BtnTienLen_Click;
            this.btnTaiLai.Click += BtnTaiLai_Click;

            // Cập nhật lại thanh địa chỉ mỗi khi chuyển trang xong
            this.webBrowser.Navigated += WebBrowser_Navigated;

            // Cho phép nhấn Enter để truy cập
            this.txtUrl.KeyDown += TxtUrl_KeyDown;
        }

        private void FrmAdmin_WebPreview_Load(object sender, EventArgs e)
        {
            // Vừa mở form lên là tự động truy cập vào trang web mặc định
            txtUrl.Text = DEFAULT_URL;
            TruyCapWebsite(DEFAULT_URL);
        }

        // ════════════════════════════════════════════════════════════
        // XỬ LÝ ĐIỀU HƯỚNG CƠ BẢN
        // ════════════════════════════════════════════════════════════
        private void TruyCapWebsite(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;

            // Kiểm tra xem có giao thức http chưa, nếu chưa thì tự động thêm vào
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "https://" + url;
            }

            try
            {
                webBrowser.Navigate(new Uri(url));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Địa chỉ web không hợp lệ!\nChi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDiToi_Click(object sender, EventArgs e)
        {
            TruyCapWebsite(txtUrl.Text.Trim());
        }

        private void TxtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TruyCapWebsite(txtUrl.Text.Trim());
                e.Handled = true; // Chặn tiếng "beep" của Windows khi ấn Enter
                e.SuppressKeyPress = true;
            }
        }

        private void BtnQuayLai_Click(object sender, EventArgs e)
        {
            if (webBrowser.CanGoBack)
            {
                webBrowser.GoBack();
            }
        }

        private void BtnTienLen_Click(object sender, EventArgs e)
        {
            if (webBrowser.CanGoForward)
            {
                webBrowser.GoForward();
            }
        }

        private void BtnTaiLai_Click(object sender, EventArgs e)
        {
            webBrowser.Refresh();
        }

        // ════════════════════════════════════════════════════════════
        // CẬP NHẬT UI KHI TẢI XONG TRANG
        // ════════════════════════════════════════════════════════════
        private void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // Cập nhật lại đường dẫn lên TextBox (đề phòng web bị redirect sang link khác)
            txtUrl.Text = webBrowser.Url.ToString();
        }
    }
}