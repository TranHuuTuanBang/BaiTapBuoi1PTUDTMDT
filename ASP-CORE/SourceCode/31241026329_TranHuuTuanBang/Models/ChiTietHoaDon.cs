using System.ComponentModel.DataAnnotations;

namespace _31241026329_TranHuuTuanBang.Models;

public class ChiTietHoaDon
{
    [Key]
    public int MaChiTietHoaDon { get; set; }
    public int MaHoaDon { get; set; }
    public HoaDon HoaDonObj { get; set; }
    public int MaMovie { get; set; }
    public Movie MovieObj { get; set; }
    public string TenPhim { get; set; }
    public int SoLuong { get; set; }
    public decimal DonGia { get; set; }
    public decimal ThanhTien => DonGia * SoLuong;
}
