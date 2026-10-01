using GPACARICOMAPI.Services.Interfaces;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace GPACARICOMAPI.Services
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MySqlConnection GetConnection()
        {
            // Try standard ConnectionStrings section first, fallback to simple key
            var cs = _configuration.GetConnectionString("DefaultConnection")
                     ?? _configuration["DefaultConnection"]
                     ?? string.Empty;

            return new MySqlConnection(cs);
        }
    }
}
