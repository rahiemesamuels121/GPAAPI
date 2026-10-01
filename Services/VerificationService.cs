using GPACARICOMAPI.Services.Interfaces;
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
        public string GenerateConfirmationLink(string token, string email )
        {
            var baseUrl = _configuration.GetSection("AppSettings:BaseUrl").Value;
            string confirmationLink = $"{baseUrl}/Auth/verify-email/?token={token}?emaul={email}";
            return confirmationLink;
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
        public async Task<bool> VerifyToken(string token, string email) {

            using var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();

            var query = @"SELECT 
            u.id,
            u.firstname,
            u.lastname,
            u.email,
            e.token
            FROM users u
            INNER JOIN email_verification_tokens e
            ON u.id = a.user_id
            WHERE u.email = @email
            AND u.is_verified = 0;";
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@userId", email);
            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
               var tokenFromDb = reader.GetString("token");
                if (tokenFromDb == null)
                {
                    return false;
                }
                return token == tokenFromDb;
            }
            return false;
        }

    }
}
