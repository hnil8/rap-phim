using System;
using System.Collections.Generic;
using CinemaManagement.UI.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.UI.DAL.Context;

public partial class CinemaDbContext : DbContext
{
    public CinemaDbContext()
    {
    }

    public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CaLamViec> CaLamViecs { get; set; }

    public virtual DbSet<ChiTietCombo> ChiTietCombos { get; set; }

    public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<GheNgoi> GheNgois { get; set; }

    public virtual DbSet<HangThanhVien> HangThanhViens { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<HoaDonFnB> HoaDonFnBs { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }

    public virtual DbSet<LichChieu> LichChieus { get; set; }

    public virtual DbSet<LichChieuGhe> LichChieuGhes { get; set; }

    public virtual DbSet<LoaiGhe> LoaiGhes { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<NhomFnB> NhomFnBs { get; set; }

    public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }

    public virtual DbSet<Phim> Phims { get; set; }

    public virtual DbSet<PhongChieu> PhongChieus { get; set; }

    public virtual DbSet<QuyTacGium> QuyTacGia { get; set; }

    public virtual DbSet<SanPhamFnB> SanPhamFnBs { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TheLoaiPhim> TheLoaiPhims { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    public virtual DbSet<VePhim> VePhims { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=RapphimDB;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CaLamViec>(entity =>
        {
            entity.HasKey(e => e.CaId);

            entity.ToTable("CaLamViec");

            entity.HasIndex(e => e.NhanVienId, "IX_CaLamViec_NhanVienId");

            entity.HasIndex(e => e.TrangThai, "IX_CaLamViec_TrangThai");

            entity.Property(e => e.GhiChuChotCa).HasMaxLength(500);
            entity.Property(e => e.ThoiGianChotCa).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianMoCa)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TienDauCa).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TongThuChuyenKhoan).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TongThuThe).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TongThuTienMat).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("DangMo");

            entity.HasOne(d => d.NhanVien).WithMany(p => p.CaLamViecs)
                .HasForeignKey(d => d.NhanVienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaLamViec_NhanVien");
        });

        modelBuilder.Entity<ChiTietCombo>(entity =>
        {
            entity.ToTable("ChiTietCombo");

            entity.HasIndex(e => new { e.ComboId, e.SanPhamId }, "UQ_ChiTietCombo_Item").IsUnique();

            entity.Property(e => e.SoLuong).HasDefaultValue(1);

            entity.HasOne(d => d.Combo).WithMany(p => p.ChiTietCombos)
                .HasForeignKey(d => d.ComboId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietCombo_Combo");

            entity.HasOne(d => d.SanPham).WithMany(p => p.ChiTietCombos)
                .HasForeignKey(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChiTietCombo_SanPham");
        });

        modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
        {
            entity.HasKey(e => new { e.PhieuNhapId, e.SanPhamId }).HasName("PK__ChiTietP__8E6BB81FEE57938E");

            entity.ToTable("ChiTietPhieuNhap");

            entity.Property(e => e.GiaNhap).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.PhieuNhap).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.PhieuNhapId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__Phieu__7C1A6C5A");

            entity.HasOne(d => d.SanPham).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__SanPh__7D0E9093");
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.ToTable("Combo");

            entity.Property(e => e.GiaCombo).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.HinhAnhUrl).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.TenCombo).HasMaxLength(200);
        });

        modelBuilder.Entity<GheNgoi>(entity =>
        {
            entity.HasKey(e => e.GheId);

            entity.ToTable("GheNgoi");

            entity.HasIndex(e => e.PhongId, "IX_GheNgoi_PhongId");

            entity.HasIndex(e => new { e.PhongId, e.DayGhe, e.CotGhe }, "UQ_GheNgoi_ViTri").IsUnique();

            entity.Property(e => e.DayGhe)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TenGhe)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComputedColumnSql("([DayGhe]+CONVERT([varchar](3),[CotGhe]))", true);

            entity.HasOne(d => d.LoaiGhe).WithMany(p => p.GheNgois)
                .HasForeignKey(d => d.LoaiGheId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GheNgoi_LoaiGhe");

            entity.HasOne(d => d.Phong).WithMany(p => p.GheNgois)
                .HasForeignKey(d => d.PhongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GheNgoi_Phong");
        });

        modelBuilder.Entity<HangThanhVien>(entity =>
        {
            entity.HasKey(e => e.HangId);

            entity.ToTable("HangThanhVien");

            entity.HasIndex(e => e.TenHang, "UQ_HangThanhVien_Ten").IsUnique();

            entity.Property(e => e.MauSac).HasMaxLength(20);
            entity.Property(e => e.MoTa).HasMaxLength(300);
            entity.Property(e => e.PhanTramUuDai).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TenHang).HasMaxLength(50);
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.ToTable("HoaDon");

            entity.HasIndex(e => e.CaId, "IX_HoaDon_CaId");

            entity.HasIndex(e => e.KhachHangId, "IX_HoaDon_KhachHangId");

            entity.HasIndex(e => e.ThoiGianTao, "IX_HoaDon_ThoiGianTao");

            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.LyDoHuy).HasMaxLength(300);
            entity.Property(e => e.PhuongThucTt)
                .HasMaxLength(30)
                .HasColumnName("PhuongThucTT");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TienGiamDiem).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TienGiamKm)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("TienGiamKM");
            entity.Property(e => e.TienGiamThanhVien).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TienKhachDua).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TienThoiLai).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TongTienFnB).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TongTienGiam)
                .HasComputedColumnSql("(([TienGiamKM]+[TienGiamDiem])+[TienGiamThanhVien])", true)
                .HasColumnType("decimal(20, 0)");
            entity.Property(e => e.TongTienGoc).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TongTienVe).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("HoanThanh");

            entity.HasOne(d => d.Ca).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.CaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDon_Ca");

            entity.HasOne(d => d.KhachHang).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.KhachHangId)
                .HasConstraintName("FK_HoaDon_KhachHang");

            entity.HasOne(d => d.KhuyenMai).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.KhuyenMaiId)
                .HasConstraintName("FK_HoaDon_KhuyenMai");
        });

        modelBuilder.Entity<HoaDonFnB>(entity =>
        {
            entity.ToTable("HoaDon_FnB");

            entity.HasIndex(e => e.HoaDonId, "IX_HoaDonFnB_HoaDonId");

            entity.Property(e => e.HoaDonFnBid).HasColumnName("HoaDonFnBId");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SoLuong).HasDefaultValue(1);
            entity.Property(e => e.ThanhTien)
                .HasComputedColumnSql("([SoLuong]*[DonGia])", true)
                .HasColumnType("decimal(29, 0)");

            entity.HasOne(d => d.Combo).WithMany(p => p.HoaDonFnBs)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK_HoaDonFnB_Combo");

            entity.HasOne(d => d.HoaDon).WithMany(p => p.HoaDonFnBs)
                .HasForeignKey(d => d.HoaDonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDonFnB_HoaDon");

            entity.HasOne(d => d.SanPham).WithMany(p => p.HoaDonFnBs)
                .HasForeignKey(d => d.SanPhamId)
                .HasConstraintName("FK_HoaDonFnB_SanPham");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.ToTable("KhachHang");

            entity.HasIndex(e => e.HoTen, "IX_KhachHang_HoTen");

            entity.HasIndex(e => e.SoDienThoai, "IX_KhachHang_SoDienThoai");

            entity.HasIndex(e => e.SoDienThoai, "UQ_KhachHang_SoDT").IsUnique();

            entity.Property(e => e.AvatarUrl).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MatKhauHash).HasMaxLength(256);
            entity.Property(e => e.NgayDangKy)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);

            entity.HasOne(d => d.Hang).WithMany(p => p.KhachHangs)
                .HasForeignKey(d => d.HangId)
                .HasConstraintName("FK_KhachHang_Hang");
        });

        modelBuilder.Entity<KhuyenMai>(entity =>
        {
            entity.ToTable("KhuyenMai");

            entity.HasIndex(e => e.MaCode, "IX_KhuyenMai_MaCode");

            entity.HasIndex(e => e.MaCode, "UQ_KhuyenMai_MaCode").IsUnique();

            entity.Property(e => e.GiaTriGiam).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.GiaTriGiamToiDa).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LoaiGiam).HasMaxLength(20);
            entity.Property(e => e.MaCode).HasMaxLength(50);
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayHetHan).HasColumnType("datetime");
            entity.Property(e => e.TenChuongTrinh).HasMaxLength(200);
        });

        modelBuilder.Entity<LichChieu>(entity =>
        {
            entity.ToTable("LichChieu");

            entity.HasIndex(e => e.PhimId, "IX_LichChieu_PhimId");

            entity.HasIndex(e => new { e.PhongId, e.GioBatDau }, "IX_LichChieu_PhongId_GioBatDau");

            entity.HasIndex(e => e.TrangThai, "IX_LichChieu_TrangThai");

            entity.Property(e => e.GiaVeCoBan).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.GioBatDau).HasColumnType("datetime");
            entity.Property(e => e.GioKetThuc).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("ChuaChieu");

            entity.HasOne(d => d.Phim).WithMany(p => p.LichChieus)
                .HasForeignKey(d => d.PhimId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichChieu_Phim");

            entity.HasOne(d => d.Phong).WithMany(p => p.LichChieus)
                .HasForeignKey(d => d.PhongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichChieu_Phong");
        });

        modelBuilder.Entity<LichChieuGhe>(entity =>
        {
            entity.ToTable("LichChieu_Ghe");

            entity.HasIndex(e => e.LichChieuId, "IX_LichChieuGhe_LichChieuId");

            entity.HasIndex(e => new { e.LichChieuId, e.TrangThaiGhe }, "IX_LichChieuGhe_TrangThai");

            entity.HasIndex(e => new { e.LichChieuId, e.GheId }, "UQ_LichChieu_Ghe_ViTri").IsUnique();

            entity.Property(e => e.ThoiGianGiu).HasColumnType("datetime");
            entity.Property(e => e.TrangThaiGhe)
                .HasMaxLength(20)
                .HasDefaultValue("Trong");

            entity.HasOne(d => d.Ghe).WithMany(p => p.LichChieuGhes)
                .HasForeignKey(d => d.GheId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichChieuGhe_Ghe");

            entity.HasOne(d => d.LichChieu).WithMany(p => p.LichChieuGhes)
                .HasForeignKey(d => d.LichChieuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichChieuGhe_LichChieu");

            entity.HasOne(d => d.VePhim).WithMany(p => p.LichChieuGhes)
                .HasForeignKey(d => d.VePhimId)
                .HasConstraintName("FK_LichChieuGhe_VePhim");
        });

        modelBuilder.Entity<LoaiGhe>(entity =>
        {
            entity.ToTable("LoaiGhe");

            entity.HasIndex(e => e.TenLoai, "UQ_LoaiGhe_Ten").IsUnique();

            entity.Property(e => e.HeSoGia)
                .HasDefaultValue(1.00m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.MoTa).HasMaxLength(200);
            entity.Property(e => e.TenLoai).HasMaxLength(50);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.HoTen, "IX_NhanVien_HoTen");

            entity.HasIndex(e => e.SoDienThoai, "IX_NhanVien_SoDienThoai");

            entity.Property(e => e.DiaChi).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.NgayVaoLam).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.NgayXoa).HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
        });

        modelBuilder.Entity<NhomFnB>(entity =>
        {
            entity.HasKey(e => e.NhomId);

            entity.ToTable("NhomFnB");

            entity.HasIndex(e => e.TenNhom, "UQ_NhomFnB_Ten").IsUnique();

            entity.Property(e => e.BieuTuong).HasMaxLength(100);
            entity.Property(e => e.TenNhom).HasMaxLength(100);
        });

        modelBuilder.Entity<PhieuNhap>(entity =>
        {
            entity.HasKey(e => e.PhieuNhapId).HasName("PK__PhieuNha__DE3A38E2292C1766");

            entity.ToTable("PhieuNhap");

            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.NgayNhap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NguoiNhap).HasMaxLength(100);
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Phim>(entity =>
        {
            entity.ToTable("Phim");

            entity.HasIndex(e => e.TenPhim, "IX_Phim_TenPhim");

            entity.HasIndex(e => e.TrangThai, "IX_Phim_TrangThai");

            entity.Property(e => e.DaoDien).HasMaxLength(200);
            entity.Property(e => e.DienVienChinh).HasMaxLength(500);
            entity.Property(e => e.GioiHanDoTuoi)
                .HasMaxLength(10)
                .HasDefaultValue("P");
            entity.Property(e => e.MoTa).HasMaxLength(2000);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgonNgu)
                .HasMaxLength(50)
                .HasDefaultValue("VietSub");
            entity.Property(e => e.NuocSanXuat).HasMaxLength(100);
            entity.Property(e => e.PosterUrl).HasMaxLength(500);
            entity.Property(e => e.TenGoc).HasMaxLength(200);
            entity.Property(e => e.TenPhim).HasMaxLength(200);
            entity.Property(e => e.TrailerUrl).HasMaxLength(500);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("SapChieu");

            entity.HasMany(d => d.TheLoais).WithMany(p => p.Phims)
                .UsingEntity<Dictionary<string, object>>(
                    "PhimTheLoai",
                    r => r.HasOne<TheLoaiPhim>().WithMany()
                        .HasForeignKey("TheLoaiId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PhimTheLoai_TheLoai"),
                    l => l.HasOne<Phim>().WithMany()
                        .HasForeignKey("PhimId")
                        .HasConstraintName("FK_PhimTheLoai_Phim"),
                    j =>
                    {
                        j.HasKey("PhimId", "TheLoaiId");
                        j.ToTable("Phim_TheLoai");
                        j.HasIndex(new[] { "TheLoaiId" }, "IX_PhimTheLoai_TheLoaiId");
                    });
        });

        modelBuilder.Entity<PhongChieu>(entity =>
        {
            entity.HasKey(e => e.PhongId);

            entity.ToTable("PhongChieu");

            entity.HasIndex(e => e.TenPhong, "UQ_PhongChieu_Ten").IsUnique();

            entity.Property(e => e.LoaiPhong)
                .HasMaxLength(50)
                .HasDefaultValue("2D");
            entity.Property(e => e.TenPhong).HasMaxLength(100);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("HoatDong");
        });

        modelBuilder.Entity<QuyTacGium>(entity =>
        {
            entity.HasKey(e => e.QuyTacId);

            entity.Property(e => e.DoiTuong)
                .HasMaxLength(20)
                .HasDefaultValue("TatCa");
            entity.Property(e => e.GiaCoBan).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.KhungGio)
                .HasMaxLength(20)
                .HasDefaultValue("TatCa");
            entity.Property(e => e.LoaiNgay)
                .HasMaxLength(20)
                .HasDefaultValue("TatCa");
            entity.Property(e => e.TenQuyTac).HasMaxLength(100);
        });

        modelBuilder.Entity<SanPhamFnB>(entity =>
        {
            entity.HasKey(e => e.SanPhamId);

            entity.ToTable("SanPhamFnB");

            entity.HasIndex(e => e.NhomId, "IX_SanPhamFnB_NhomId");

            entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.HinhAnhUrl).HasMaxLength(500);
            entity.Property(e => e.MoTa).HasMaxLength(500);
            entity.Property(e => e.TenSanPham).HasMaxLength(200);

            entity.HasOne(d => d.Nhom).WithMany(p => p.SanPhamFnBs)
                .HasForeignKey(d => d.NhomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SanPhamFnB_Nhom");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.VaiTroId, "IX_TaiKhoan_VaiTroId");

            entity.HasIndex(e => e.NhanVienId, "UQ_TaiKhoan_NhanVienId").IsUnique();

            entity.HasIndex(e => e.TenDangNhap, "UQ_TaiKhoan_TenDangNhap").IsUnique();

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LanDangNhapCuoi).HasColumnType("datetime");
            entity.Property(e => e.MatKhauHash).HasMaxLength(256);
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);

            entity.HasOne(d => d.NhanVien).WithOne(p => p.TaiKhoan)
                .HasForeignKey<TaiKhoan>(d => d.NhanVienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaiKhoan_NhanVien");

            entity.HasOne(d => d.VaiTro).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.VaiTroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaiKhoan_VaiTro");
        });

        modelBuilder.Entity<TheLoaiPhim>(entity =>
        {
            entity.HasKey(e => e.TheLoaiId);

            entity.ToTable("TheLoaiPhim");

            entity.HasIndex(e => e.TenTheLoai, "UQ_TheLoaiPhim_Ten").IsUnique();

            entity.Property(e => e.TenTheLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.ToTable("VaiTro");

            entity.HasIndex(e => e.TenVaiTro, "UQ_VaiTro_TenVaiTro").IsUnique();

            entity.Property(e => e.MoTa).HasMaxLength(200);
            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
        });

        modelBuilder.Entity<VePhim>(entity =>
        {
            entity.HasKey(e => e.VeId);

            entity.ToTable("VePhim");

            entity.HasIndex(e => e.HoaDonId, "IX_VePhim_HoaDonId");

            entity.HasIndex(e => e.LichChieuGheId, "IX_VePhim_LichChieuGheId");

            entity.HasIndex(e => e.LichChieuGheId, "UQ_VePhim_GheSuat").IsUnique();

            entity.Property(e => e.DoiTuongKhach)
                .HasMaxLength(20)
                .HasDefaultValue("NguoiLon");
            entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.GiaGoc).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MaVach).HasMaxLength(100);
            entity.Property(e => e.ThoiGianIn).HasColumnType("datetime");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("DaBan");

            entity.HasOne(d => d.HoaDon).WithMany(p => p.VePhims)
                .HasForeignKey(d => d.HoaDonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VePhim_HoaDon");

            entity.HasOne(d => d.LichChieuGhe).WithOne(p => p.VePhimNavigation)
                .HasForeignKey<VePhim>(d => d.LichChieuGheId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VePhim_LichChieuGhe");

            entity.HasOne(d => d.QuyTac).WithMany(p => p.VePhims)
                .HasForeignKey(d => d.QuyTacId)
                .HasConstraintName("FK_VePhim_QuyTacGia");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
