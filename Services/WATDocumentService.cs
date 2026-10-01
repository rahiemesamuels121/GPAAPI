using GPACARICOMAPI.Models;
using GPACARICOMAPI.Repositories.Interface;
using GPACARICOMAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GPACARICOMAPI.Services;

public class WATDocumentService
    : IWATDocumentService
{
    private readonly IFileStorageService _fileStorage;
    private readonly IWATDocumentRepository _repository;

    public WATDocumentService(
        IFileStorageService fileStorage,
        IWATDocumentRepository repository)
    {
        _fileStorage = fileStorage;
        _repository = repository;
    }

    public async Task<FileUploadResult> UploadDocumentAsync(
        long applicationId,
        int documentTypeId,
        string documentFolder,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        if (applicationId <= 0)
        {
            throw new ArgumentException(
                "Invalid application ID.");
        }

        if (documentTypeId <= 0)
        {
            throw new ArgumentException(
                "Invalid document type.");
        }

        var existingPath =
            await _repository.GetExistingPathAsync(
                applicationId,
                documentTypeId,
                cancellationToken);

        var folder =
            Path.Combine(
                "wat",
                applicationId.ToString(),
                documentFolder);

        var upload =
            await _fileStorage.UploadAsync(
                file,
                folder,
                cancellationToken);

        try
        {
            await _repository.SaveDocumentAsync(
                applicationId,
                documentTypeId,
                upload.OriginalFileName,
                upload.RelativePath,
                upload.ContentType,
                cancellationToken);
        }
        catch
        {
            // DB failed, remove newly-created file.
            await _fileStorage.DeleteAsync(
                upload.RelativePath,
                cancellationToken);

            throw;
        }

        // Database succeeded, old file can now be removed.
        if (!string.IsNullOrWhiteSpace(existingPath) &&
            !string.Equals(
                existingPath,
                upload.RelativePath,
                StringComparison.OrdinalIgnoreCase))
        {
            await _fileStorage.DeleteAsync(
                existingPath,
                cancellationToken);
        }

        return upload;
    }
}