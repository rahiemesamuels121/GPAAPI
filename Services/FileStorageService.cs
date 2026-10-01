using GPACARICOMAPI.Configuration;
using GPACARICOMAPI.Models;
using GPACARICOMAPI.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace GPACARICOMAPI.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly FileStorageOptions _options;

    private static readonly string[] AllowedExtensions =
    [
        ".pdf",
        ".jpg",
        ".jpeg",
        ".png",
        ".doc",
        ".docx"
    ];

    public FileStorageService(
        IWebHostEnvironment environment,
        IOptions<FileStorageOptions> options)
    {
        _environment = environment;
        _options = options.Value;
    }

    public async Task<FileUploadResult> UploadAsync(
        IFormFile file,
        string folder,
        CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
        {
            throw new ArgumentException(
                "A file is required.");
        }

        var maxSize =
            _options.MaxFileSizeMB *
            1024L *
            1024L;

        if (file.Length > maxSize)
        {
            throw new ArgumentException(
                $"The file cannot exceed {_options.MaxFileSizeMB} MB.");
        }

        var extension =
            Path.GetExtension(file.FileName)
                .ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
        {
            throw new ArgumentException(
                "The selected file type is not allowed.");
        }

        var safeFolder =
            folder
                .Replace("..", string.Empty)
                .Trim('/', '\\');

        var rootPath =
            Path.Combine(
                _environment.ContentRootPath,
                _options.RootPath);

        var targetFolder =
            Path.Combine(
                rootPath,
                safeFolder);

        Directory.CreateDirectory(targetFolder);

        var storedFileName =
            $"{Guid.NewGuid():N}{extension}";

        var physicalPath =
            Path.Combine(
                targetFolder,
                storedFileName);

        await using var stream =
            new FileStream(
                physicalPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                81920,
                useAsync: true);

        await file.CopyToAsync(
            stream,
            cancellationToken);

        var relativePath =
            Path.Combine(
                    _options.RootPath,
                    safeFolder,
                    storedFileName)
                .Replace("\\", "/");

        return new FileUploadResult
        {
            OriginalFileName = file.FileName,
            StoredFileName = storedFileName,
            RelativePath = relativePath,
            ContentType = file.ContentType,
            FileSize = file.Length
        };
    }

    public Task DeleteAsync(
        string relativePath,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return Task.CompletedTask;
        }

        var normalizedPath =
            relativePath.Replace("/", Path.DirectorySeparatorChar.ToString());

        var physicalPath =
            Path.Combine(
                _environment.ContentRootPath,
                normalizedPath);

        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }

        return Task.CompletedTask;
    }
}