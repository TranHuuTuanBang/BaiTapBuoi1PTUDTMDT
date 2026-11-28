using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _31241026329_TranHuuTuanBang.Data;
using _31241026329_TranHuuTuanBang.Models;
using _31241026329_TranHuuTuanBang.Services;

namespace _31241026329_TranHuuTuanBang.Controllers;

public class MoviesController(ApplicationDbContext context, PosterStorage posters) : Controller
{
    public async Task<IActionResult> Index(string q, int? genre, CancellationToken cancellationToken)
    {
        var movies = context.Movies.Include(movie => movie.GenreObj).AsNoTracking().AsQueryable();
        q = q?.Trim();
        if (!string.IsNullOrWhiteSpace(q))
            movies = movies.Where(movie => movie.Title.Contains(q));
        if (genre.HasValue)
            movies = movies.Where(movie => movie.GenreID == genre.Value);

        return View(new CatalogView(
            await movies.OrderBy(movie => movie.Title).ToListAsync(cancellationToken),
            await context.Genres.AsNoTracking().OrderBy(item => item.Name).ToListAsync(cancellationToken),
            genre,
            q));
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var movie = await context.Movies.AsNoTracking().Include(item => item.GenreObj)
            .SingleOrDefaultAsync(item => item.ID == id, cancellationToken);
        return movie == null ? NotFound() : View(movie);
    }

    [Authorize(Roles = AdminSetup.Role)]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await LoadGenresAsync(0, cancellationToken);
        return View("Form", new MovieInput());
    }

    [HttpPost, Authorize(Roles = AdminSetup.Role)]
    public Task<IActionResult> Create(MovieInput input, CancellationToken cancellationToken) =>
        SaveAsync(null, input, cancellationToken);

    [Authorize(Roles = AdminSetup.Role)]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var movie = await context.Movies.FindAsync([id], cancellationToken);
        if (movie == null)
            return NotFound();

        ViewData["MovieId"] = id;
        await LoadGenresAsync(movie.GenreID, cancellationToken);
        return View("Form", new MovieInput
        {
            Title = movie.Title,
            Summary = movie.Summary,
            GenreID = movie.GenreID,
            Price = movie.Price,
            ReleaseDate = movie.ReleaseDate,
            Rated = movie.Rated,
            Poster = movie.Poster
        });
    }

    [HttpPost, Authorize(Roles = AdminSetup.Role)]
    public Task<IActionResult> Edit(int id, MovieInput input, CancellationToken cancellationToken) =>
        SaveAsync(id, input, cancellationToken);

    [Authorize(Roles = AdminSetup.Role)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var movie = await context.Movies.FindAsync([id], cancellationToken);
        return movie == null ? NotFound() : View(movie);
    }

    [HttpPost, ActionName(nameof(Delete)), Authorize(Roles = AdminSetup.Role)]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        var movie = await context.Movies.FindAsync([id], cancellationToken);
        if (movie == null)
            return NotFound();

        if (await context.ChiTietHoaDons.AnyAsync(line => line.MaMovie == id, cancellationToken))
        {
            ModelState.AddModelError("", "Phim đã có trong đơn hàng và cần được giữ lại.");
            return View("Delete", movie);
        }

        context.Remove(movie);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Chưa thể xóa phim. Vui lòng tải lại trang và thử lại.");
            return View("Delete", movie);
        }

        posters.Remove(movie.PicturePath);
        TempData["Notice"] = "Đã xóa phim.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> SaveAsync(int? id, MovieInput input, CancellationToken cancellationToken)
    {
        var movie = id.HasValue ? await context.Movies.FindAsync([id.Value], cancellationToken) : new Movie();
        if (movie == null)
            return NotFound();

        ViewData["MovieId"] = id;
        input.Poster = id.HasValue ? movie.Poster : null;
        if (id.HasValue && movie.ReleaseDate.Date == input.ReleaseDate.Date)
            ModelState.Remove(nameof(input.ReleaseDate));

        if (input.GenreID > 0 && !await context.Genres.AnyAsync(genre => genre.ID == input.GenreID, cancellationToken))
            ModelState.AddModelError(nameof(input.GenreID), "Thể loại đã chọn không tồn tại.");

        string newPoster = null;
        if (ModelState.IsValid && input.PictureUpload != null)
        {
            try
            {
                newPoster = await posters.SaveAsync(input.PictureUpload, cancellationToken);
            }
            catch (InvalidDataException error)
            {
                ModelState.AddModelError(nameof(input.PictureUpload), error.Message);
            }
        }

        if (!ModelState.IsValid)
        {
            await LoadGenresAsync(input.GenreID, cancellationToken);
            return View("Form", input);
        }

        var oldPoster = movie.PicturePath;
        movie.Title = input.Title.Trim();
        movie.Summary = input.Summary.Trim();
        movie.GenreID = input.GenreID;
        movie.Price = input.Price;
        movie.ReleaseDate = input.ReleaseDate.Date;
        movie.Rated = input.Rated;
        movie.PicturePath = newPoster ?? oldPoster;
        if (!id.HasValue)
            context.Add(movie);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            posters.Remove(newPoster);
            ModelState.AddModelError("", "Chưa lưu được phim. Vui lòng kiểm tra thể loại và thử lại.");
            await LoadGenresAsync(input.GenreID, cancellationToken);
            return View("Form", input);
        }

        if (newPoster != null)
            posters.Remove(oldPoster);
        TempData["Notice"] = id.HasValue ? "Đã cập nhật phim." : "Đã thêm phim.";
        return RedirectToAction(nameof(Details), new { id = movie.ID });
    }

    private async Task LoadGenresAsync(int selected, CancellationToken cancellationToken) =>
        ViewData["Genres"] = new SelectList(
            await context.Genres.AsNoTracking().OrderBy(item => item.Name).ToListAsync(cancellationToken),
            nameof(Genre.ID), nameof(Genre.Name), selected);
}
