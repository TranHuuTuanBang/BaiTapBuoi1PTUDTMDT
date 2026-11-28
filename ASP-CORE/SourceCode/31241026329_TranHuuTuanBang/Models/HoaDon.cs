using System.ComponentModel.DataAnnotations;

namespace _31241026329_TranHuuTuanBang.Models;

public class HoaDon
{
    [Key]
    public int MaHoaDon { get; set; }
    public Guid CheckoutToken { get; set; }
    public DateTime NgayLap { get; set; }
    public decimal TongTien { get; set; }
    public IList<ChiTietHoaDon> ChiTietHoaDons { get; set; } = [];
    public ShippingDetail ShippingDetail { get; set; }
}
