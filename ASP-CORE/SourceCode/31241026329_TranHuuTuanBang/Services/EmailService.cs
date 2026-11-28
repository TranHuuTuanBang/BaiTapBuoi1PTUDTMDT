using System.Net;
using System.Net.Mail;
using _31241026329_TranHuuTuanBang.Models;

namespace _31241026329_TranHuuTuanBang.Services;

public sealed class EmailService(IConfiguration config, ILogger<EmailService> logger)
{
    public async Task SendConfirmationAsync(HoaDon order)
    {
        var address = config["Email:Address"];
        var password = config["Email:AppPassword"];
        var testRecipient = config["Email:TestRecipient"];
        if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(password) ||
            !string.Equals(order.ShippingDetail.Email, testRecipient, StringComparison.OrdinalIgnoreCase))
            return;

        try
        {
            using var mail = new MailMessage(address, testRecipient)
            {
                Subject = $"Cuộn Phim · Xác nhận đơn #{order.MaHoaDon:D5}",
                Body = string.Join(Environment.NewLine, order.ChiTietHoaDons.Select(
                    line => $"{line.TenPhim} × {line.SoLuong} — {line.ThanhTien:N0} đ"))
                    + $"{Environment.NewLine}Tổng cộng {order.TongTien:N0} đ"
            };
            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(address, password)
            };
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            await client.SendMailAsync(mail, timeout.Token);
        }
        catch (Exception error) when (error is SmtpException or FormatException or OperationCanceledException)
        {
            logger.LogWarning("Gửi email đơn {OrderId} thất bại ({ErrorType}).", order.MaHoaDon, error.GetType().Name);
        }
    }
}
