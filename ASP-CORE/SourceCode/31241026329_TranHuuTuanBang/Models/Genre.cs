using System.ComponentModel.DataAnnotations;

namespace _31241026329_TranHuuTuanBang.Models;

public class Genre
{
    public int ID { get; set; }
    [Display(Name = "Tên thể loại"), Required, StringLength(100)]
    public string Name { get; set; }
    public IList<Movie> Movies { get; set; } = [];
}
