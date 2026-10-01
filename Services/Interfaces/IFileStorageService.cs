using GPACARICOMAPI.Models;
using Microsoft.AspNetCore.Http;

namespace GPACARICOMAPI.Services.Interfaces;

public interface IFileStorageService
{
    Task<FileUploadResult> UploadAsync(
        IFormFile file,
        string folder,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string relativePath,
        CancellationToken cancellationToken = default);
}