using System.ComponentModel.DataAnnotations;

namespace _31241026329_TranHuuTuanBang.Models;

public class ShippingDetail
{
    public int ID { get; set; }
    public int HoaDonID { get; set; }
    [Display(Name = "Họ tên người nhận"), Required, StringLength(120)]
    public string Name { get; set; }
    [Display(Name = "Địa chỉ giao hàng"), Required, StringLength(300)]
    public string Address { get; set; }
    [Display(Name = "Email"), Required, EmailAddress, StringLength(254)]
    public string Email { get; set; }
    [Display(Name = "Số điện thoại"), Required, Phone, StringLength(30)]
    public string Mobile { get; set; }
    [Display(Name = "Ngày giao dự kiến"), DataType(DataType.Date)]
    public DateTime DeliveryDate { get; set; } = DateTime.Today.AddDays(1);
}

public class CheckoutInput
{
    public Guid Token { get; set; }
    public ShippingDetail Shipping { get; set; } = new();
    public IReadOnlyList<CartLine> Lines { get; set; } = [];
}

public record CartLine(Movie Movie, int Quantity)
{
    public decimal Subtotal => Movie.Price * Quantity;
}

public record CatalogView(IReadOnlyList<Movie> Movies, IReadOnlyList<Genre> Genres, int? Genre, string Query);
