using System.ComponentModel.DataAnnotations;

namespace _31241026329_TranHuuTuanBang.Extension;

public sealed class CheckDateGreaterThanTodayAttribute : ValidationAttribute
{
    public CheckDateGreaterThanTodayAttribute() : base("Ngày phát hành phải từ hôm nay trở đi.") { }

    public override bool IsValid(object value) =>
        value is DateTime date && date.Date >= DateTime.Today;
}
