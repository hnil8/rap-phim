using System;
using System.Drawing;
using System.Windows.Forms;

namespace CinemaManagement.UI.UserControls
{
    // Đổi tên thành KieuGhe để không đụng với bảng LoaiGhe trong Database
    public enum TrangThaiGhe { Trong, DangChon, DaBan, BaoTri }
    public enum KieuGhe { Thuong, VIP, Couple }

    public partial class ucSeat : UserControl
    {
        public int MaGhe { get; set; }
        public string TenGhe { get; set; }
        public decimal GiaGhe { get; set; }
        public KieuGhe Kieu { get; set; } // Đã đổi thành KieuGhe

        private TrangThaiGhe _trangThai;
        public TrangThaiGhe TrangThai
        {
            get => _trangThai;
            set
            {
                _trangThai = value;
                CapNhatMauSac();
            }
        }

        public event EventHandler GheDuocClick;

        public ucSeat()
        {
            InitializeComponent();
            // Nếu báo lỗi đỏ ở btnSeat, hãy chắc chắn bạn đã đặt tên nút là btnSeat trong Designer
            btnSeat.Click += BtnSeat_Click;
        }

        public void ThietLapGhe(int maGhe, string tenGhe, KieuGhe kieu, decimal giaGhe, TrangThaiGhe trangThai)
        {
            this.MaGhe = maGhe;
            this.TenGhe = tenGhe;
            this.Kieu = kieu;
            this.GiaGhe = giaGhe;
            this.TrangThai = trangThai;
            btnSeat.Text = tenGhe;
        }

        private void CapNhatMauSac()
        {
            if (TrangThai == TrangThaiGhe.DaBan || TrangThai == TrangThaiGhe.BaoTri)
            {
                btnSeat.FillColor = Color.DarkGray;
                btnSeat.BorderColor = Color.DimGray;
                btnSeat.ForeColor = Color.White;
                btnSeat.Enabled = false;
                return;
            }

            btnSeat.Enabled = true;

            if (TrangThai == TrangThaiGhe.DangChon)
            {
                btnSeat.FillColor = Color.Teal;
                btnSeat.BorderColor = Color.DarkCyan;
                btnSeat.ForeColor = Color.White;
            }
            else if (TrangThai == TrangThaiGhe.Trong)
            {
                if (Kieu == KieuGhe.VIP)
                {
                    btnSeat.FillColor = Color.Orange;
                    btnSeat.BorderColor = Color.DarkOrange;
                    btnSeat.ForeColor = Color.White;
                }
                else if (Kieu == KieuGhe.Couple)
                {
                    btnSeat.FillColor = Color.HotPink;
                    btnSeat.BorderColor = Color.DeepPink;
                    btnSeat.ForeColor = Color.White;
                }
                else
                {
                    btnSeat.FillColor = Color.White;
                    btnSeat.BorderColor = Color.Gray;
                    btnSeat.ForeColor = Color.Black;
                }
            }
        }

        private void BtnSeat_Click(object sender, EventArgs e)
        {
            if (TrangThai == TrangThaiGhe.Trong)
                TrangThai = TrangThaiGhe.DangChon;
            else if (TrangThai == TrangThaiGhe.DangChon)
                TrangThai = TrangThaiGhe.Trong;

            GheDuocClick?.Invoke(this, EventArgs.Empty);
        }
    }
}