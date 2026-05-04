using CinemaManagement.BLL.DTOs;
using CinemaManagement.BLL.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_Phim : Form
    {
        // ════════════════════════════════════════════════════════════
        //  1. DEPENDENCY INJECTION & BIẾN STATE
        // ════════════════════════════════════════════════════════════
        private readonly IMovieService _movieService;
        private string _selectedPosterPath = string.Empty;
        private int _selectedMaPhim = -1;

        private bool _isFirstTimeLoad = true;
        private bool _isLoading = false;

        public frmAdmin_Phim(IMovieService movieService)
        {
            InitializeComponent();
            _movieService = movieService;

            this.Load += frmAdmin_Phim_Load;
            dgvPhim.CellClick += dgvPhim_CellClick;
            tsmiSuaPhim.Click += tsmiSuaPhim_Click;
            tsmiXoaPhim.Click += tsmiXoaPhim_Click;
            btnTheLoai.Click += btnTheLoai_Click;
        }

        // ════════════════════════════════════════════════════════════
        //  2. SỰ KIỆN LOAD
        // ════════════════════════════════════════════════════════════
        private async void frmAdmin_Phim_Load(object sender, EventArgs e)
        {
            if (!_isFirstTimeLoad) return;
            _isFirstTimeLoad = false;
            _isLoading = true;

            try
            {
                dgvPhim.DefaultCellStyle.ForeColor = Color.Black;
                dgvPhim.DefaultCellStyle.SelectionForeColor = Color.Black;

                NapDuLieuMacDinhComboBox();
                await LoadDanhSachTheLoaiAsync();
                await LoadDuLieuPhimAsync();
                XoaTrangForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo form: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void NapDuLieuMacDinhComboBox()
        {
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Đang chiếu", "Sắp chiếu", "Ngừng chiếu" });
            cboTrangThai.SelectedIndex = 0;

            txtNgonNgu.Items.Clear();
            txtNgonNgu.Items.AddRange(new object[] { "Tiếng Việt", "Tiếng Anh", "Tiếng Hàn", "Tiếng Nhật", "Khác" });
            txtNgonNgu.SelectedIndex = 0;

            txtGioiHanTuoi.Items.Clear();
            txtGioiHanTuoi.Items.AddRange(new object[] { "P - Mọi lứa tuổi", "K - Dưới 13 tuổi", "T13 - 13+", "T16 - 16+", "T18 - 18+", "C - Cấm chiếu" });
            txtGioiHanTuoi.SelectedIndex = 0;
        }

        private async Task LoadDanhSachTheLoaiAsync()
        {
            flpTheLoai.Controls.Clear();
            var danhSach = await _movieService.GetAllTheLoaisAsync();

            foreach (var tl in danhSach)
            {
                Guna.UI2.WinForms.Guna2CheckBox chk = new Guna.UI2.WinForms.Guna2CheckBox
                {
                    Text = tl.TenTheLoai,
                    Tag = tl.TheLoaiId,
                    AutoSize = true,
                    Margin = new Padding(5),
                    ForeColor = Color.Black
                };
                flpTheLoai.Controls.Add(chk);
            }
        }

        private async Task LoadDuLieuPhimAsync(string keyword = "")
        {
            try
            {
                var dsPhim = await _movieService.GetAllAsync(keyword, null);
                dgvPhim.DataSource = dsPhim;

                if (dgvPhim.Columns.Contains("PhimId"))
                {
                    dgvPhim.Columns["PhimId"].HeaderText = "Mã Phim";
                    dgvPhim.Columns["TenPhim"].HeaderText = "Tên Phim";
                    dgvPhim.Columns["ThoiLuongPhut"].HeaderText = "Thời Lượng";
                    dgvPhim.Columns["NamPhatHanh"].HeaderText = "Năm";

                    if (dgvPhim.Columns.Contains("PosterUrl")) dgvPhim.Columns["PosterUrl"].Visible = false;
                    if (dgvPhim.Columns.Contains("MoTa")) dgvPhim.Columns["MoTa"].Visible = false;
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải danh sách phim: " + ex.Message); }
        }

        // ════════════════════════════════════════════════════════════
        //  3. TẠO CỬA SỔ NHẬP NHANH THỂ LOẠI (NÚT +)
        // ════════════════════════════════════════════════════════════
        private async Task<bool> PromptAndCreateNewGenreAsync()
        {
            Form promptForm = new Form()
            {
                Width = 350,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Thêm nhanh Thể Loại",
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.Salmon,
                ForeColor = Color.White,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblTitle = new Label() { Left = 20, Top = 20, Width = 300, Text = "Nhập tên thể loại phim mới:", Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            TextBox txtInput = new TextBox() { Left = 20, Top = 50, Width = 290, Font = new Font("Segoe UI", 10) };

            Button btnConfirm = new Button() { Text = "Lưu", Left = 110, Top = 90, Width = 90, BackColor = Color.Maroon, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.OK };
            Button btnCancel = new Button() { Text = "Hủy", Left = 220, Top = 90, Width = 90, BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel };

            btnConfirm.FlatAppearance.BorderSize = 0;
            btnCancel.FlatAppearance.BorderSize = 0;

            promptForm.Controls.Add(lblTitle);
            promptForm.Controls.Add(txtInput);
            promptForm.Controls.Add(btnConfirm);
            promptForm.Controls.Add(btnCancel);

            promptForm.AcceptButton = btnConfirm;
            promptForm.CancelButton = btnCancel;

            if (promptForm.ShowDialog() == DialogResult.OK)
            {
                string genreName = txtInput.Text.Trim();

                if (string.IsNullOrWhiteSpace(genreName))
                {
                    MessageBox.Show("Tên thể loại không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                var theLoaiService = Program.Services.GetRequiredService<ITheLoaiService>();
                var createDto = new CreateTheLoaiDto { TenTheLoai = genreName };

                var result = await theLoaiService.CreateAsync(createDto);

                if (result.IsSuccess)
                {
                    MessageBox.Show($"Đã thêm thể loại '{genreName}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Lỗi: " + result.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return false;
        }

        private async void btnTheLoai_Click(object sender, EventArgs e)
        {
            bool isCreatedSuccess = await PromptAndCreateNewGenreAsync();
            if (isCreatedSuccess)
            {
                await LoadDanhSachTheLoaiAsync();
            }
        }

        // ════════════════════════════════════════════════════════════
        //  4. CHỨC NĂNG CRUD PHIM
        // ════════════════════════════════════════════════════════════
        private void XoaTrangForm()
        {
            _selectedMaPhim = -1;
            _selectedPosterPath = string.Empty;
            tctTenPhim.Clear();
            txtDaoDien.Clear();
            tctDianVien.Clear();
            txtMoTa.Clear();
            txtNamPhatHanh.Clear();
            txtThoiLuong.Clear();
            picPoster.Image = null;
            dtpNgayKhoiChieu.Value = DateTime.Now;

            foreach (Control ctrl in flpTheLoai.Controls)
            {
                if (ctrl is Guna.UI2.WinForms.Guna2CheckBox chk) chk.Checked = false;
            }
        }

        private List<int> LayMaTheLoaiDangChon()
        {
            return flpTheLoai.Controls.OfType<Guna.UI2.WinForms.Guna2CheckBox>()
                .Where(c => c.Checked)
                .Select(c => (int)c.Tag).ToList();
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tctTenPhim.Text)) { MessageBox.Show("Vui lòng nhập tên phim!"); return; }

            try
            {
                var dto = new CreatePhimDto
                {
                    TenPhim = tctTenPhim.Text.Trim(),
                    DaoDien = txtDaoDien.Text.Trim(),
                    DienVienChinh = tctDianVien.Text.Trim(),
                    MoTa = txtMoTa.Text.Trim(),
                    NamPhatHanh = int.TryParse(txtNamPhatHanh.Text, out int nam) ? nam : DateTime.Now.Year,
                    ThoiLuongPhut = int.TryParse(txtThoiLuong.Text, out int tl) ? tl : 0,
                    NgayKhoiChieu = DateOnly.FromDateTime(dtpNgayKhoiChieu.Value),
                    NgonNgu = txtNgonNgu.Text,
                    GioiHanDoTuoi = txtGioiHanTuoi.Text.Split('-')[0].Trim(), // <-- ĐÃ ĐỔI TÊN THÀNH GioiHanTuoi
                    TrangThai = cboTrangThai.Text == "Đang chiếu" ? "DangChieu" : (cboTrangThai.Text == "Ngừng chiếu" ? "NgungChieu" : "SapChieu"),
                    TheLoaiIds = LayMaTheLoaiDangChon(),
                    PosterUrl = _selectedPosterPath
                };

                var result = await _movieService.CreateAsync(dto);
                if (result.IsSuccess)
                {
                    MessageBox.Show("Thêm phim thành công!");
                    await LoadDuLieuPhimAsync();
                    XoaTrangForm();
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedMaPhim == -1) { MessageBox.Show("Vui lòng chọn phim cần sửa!"); return; }

            try
            {
                var dto = new CreatePhimDto
                {
                    TenPhim = tctTenPhim.Text.Trim(),
                    DaoDien = txtDaoDien.Text.Trim(),
                    DienVienChinh = tctDianVien.Text.Trim(),
                    MoTa = txtMoTa.Text.Trim(),
                    NamPhatHanh = int.Parse(txtNamPhatHanh.Text),
                    ThoiLuongPhut = int.Parse(txtThoiLuong.Text),
                    NgayKhoiChieu = DateOnly.FromDateTime(dtpNgayKhoiChieu.Value),
                    NgonNgu = txtNgonNgu.Text,
                    GioiHanDoTuoi = txtGioiHanTuoi.Text.Split('-')[0].Trim(), // <-- ĐÃ ĐỔI TÊN THÀNH GioiHanTuoi
                    TrangThai = cboTrangThai.Text == "Đang chiếu" ? "DangChieu" : (cboTrangThai.Text == "Ngừng chiếu" ? "NgungChieu" : "SapChieu"),
                    TheLoaiIds = LayMaTheLoaiDangChon(),
                    PosterUrl = _selectedPosterPath
                };

                var result = await _movieService.UpdateAsync(_selectedMaPhim, dto);
                if (result.IsSuccess)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    await LoadDuLieuPhimAsync();
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedMaPhim == -1) return;
            if (MessageBox.Show("Xác nhận xóa phim này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var result = await _movieService.SoftDeleteAsync(_selectedMaPhim);
                if (result.IsSuccess)
                {
                    MessageBox.Show("Đã xóa phim.");
                    await LoadDuLieuPhimAsync();
                    XoaTrangForm();
                }
            }
        }

        private async void btnLamMoi_Click(object sender, EventArgs e)
        {
            XoaTrangForm();
            await LoadDuLieuPhimAsync();
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _selectedPosterPath = ofd.FileName;
                picPoster.Image = Image.FromFile(_selectedPosterPath);
                picPoster.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        // ════════════════════════════════════════════════════════════
        //  5. TƯƠNG TÁC LƯỚI & TÌM KIẾM
        // ════════════════════════════════════════════════════════════
        private async void dgvPhim_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvPhim.Rows[e.RowIndex];
            _selectedMaPhim = Convert.ToInt32(row.Cells["PhimId"].Value);

            var phim = await _movieService.GetByIdAsync(_selectedMaPhim);
            if (phim != null)
            {
                tctTenPhim.Text = phim.TenPhim;
                txtDaoDien.Text = phim.DaoDien;
                tctDianVien.Text = phim.DienVienChinh;
                txtMoTa.Text = phim.MoTa;
                txtNamPhatHanh.Text = phim.NamPhatHanh.ToString();
                txtThoiLuong.Text = phim.ThoiLuongPhut.ToString();
                dtpNgayKhoiChieu.Value = phim.NgayKhoiChieu.HasValue ? phim.NgayKhoiChieu.Value.ToDateTime(TimeOnly.MinValue) : DateTime.Now;

                foreach (Guna.UI2.WinForms.Guna2CheckBox chk in flpTheLoai.Controls.OfType<Guna.UI2.WinForms.Guna2CheckBox>())
                {
                    chk.Checked = phim.TheLoais.Contains(chk.Text);
                }

                _selectedPosterPath = phim.PosterUrl;
                if (!string.IsNullOrEmpty(_selectedPosterPath) && File.Exists(_selectedPosterPath))
                    picPoster.Image = Image.FromFile(_selectedPosterPath);
                else picPoster.Image = null;
            }
        }

        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            await LoadDuLieuPhimAsync(txtTimLiem.Text.Trim());
        }

        private void tsmiSuaPhim_Click(object sender, EventArgs e) => btnSua_Click(sender, e);
        private void tsmiXoaPhim_Click(object sender, EventArgs e) => btnXoa_Click(sender, e);

        // CÁC HÀM RỖNG ĐỂ TRÁNH LỖI DESIGNER
        private void dgvPhim_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void guna2HtmlLabel12_Click(object sender, EventArgs e) { }
    }
}