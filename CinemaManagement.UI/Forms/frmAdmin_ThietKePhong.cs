using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities; // Namespace DB của bạn
using CinemaManagement.UI.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_ThietKePhong : Form
    {
        // Sử dụng Dependency Injection thay vì new()
        private readonly CinemaDbContext _dbContext;

        // Tiêm DbContext vào qua Constructor
        public frmAdmin_ThietKePhong(CinemaDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;

            this.Load += FrmAdmin_ThietKePhong_Load;
            btnTaoKhung.Click += BtnTaoKhung_Click;
            btnLuuSoDo.Click += BtnLuuSoDo_Click;
        }

        private void FrmAdmin_ThietKePhong_Load(object sender, EventArgs e)
        {
            try
            {
                var danhSachPhong = _dbContext.PhongChieus.Where(p => p.IsDeleted == false).ToList();
                cboPhong.DataSource = danhSachPhong;
                cboPhong.DisplayMember = "TenPhong";
                cboPhong.ValueMember = "PhongId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chưa kết nối được Database.\nLỗi: " + ex.Message);
            }
        }

        private void BtnTaoKhung_Click(object sender, EventArgs e)
        {
            if (cboPhong.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phòng chiếu trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soHang = int.Parse(txtSoHang.Text);
            int soCot = int.Parse(txtSoCot.Text);

            flpSoDoGhe.Controls.Clear();
            flpSoDoGhe.Width = soCot * 60 + 40;

            char[] danhSachHang = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            for (int i = 0; i < soHang; i++)
            {
                string dayGhe = danhSachHang[i].ToString();

                for (int j = 1; j <= soCot; j++)
                {
                    ucSeat gheUI = new ucSeat();
                    string tenGhe = dayGhe + j;

                    // Sử dụng KieuGhe.Thuong
                    gheUI.ThietLapGhe(0, tenGhe, KieuGhe.Thuong, 0, TrangThaiGhe.Trong);
                    gheUI.Tag = new { Day = dayGhe, Cot = j };
                    gheUI.GheDuocClick += BietDoiToMau_Click;

                    flpSoDoGhe.Controls.Add(gheUI);
                }
            }
        }

        private void BietDoiToMau_Click(object sender, EventArgs e)
        {
            ucSeat gheBiClick = sender as ucSeat;
            if (gheBiClick == null) return;

            if (rdoThuong.Checked)
            {
                gheBiClick.Kieu = KieuGhe.Thuong; // Đổi Loai thành Kieu
                gheBiClick.TrangThai = TrangThaiGhe.Trong;
            }
            else if (rdoVIP.Checked)
            {
                gheBiClick.Kieu = KieuGhe.VIP;
                gheBiClick.TrangThai = TrangThaiGhe.Trong;
            }
            else if (rdoCouple.Checked)
            {
                gheBiClick.Kieu = KieuGhe.Couple;
                gheBiClick.TrangThai = TrangThaiGhe.Trong;
            }
            else if (rdoLoiDi.Checked)
            {
                gheBiClick.TrangThai = TrangThaiGhe.BaoTri;
            }
        }

        private void BtnLuuSoDo_Click(object sender, EventArgs e)
        {
            if (flpSoDoGhe.Controls.Count == 0)
            {
                MessageBox.Show("Vui lòng tạo khung và thiết kế sơ đồ trước khi lưu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int phongId = (int)cboPhong.SelectedValue;

            try
            {
                var gheCu = _dbContext.GheNgois.Where(g => g.PhongId == phongId).ToList();
                if (gheCu.Any())
                {
                    _dbContext.GheNgois.RemoveRange(gheCu);
                    _dbContext.SaveChanges();
                }

                List<GheNgoi> danhSachGheMoi = new List<GheNgoi>();

                foreach (Control control in flpSoDoGhe.Controls)
                {
                    if (control is ucSeat gheUI)
                    {
                        dynamic toaDo = gheUI.Tag;

                        int loaiGheId = 1;
                        if (gheUI.Kieu == KieuGhe.VIP) loaiGheId = 2; // Đổi Loai thành Kieu
                        if (gheUI.Kieu == KieuGhe.Couple) loaiGheId = 3;

                        bool laLoiDi = (gheUI.TrangThai == TrangThaiGhe.BaoTri);

                        GheNgoi gheMoi = new GheNgoi
                        {
                            PhongId = phongId,
                            LoaiGheId = loaiGheId,
                            DayGhe = toaDo.Day.ToString(),
                            CotGhe = (int)toaDo.Cot,
                            TenGhe = gheUI.TenGhe,
                            IsDeleted = laLoiDi
                        };

                        danhSachGheMoi.Add(gheMoi);
                    }
                }

                _dbContext.GheNgois.AddRange(danhSachGheMoi);
                _dbContext.SaveChanges();

                MessageBox.Show($"Đã lưu thành công sơ đồ gồm {danhSachGheMoi.Count} vị trí cho phòng này!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi trong quá trình lưu: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}