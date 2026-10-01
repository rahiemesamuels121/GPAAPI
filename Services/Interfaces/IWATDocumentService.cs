using GPACARICOMAPI.Models;
using Microsoft.AspNetCore.Http;

namespace GPACARICOMAPI.Services.Interfaces;

public interface IWATDocumentService
{
    Task<FileUploadResult> UploadDocumentAsync(
        long applicationId,
        int documentTypeId,
        string documentFolder,
        IFormFile file,
        CancellationToken cancellationToken = default);
}