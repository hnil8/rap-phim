using CinemaManagement.BLL.DTOs;
using CinemaManagement.BLL.Services;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities;
using Guna.UI2.WinForms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinemaManagement.UI.Forms
{
    public partial class frmAdmin_LichChieu : Form
    {
        // ════════════════════════════════════════════════════════════
        //  1. DEPENDENCY INJECTION & BIẾN TOÀN CỤC
        // ════════════════════════════════════════════════════════════
        private readonly IShowtimeService _showtimeService;
        private readonly IMovieService _movieService;
        private readonly CinemaDbContext _db;

        private readonly Guna2TextBox _txtGiaVe = new();
        private int _selectedId = -1;
        private List<LichChieuDto> _currentRows = new();

        public frmAdmin_LichChieu(IShowtimeService showtimeService, IMovieService movieService, CinemaDbContext db)
        {
            InitializeComponent();

            // [SỬA LỖI VỠ GIAO DIỆN TUYỆT ĐỐI]
            this.AutoScaleMode = AutoScaleMode.None; // Tắt tính năng tự động phóng to của Windows
            this.MinimumSize = this.Size;           // Chốt cứng kích thước bằng bản thiết kế

            _showtimeService = showtimeService;
            _movieService = movieService;
            _db = db;

            ConfigureGiaVeInput();
            DangKySuKien();
        }

        // ════════════════════════════════════════════════════════════
        //  2. KHỞI TẠO GIAO DIỆN & SỰ KIỆN
        // ════════════════════════════════════════════════════════════
        private void ConfigureGiaVeInput()
        {
            // Tìm tọa độ của ô Chọn Giờ để đặt ô Giá cơ bản ngay dưới nó
            // Giả sử ô Chọn Giờ của bạn tên là txtGioChieu
            int startX = txtGioChieu.Location.X;
            int startY = txtGioChieu.Location.Y + txtGioChieu.Height + 20;

            var lblGiaVe = new Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(startX - 90, startY + 5),
                Name = "labelGiaVe",
                Text = "Giá cơ bản",
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.White
            };

            _txtGiaVe.BorderRadius = 5;
            _txtGiaVe.Location = new System.Drawing.Point(startX, startY);
            _txtGiaVe.Name = "txtGiaVe";
            _txtGiaVe.Size = new System.Drawing.Size(258, 36);
            _txtGiaVe.Text = "75000";

            // Thêm vào đúng Panel chứa các ô nhập liệu (ví dụ guna2Panel1)
            guna2Panel1.Controls.Add(lblGiaVe);
            guna2Panel1.Controls.Add(_txtGiaVe);

            lblGiaVe.BringToFront();
            _txtGiaVe.BringToFront();

            // KHÔNG viết code set Location cho các nút btnThem, btnSua... ở đây nữa
            // Hãy quay lại bản Design của frmAdmin_LichChieu và kéo chúng xuống vị trí bạn muốn.
        }

        private void DangKySuKien()
        {
            Load += frmAdmin_LichChieu_Load;
        }

        private async void frmAdmin_LichChieu_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigureGrid();
                await LoadLookupDataAsync();
                ResetForm();
                await LoadGridAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi tai du lieu lich chieu: " + ex.Message, "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════════
        //  3. CẤU HÌNH GRID & COMBOBOX
        // ════════════════════════════════════════════════════════════
        private void ConfigureGrid()
        {
            dgvLichChieu.Rows.Clear();
            dgvLichChieu.AutoGenerateColumns = false;
            dgvLichChieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLichChieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLichChieu.MultiSelect = false;
            dgvLichChieu.AllowUserToAddRows = false;

            EnsureTextColumn("colId", "ID", false);
            EnsureTextColumn("colPhim", "Phim");
            EnsureTextColumn("colRap", "Phong");
            EnsureTextColumn("colNgay", "Ngay chieu");
            EnsureTextColumn("colGio", "Gio bat dau");
            EnsureTextColumn("colKetThuc", "Gio ket thuc");
            EnsureTextColumn("colGiaVe", "Gia co ban");
            EnsureTextColumn("colTrangThai", "Trang thai");

            dgvLichChieu.Columns["colCheck"].Width = 50;
            dgvLichChieu.Columns["colCheck"].ReadOnly = false;
        }

        private void EnsureTextColumn(string name, string headerText, bool visible = true)
        {
            if (!dgvLichChieu.Columns.Contains(name))
            {
                dgvLichChieu.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = name,
                    HeaderText = headerText,
                    ReadOnly = true
                });
            }

            dgvLichChieu.Columns[name].Visible = visible;
        }

        private async Task LoadLookupDataAsync()
        {
            var movies = await _movieService.GetAllAsync();
            var rooms = await _db.PhongChieus
                .AsNoTracking()
                .Where(room => !room.IsDeleted && room.TrangThai == "HoatDong")
                .OrderBy(room => room.TenPhong)
                .ToListAsync();

            var movieItems = new List<LookupItem> { new(0, "--- Tat ca phim ---") };
            movieItems.AddRange(movies.Select(movie => new LookupItem(movie.PhimId, movie.TenPhim)));

            var roomItems = new List<LookupItem> { new(0, "--- Tat ca phong ---") };
            roomItems.AddRange(rooms.Select(room => new LookupItem(room.PhongId, room.TenPhong)));

            cbChonPhim.DataSource = movieItems;
            cbChonPhim.DisplayMember = nameof(LookupItem.Name);
            cbChonPhim.ValueMember = nameof(LookupItem.Id);

            cbChonRap.DataSource = roomItems;
            cbChonRap.DisplayMember = nameof(LookupItem.Name);
            cbChonRap.ValueMember = nameof(LookupItem.Id);
        }

        private async Task LoadGridAsync()
        {
            _selectedId = -1;
            var rows = await _showtimeService.GetByNgayAsync(dtpNgayChieu.Value.Date);
            var selectedMovieId = cbChonPhim.SelectedValue as int? ?? 0;
            var selectedRoomId = cbChonRap.SelectedValue as int? ?? 0;

            if (selectedMovieId > 0)
                rows = rows.Where(row => row.PhimId == selectedMovieId).ToList();

            if (selectedRoomId > 0)
                rows = rows.Where(row => row.PhongId == selectedRoomId).ToList();

            _currentRows = rows.OrderBy(row => row.GioBatDau).ToList();

            dgvLichChieu.Rows.Clear();
            foreach (var row in _currentRows)
            {
                dgvLichChieu.Rows.Add(false,
                    row.LichChieuId,
                    row.TenPhim,
                    row.TenPhong,
                    row.NgayChieuText,
                    row.GioBatDauText,
                    row.GioKetThucText,
                    row.GiaVeCoBan.ToString("N0"),
                    row.TrangThai);
            }

            DatTrangThaiBanDau();
        }

        // ════════════════════════════════════════════════════════════
        //  4. TƯƠNG TÁC BUTTON
        // ════════════════════════════════════════════════════════════
        private async void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                await LoadGridAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi tim kiem lich chieu: " + ex.Message, "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            if (!TryBuildDto(out var dto))
                return;

            btnThem.Enabled = false;
            try
            {
                var result = await _showtimeService.CreateAsync(dto);
                MessageBox.Show(result.Message, result.IsSuccess ? "Thong bao" : "Loi", MessageBoxButtons.OK,
                    result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (result.IsSuccess)
                {
                    ResetForm();
                    await LoadGridAsync();
                }
            }
            finally
            {
                btnThem.Enabled = true;
            }
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Vui long chon lich chieu can cap nhat.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryBuildDto(out var dto))
                return;

            btnSua.Enabled = false;
            try
            {
                var result = await _showtimeService.UpdateAsync(_selectedId, dto);
                MessageBox.Show(result.Message, result.IsSuccess ? "Thong bao" : "Loi", MessageBoxButtons.OK,
                    result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (result.IsSuccess)
                {
                    ResetForm();
                    await LoadGridAsync();
                }
            }
            finally
            {
                btnSua.Enabled = true;
            }
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedId <= 0)
            {
                MessageBox.Show("Vui long chon lich chieu can huy.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Ban co chac muon huy lich chieu nay?", "Xac nhan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            var result = await _showtimeService.UpdateTrangThaiAsync(_selectedId, "HuyChieu");
            MessageBox.Show(result.Message, result.IsSuccess ? "Thong bao" : "Loi", MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (result.IsSuccess)
            {
                ResetForm();
                await LoadGridAsync();
            }
        }

        private async void btnXoaLich_Click(object sender, EventArgs e)
        {
            dgvLichChieu.CommitEdit(DataGridViewDataErrorContexts.Commit);

            var selectedIds = new List<int>();
            foreach (DataGridViewRow row in dgvLichChieu.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if (Convert.ToBoolean(row.Cells["colCheck"].Value))
                    selectedIds.Add(Convert.ToInt32(row.Cells["colId"].Value));
            }

            if (!selectedIds.Any())
            {
                MessageBox.Show("Vui long chon it nhat mot lich chieu.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Ban co chac muon huy {selectedIds.Count} lich chieu da chon?", "Xac nhan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            foreach (var id in selectedIds)
                await _showtimeService.UpdateTrangThaiAsync(id, "HuyChieu");

            ResetForm();
            await LoadGridAsync();
        }

        // ════════════════════════════════════════════════════════════
        //  5. TƯƠNG TÁC LƯỚI & XỬ LÝ LÔ-GÍC
        // ════════════════════════════════════════════════════════════
        private void dgvLichChieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _currentRows.Count)
                return;

            if (e.ColumnIndex == dgvLichChieu.Columns["colCheck"].Index)
                return;

            var item = _currentRows[e.RowIndex];
            _selectedId = item.LichChieuId;
            cbChonPhim.SelectedValue = item.PhimId;
            cbChonRap.SelectedValue = item.PhongId;
            dtpNgayChieu.Value = item.GioBatDau.Date;
            txtGioChieu.Text = item.GioBatDau.ToString("HH:mm");
            _txtGiaVe.Text = item.GiaVeCoBan.ToString("0", CultureInfo.InvariantCulture);

            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private bool TryBuildDto(out CreateLichChieuDto dto)
        {
            dto = new CreateLichChieuDto();

            if ((cbChonPhim.SelectedValue as int? ?? 0) <= 0)
            {
                MessageBox.Show("Vui long chon mot phim cu the.", "Canh bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if ((cbChonRap.SelectedValue as int? ?? 0) <= 0)
            {
                MessageBox.Show("Vui long chon mot phong chieu cu the.", "Canh bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!TimeOnly.TryParseExact(txtGioChieu.Text.Trim(), "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var gioChieu))
            {
                MessageBox.Show("Gio chieu khong hop le. Dinh dang dung la HH:mm.", "Canh bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(_txtGiaVe.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var giaVe)
                && !decimal.TryParse(_txtGiaVe.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out giaVe))
            {
                MessageBox.Show("Gia ve khong hop le.", "Canh bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (giaVe <= 0)
            {
                MessageBox.Show("Gia ve phai lon hon 0.", "Canh bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            dto = new CreateLichChieuDto
            {
                PhimId = (int)cbChonPhim.SelectedValue,
                PhongId = (int)cbChonRap.SelectedValue,
                GioBatDau = dtpNgayChieu.Value.Date.Add(gioChieu.ToTimeSpan()),
                GiaVeCoBan = giaVe
            };

            return true;
        }

        private void ResetForm()
        {
            _selectedId = -1;
            if (cbChonPhim.Items.Count > 0) cbChonPhim.SelectedIndex = 0;
            if (cbChonRap.Items.Count > 0) cbChonRap.SelectedIndex = 0;
            dtpNgayChieu.Value = DateTime.Today;
            txtGioChieu.Text = string.Empty;
            _txtGiaVe.Text = "75000";
            DatTrangThaiBanDau();
        }

        private void DatTrangThaiBanDau()
        {
            btnSua.Enabled = _selectedId > 0;
            btnXoa.Enabled = _selectedId > 0;
        }

        private sealed record LookupItem(int Id, string Name);

        // ════════════════════════════════════════════════════════════
        //  6. CÁC HÀM RỖNG CHO DESIGNER
        // ════════════════════════════════════════════════════════════
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void dgvLichChieu_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}