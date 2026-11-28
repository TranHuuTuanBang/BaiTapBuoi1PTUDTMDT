using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _31241026329_TranHuuTuanBang.Data;
using _31241026329_TranHuuTuanBang.Models;
using _31241026329_TranHuuTuanBang.Services;

namespace _31241026329_TranHuuTuanBang.Controllers;

public class GenresController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
        View(await context.Genres.AsNoTracking().Include(item => item.Movies)
            .OrderBy(item => item.Name).ToListAsync(cancellationToken));

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var genre = await context.Genres.AsNoTracking().Include(item => item.Movies)
            .SingleOrDefaultAsync(item => item.ID == id, cancellationToken);
        return genre == null ? NotFound() : View(genre);
    }

    [Authorize(Roles = AdminSetup.Role)]
    public IActionResult Create() => View("Form", new Genre());

    [HttpPost, Authorize(Roles = AdminSetup.Role)]
    public Task<IActionResult> Create([Bind("Name")] Genre input, CancellationToken cancellationToken) =>
        SaveAsync(null, input, cancellationToken);

    [Authorize(Roles = AdminSetup.Role)]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var genre = await context.Genres.FindAsync([id], cancellationToken);
        return genre == null ? NotFound() : View("Form", genre);
    }

    [HttpPost, Authorize(Roles = AdminSetup.Role)]
    public Task<IActionResult> Edit(int id, [Bind("Name")] Genre input, CancellationToken cancellationToken) =>
        SaveAsync(id, input, cancellationToken);

    [HttpPost, Authorize(Roles = AdminSetup.Role)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var genre = await context.Genres.FindAsync([id], cancellationToken);
        if (genre == null)
            return NotFound();

        context.Remove(genre);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            TempData["Notice"] = "Đã xóa thể loại.";
        }
        catch (DbUpdateException)
        {
            TempData["Notice"] = "Chưa xóa được thể loại. Hãy chuyển phim sang thể loại khác trước.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> SaveAsync(int? id, Genre input, CancellationToken cancellationToken)
    {
        var genre = id.HasValue ? await context.Genres.FindAsync([id.Value], cancellationToken) : new Genre();
        if (genre == null)
            return NotFound();

        input.ID = id.GetValueOrDefault();
        if (!ModelState.IsValid)
            return View("Form", input);

        genre.Name = input.Name.Trim();
        if (!id.HasValue)
            context.Add(genre);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(nameof(input.Name), "Chưa lưu được. Hãy kiểm tra tên thể loại đã tồn tại chưa.");
            return View("Form", input);
        }
        TempData["Notice"] = id.HasValue ? "Đã cập nhật thể loại." : "Đã thêm thể loại.";
        return RedirectToAction(nameof(Index));
    }
}
