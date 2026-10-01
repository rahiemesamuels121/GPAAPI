using GPACARICOMAPI.Models;
using GPACARICOMAPI.Services.Interfaces;
using MySql.Data.MySqlClient;
using System.Data;

namespace GPACARICOMAPI.Services
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public ProgramRepository(IConnectionFactory DBConnection)
        {
            _connectionFactory = DBConnection;
        }
        public async Task<bool> AddNewProgram(ProgramModel program)
        {
            using var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();

            string query = @"
        INSERT INTO gpa_programs
        (
            program_name,
            program_description,
            program_date,
            created_date,
            created_by,
            is_deleted,
            image_path
        )
        VALUES
        (
            @programName,
            @programDescription,
            @programDate,
            @createdDate,
            @createdBy,
            @isDeleted
            @imagePath
        );";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@programName", program.programName);
            command.Parameters.AddWithValue("@programDescription", program.programDescription);
            command.Parameters.AddWithValue("@programDate", program.programDate);
            command.Parameters.AddWithValue("@createdDate", DateTime.Now);
            command.Parameters.AddWithValue("@createdBy", program.CreatedBy);
            command.Parameters.AddWithValue("@isDeleted", false);
            command.Parameters.AddWithValue("@imagePath", program.ImagePath);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
     
        public async Task<bool> DeleteProgram(int id)
        {
            using var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();

            string query = @"
        UPDATE gpa_programs
        SET
            is_deleted = 1,
            deleted_date = @deletedDate,
            deleted_by = @deletedBy
        WHERE
            program_id = @id
            AND is_deleted = 0;";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@deletedDate", DateTime.Now);
            command.Parameters.AddWithValue("@deletedBy", "System"); // Replace with logged-in user later

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<ProgramModel> GetProgram(int id)
        {
            using var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();

            string query = @"
        SELECT *
        FROM gpa_programs
        WHERE program_id = @id
        AND is_deleted = 0
        LIMIT 1;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ProgramModel
                {
                    programid = reader.GetInt32("program_id"),
                    programName = reader.GetString("program_name"),
                    programDescription = reader.GetString("program_description"),
                    programDate = reader.GetString("program_date"),

                    CreatedDate = reader.GetDateTime("created_date"),

                    CreatedBy = reader.IsDBNull(reader.GetOrdinal("created_by"))
                        ? string.Empty
                        : reader.GetString("created_by"),

                    UpdatedDate = reader.IsDBNull(reader.GetOrdinal("updated_date"))
                        ? null
                        : reader.GetDateTime("updated_date"),

                    UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updated_by"))
                        ? null
                        : reader.GetString("updated_by"),

                    IsDeleted = reader.GetBoolean("is_deleted"),

                    DeletedDate = reader.IsDBNull(reader.GetOrdinal("deleted_date"))
                        ? null
                        : reader.GetDateTime("deleted_date"),

                    DeletedBy = reader.IsDBNull(reader.GetOrdinal("deleted_by"))
                        ? null
                        : reader.GetString("deleted_by")
                };
            }

            return null;
        }

        public async Task<List<ProgramModel>> GetProgramsAsync()
        {
            var programs = new List<ProgramModel>();

            var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();
            string query = @"
                    SELECT * FROM gpa_programs 
                    WHERE is_deleted = 0 
                    ORDER BY created_date;";
            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                programs.Add(new ProgramModel
                {
                    programid = reader.GetInt32("program_id"),
                    programName = reader.GetString("program_name"),
                    programDescription = reader.GetString("program_description"),
                    programDate = reader.GetString("program_date"),
                    CreatedDate = reader.GetDateTime("created_date"),
                    CreatedBy = reader.IsDBNull(reader.GetOrdinal("created_by"))
                    ? string.Empty
                    : reader.GetString("created_by"),

                    UpdatedDate = reader.IsDBNull(reader.GetOrdinal("updated_date"))
                    ? null
                    : reader.GetDateTime("updated_date"),

                    UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updated_by"))
                    ? null
                    : reader.GetString("updated_by"),

                    IsDeleted = reader.GetBoolean("is_deleted"),

                    DeletedDate = reader.IsDBNull(reader.GetOrdinal("deleted_date"))
                    ? null
                    : reader.GetDateTime("deleted_date"),

                    DeletedBy = reader.IsDBNull(reader.GetOrdinal("deleted_by"))
                    ? null
                    : reader.GetString("deleted_by")

                });
            }
            return programs;

        }

        public async Task<bool> UpdateProgram(ProgramModel program)
        {
            using var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();

            string query = @"
        UPDATE gpa_programs
        SET
            program_name = @programName,
            program_description = @programDescription,
            program_date = @programDate,
            updated_date = @updatedDate,
            updated_by = @updatedBy
        WHERE
            program_id = @id
            AND is_deleted = 0;";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", program.programid);
            command.Parameters.AddWithValue("@programName", program.programName);
            command.Parameters.AddWithValue("@programDescription", program.programDescription);
            command.Parameters.AddWithValue("@programDate", program.programDate);
            command.Parameters.AddWithValue("@updatedDate", DateTime.Now);
            command.Parameters.AddWithValue("@updatedBy", program.UpdatedBy);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
    }
}
