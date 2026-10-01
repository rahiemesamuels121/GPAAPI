namespace GPACARICOMAPI.Repositories.Interface;

public interface IWATDocumentRepository
{
    Task<long> SaveDocumentAsync(
        long applicationId,
        int documentTypeId,
        string fileName,
        string filePath,
        string mimeType,
        CancellationToken cancellationToken = default);

    Task<string?> GetExistingPathAsync(
        long applicationId,
        int documentTypeId,
        CancellationToken cancellationToken = default);
}