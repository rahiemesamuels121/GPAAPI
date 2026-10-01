using GPACARICOMAPI.Repositories.Interface;
using GPACARICOMAPI.Services.Interfaces;
using MySql.Data.MySqlClient;

namespace GPACARICOMAPI.Repositories;

public class WATDocumentRepository
    : IWATDocumentRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public WATDocumentRepository(
        IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> SaveDocumentAsync(
        long applicationId,
        int documentTypeId,
        string fileName,
        string filePath,
        string mimeType,
        CancellationToken cancellationToken = default)
    {
        await using var connection =
            _connectionFactory.GetConnection();

        await connection.OpenAsync(
            cancellationToken);

        const string sql = """
            INSERT INTO wat_application_documents
            (
                application_id,
                document_type_id,
                document_status_id,
                file_name,
                file_path,
                mime_type,
                uploaded_at
            )
            VALUES
            (
                @ApplicationId,
                @DocumentTypeId,
                2,
                @FileName,
                @FilePath,
                @MimeType,
                CURRENT_TIMESTAMP
            )
            ON DUPLICATE KEY UPDATE
                document_status_id = 2,
                file_name = VALUES(file_name),
                file_path = VALUES(file_path),
                mime_type = VALUES(mime_type),
                uploaded_at = CURRENT_TIMESTAMP,
                reviewed_at = NULL,
                review_notes = NULL;

            SELECT application_document_id
            FROM wat_application_documents
            WHERE application_id = @ApplicationId
              AND document_type_id = @DocumentTypeId;
            """;

        await using var command =
            new MySqlCommand(
                sql,
                connection);

        command.Parameters.AddWithValue(
            "@ApplicationId",
            applicationId);

        command.Parameters.AddWithValue(
            "@DocumentTypeId",
            documentTypeId);

        command.Parameters.AddWithValue(
            "@FileName",
            fileName);

        command.Parameters.AddWithValue(
            "@FilePath",
            filePath);

        command.Parameters.AddWithValue(
            "@MimeType",
            mimeType);

        var result =
            await command.ExecuteScalarAsync(
                cancellationToken);

        return Convert.ToInt64(result);
    }

    public async Task<string?> GetExistingPathAsync(
        long applicationId,
        int documentTypeId,
        CancellationToken cancellationToken = default)
    {
        await using var connection =
            _connectionFactory.GetConnection();

        await connection.OpenAsync(
            cancellationToken);

        const string sql = """
            SELECT file_path
            FROM wat_application_documents
            WHERE application_id = @ApplicationId
              AND document_type_id = @DocumentTypeId
            LIMIT 1;
            """;

        await using var command =
            new MySqlCommand(
                sql,
                connection);

        command.Parameters.AddWithValue(
            "@ApplicationId",
            applicationId);

        command.Parameters.AddWithValue(
            "@DocumentTypeId",
            documentTypeId);

        var result =
            await command.ExecuteScalarAsync(
                cancellationToken);

        if (result is null ||
            result == DBNull.Value)
        {
            return null;
        }

        return result.ToString();
    }
}