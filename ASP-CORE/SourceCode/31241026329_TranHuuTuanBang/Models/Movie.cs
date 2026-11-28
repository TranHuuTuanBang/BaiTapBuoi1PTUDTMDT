using System.ComponentModel.DataAnnotations;
using _31241026329_TranHuuTuanBang.Extension;

namespace _31241026329_TranHuuTuanBang.Models;

public class Movie
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int GenreID { get; set; }
    public Genre GenreObj { get; set; }
    public decimal Price { get; set; }
    public double Rated { get; set; }
    public string PicturePath { get; set; }
    public string Poster => PicturePath ?? "/images/no-image.jpg";
}

public class MovieInput
{
    [Display(Name = "Tên phim"), Required, StringLength(200, MinimumLength = 3)]
    public string Title { get; set; }
    [Display(Name = "Tóm tắt"), Required, StringLength(2000, MinimumLength = 3)]
    public string Summary { get; set; }
    [Display(Name = "Ngày phát hành"), DataType(DataType.Date), CheckDateGreaterThanToday]
    public DateTime ReleaseDate { get; set; } = DateTime.Today;
    [Display(Name = "Thể loại"), Range(1, int.MaxValue, ErrorMessage = "Hãy chọn thể loại.")]
    public int GenreID { get; set; }
    [Display(Name = "Giá (đồng)"), Range(5000, 100000000)]
    public decimal Price { get; set; }
    [Display(Name = "Xếp hạng"), Range(1, 5)]
    public double Rated { get; set; } = 4;
    [Display(Name = "Ảnh phim")]
    public IFormFile PictureUpload { get; set; }
    public string Poster { get; set; }
}
