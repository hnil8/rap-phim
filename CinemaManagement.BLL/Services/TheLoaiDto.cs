namespace CinemaManagement.BLL.DTOs;

public class TheLoaiDto
{
    public int TheLoaiId { get; set; }
    public string TenTheLoai { get; set; } = string.Empty;
}

public class CreateTheLoaiDto
{
    public string TenTheLoai { get; set; } = string.Empty;
}