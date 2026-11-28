using System.Text.Json;

namespace _31241026329_TranHuuTuanBang.Extension;

public static class SessionExtensions
{
    public static Dictionary<int, int> GetCart(this ISession session)
    {
        var value = session.GetString("Cart");
        return value == null ? [] : JsonSerializer.Deserialize<Dictionary<int, int>>(value) ?? [];
    }

    public static void SetCart(this ISession session, Dictionary<int, int> items) =>
        session.SetString("Cart", JsonSerializer.Serialize(items));
}
