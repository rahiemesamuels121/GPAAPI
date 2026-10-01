using MySql.Data.MySqlClient;

namespace GPACARICOMAPI.Services.Interfaces
{
    
        public interface IConnectionFactory
        {
            MySqlConnection GetConnection();
        }
    
}
