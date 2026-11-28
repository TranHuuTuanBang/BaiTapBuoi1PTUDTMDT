namespace _31241026329_TranHuuTuanBang.Services;

public sealed class PosterStorage(string directory, ILogger<PosterStorage> logger)
{
    public const int MaxBytes = 2 * 1024 * 1024;

    public async Task<string> SaveAsync(IFormFile upload, CancellationToken cancellationToken)
    {
        if (upload.Length is < 12 or > MaxBytes)
            throw new InvalidDataException("Ảnh phải có dung lượng từ 12 byte đến 2 MB.");

        await using var stream = upload.OpenReadStream();
        var header = new byte[12];
        await stream.ReadExactlyAsync(header.AsMemory(), cancellationToken);
        var extension = header switch
        {
            [0xFF, 0xD8, 0xFF, ..] => ".jpg",
            [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, ..] => ".png",
            [0x52, 0x49, 0x46, 0x46, _, _, _, _, 0x57, 0x45, 0x42, 0x50] => ".webp",
            _ => throw new InvalidDataException("Chọn ảnh JPG, PNG hoặc WebP hợp lệ.")
        };

        var name = $"{Guid.NewGuid():N}{extension}";
        var path = Path.Combine(directory, name);
        try
        {
            await using var file = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
            await file.WriteAsync(header.AsMemory(), cancellationToken);
            await stream.CopyToAsync(file, cancellationToken);
        }
        catch
        {
            Remove("/posters/" + name);
            throw;
        }

        return "/posters/" + name;
    }

    public void Remove(string url)
    {
        if (url == null || !url.StartsWith("/posters/", StringComparison.Ordinal))
            return;

        try
        {
            File.Delete(Path.Combine(directory, Path.GetFileName(url)));
        }
        catch (IOException error)
        {
            logger.LogWarning(error, "Không dọn được ảnh {ImageName}.", Path.GetFileName(url));
        }
        catch (UnauthorizedAccessException error)
        {
            logger.LogWarning(error, "Không có quyền dọn ảnh {ImageName}.", Path.GetFileName(url));
        }
    }
}
