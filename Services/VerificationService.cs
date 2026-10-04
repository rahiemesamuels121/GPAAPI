using GPACARICOMAPI.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Configuration;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GPACARICOMAPI.Services
{
    public class VerificationService : IVerificationService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionFactory _connectionFactory;

        public VerificationService(IConfiguration configuration, IConnectionFactory connectionFactory) {
            _configuration = configuration;
            _connectionFactory = connectionFactory;
        }

        public String CreateToken()
        {
            var baseUrl = _configuration.GetSection("AppSettings:BaseUrl");
            Guid userToken = Guid.NewGuid();
            return userToken.ToString();
        }

        //Generates the confirmation link to be sent to the user for email verification
       


public string GenerateConfirmationLink(string token, string email)
    {
        var baseUrl = _configuration["AppSettings:BaseUrl"];

        var url = $"https://{baseUrl}/Auth/verify-email";

        return QueryHelpers.AddQueryString(
            url,
            new Dictionary<string, string?>
            {
                ["token"] = token,
                ["email"] = email
            });
    }


    //saves the token to the database for later verification
    public async Task<bool> SaveToken(string user_id, string token ) {

            using var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();

            var query = @"INSERT INTO email_verification_tokens 
                (
                user_id,
                token,
                created_on,
                expires_on
                )
                VALUES
                (
                @user_id,
                @token,
                @created_on,
                @expires_on
                );";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@user_id",user_id);
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@created_on", DateTime.Now);
            cmd.Parameters.AddWithValue("@expires_on", DateTime.Now.AddHours(1));
            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;

        }


        //verifies the token sent by the user against the token stored in the database
        public async Task<bool> VerifyToken(string token, string email)
        {
            using var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var query = @"
            SELECT 
                u.id,
                e.token
            FROM users u
            INNER JOIN email_verification_tokens e
                ON u.id = e.user_id
            WHERE u.email = @email
              AND u.is_verified = 0
            LIMIT 1;
        ";

                long userId;

                using (var command = new MySqlCommand(query, connection, transaction))
                {
                    command.Parameters.AddWithValue("@email", email);

                    using var reader = await command.ExecuteReaderAsync();

                    if (!await reader.ReadAsync())
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    var tokenFromDb = reader["token"]?.ToString();

                    if (string.IsNullOrWhiteSpace(tokenFromDb))
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    if (!string.Equals(
                            token,
                            tokenFromDb,
                            StringComparison.Ordinal))
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    userId = Convert.ToInt64(reader["id"]);
                }

                // Mark user as verified
                var updateQuery = @"
            UPDATE users
            SET is_verified = 1
            WHERE id = @userId
              AND is_verified = 0;
        ";

                using (var updateCommand =
                       new MySqlCommand(updateQuery, connection, transaction))
                {
                    updateCommand.Parameters.AddWithValue("@userId", userId);

                    var rowsAffected =
                        await updateCommand.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }

                // Remove verification token
                var deleteQuery = @"
            DELETE FROM email_verification_tokens
            WHERE user_id = @userId;
        ";

                using (var deleteCommand =
                       new MySqlCommand(deleteQuery, connection, transaction))
                {
                    deleteCommand.Parameters.AddWithValue("@userId", userId);

                    await deleteCommand.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


    }
}
