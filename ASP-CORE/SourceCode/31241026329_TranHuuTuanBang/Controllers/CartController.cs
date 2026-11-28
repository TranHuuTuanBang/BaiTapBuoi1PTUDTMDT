using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using _31241026329_TranHuuTuanBang.Data;
using _31241026329_TranHuuTuanBang.Extension;
using _31241026329_TranHuuTuanBang.Models;
using _31241026329_TranHuuTuanBang.Services;

namespace _31241026329_TranHuuTuanBang.Controllers;

public class CartController(ApplicationDbContext context, EmailService email) : Controller
{
    private const int MaxQuantity = 99;

    public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
        View(await GetLinesAsync(HttpContext.Session.GetCart(), cancellationToken));

    [HttpPost]
    public async Task<IActionResult> AddToCart(int id, CancellationToken cancellationToken)
    {
        if (!await context.Movies.AnyAsync(movie => movie.ID == id, cancellationToken))
            return NotFound();

        var cart = HttpContext.Session.GetCart();
        cart[id] = Math.Min(cart.GetValueOrDefault(id) + 1, MaxQuantity);
        HttpContext.Session.SetCart(cart);
        HttpContext.Session.Remove("CheckoutToken");
        TempData["Notice"] = "Đã cập nhật phim trong giỏ.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Update(int id, int quantity)
    {
        var cart = HttpContext.Session.GetCart();
        if (ModelState.IsValid && quantity is >= 1 and <= MaxQuantity && cart.ContainsKey(id))
        {
            cart[id] = quantity;
            HttpContext.Session.SetCart(cart);
            HttpContext.Session.Remove("CheckoutToken");
        }
        else
        {
            TempData["Notice"] = "Số lượng phải từ 1 đến 99.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int id)
    {
        var cart = HttpContext.Session.GetCart();
        cart.Remove(id);
        HttpContext.Session.SetCart(cart);
        HttpContext.Session.Remove("CheckoutToken");
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Checkout(CancellationToken cancellationToken)
    {
        var lines = await GetLinesAsync(HttpContext.Session.GetCart(), cancellationToken);
        if (lines.Count == 0)
            return RedirectToAction(nameof(Index));

        if (!Guid.TryParse(HttpContext.Session.GetString("CheckoutToken"), out var token))
        {
            token = Guid.NewGuid();
            HttpContext.Session.SetString("CheckoutToken", token.ToString());
        }

        return View(new CheckoutInput { Lines = lines, Token = token });
    }

    [HttpPost, EnableRateLimiting("Checkout")]
    public async Task<IActionResult> Checkout(CheckoutInput input, CancellationToken cancellationToken)
    {
        var session = HttpContext.Session;
        if (input.Token == Guid.Empty || session.GetString("CheckoutToken") != input.Token.ToString())
        {
            TempData["Notice"] = "Giỏ hàng đã thay đổi hoặc phiên đặt hàng đã kết thúc.";
            return RedirectToAction(nameof(Index));
        }

        var cart = session.GetCart();
        input.Lines = await GetLinesAsync(cart, cancellationToken);
        if (input.Lines.Count != cart.Count)
        {
            session.SetCart(input.Lines.ToDictionary(line => line.Movie.ID, line => line.Quantity));
            session.Remove("CheckoutToken");
            TempData["Notice"] = "Có phim đã ngừng bán. Vui lòng kiểm tra giỏ hàng.";
            return RedirectToAction(nameof(Index));
        }

        if (input.Lines.Count == 0)
            return RedirectToAction(nameof(Index));

        if (input.Shipping == null)
            ModelState.AddModelError(nameof(input.Shipping), "Hãy nhập thông tin giao hàng.");
        else if (input.Shipping.DeliveryDate.Date <= DateTime.Today)
            ModelState.AddModelError("Shipping.DeliveryDate", "Ngày giao dự kiến phải sau hôm nay.");

        if (!ModelState.IsValid)
            return View(input);

        var order = new HoaDon
        {
            CheckoutToken = input.Token,
            NgayLap = DateTime.UtcNow,
            TongTien = input.Lines.Sum(line => line.Subtotal),
            ShippingDetail = new ShippingDetail
            {
                Name = input.Shipping.Name.Trim(),
                Email = input.Shipping.Email.Trim(),
                Address = input.Shipping.Address.Trim(),
                Mobile = input.Shipping.Mobile.Trim(),
                DeliveryDate = input.Shipping.DeliveryDate.Date
            },
            ChiTietHoaDons = input.Lines.Select(line => new ChiTietHoaDon
            {
                MaMovie = line.Movie.ID,
                TenPhim = line.Movie.Title,
                DonGia = line.Movie.Price,
                SoLuong = line.Quantity
            }).ToList()
        };

        context.Add(order);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            var existing = await context.HoaDons.AsNoTracking()
                .SingleOrDefaultAsync(item => item.CheckoutToken == input.Token, cancellationToken);
            if (existing != null)
                return Complete(existing.MaHoaDon);

            ModelState.AddModelError("", "Chưa lưu được đơn hàng. Vui lòng thử lại.");
            return View(input);
        }

        await email.SendConfirmationAsync(order);
        return Complete(order.MaHoaDon);
    }

    public IActionResult CheckoutCompleted()
    {
        if (TempData["OrderId"] is not int orderId)
            return RedirectToAction(nameof(Index));

        return View(orderId);
    }

    private IActionResult Complete(int orderId)
    {
        HttpContext.Session.Remove("Cart");
        HttpContext.Session.Remove("CheckoutToken");
        TempData["OrderId"] = orderId;
        return RedirectToAction(nameof(CheckoutCompleted));
    }

    private async Task<IReadOnlyList<CartLine>> GetLinesAsync(
        Dictionary<int, int> cart, CancellationToken cancellationToken)
    {
        if (cart.Count == 0)
            return [];

        var ids = cart.Keys.ToArray();
        var movies = await context.Movies.AsNoTracking().Where(movie => ids.Contains(movie.ID))
            .OrderBy(movie => movie.Title).ToListAsync(cancellationToken);
        return movies.Select(movie => new CartLine(movie, cart[movie.ID])).ToArray();
    }
}
