using GPACARICOMAPI.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace GPACARICOMAPI.Services.Interfaces
{
    public class TestimonialRepository : ITestimonialRepository
    {
        private readonly IConnectionFactory _connectionFactory;
       public TestimonialRepository(IConnectionFactory DBConnection) 
            {
            _connectionFactory = DBConnection;
            }

        public async Task<bool> AddNewTestimonial(TestimonialModel testimonial)
        {
            using var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();

            string query = @"
        INSERT INTO testimonials
        (
            testimonial_user_id,
            testimonial_title,
            testimonial_body,
            username,
            created_date,
            created_by,
            is_deleted
        )
        VALUES
        (
            @userId,
            @title,
            @body,
            @username,
            @createdDate,
            @createdBy,
            @isDeleted
        );";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@userId", testimonial.testimonialUserid);
            command.Parameters.AddWithValue("@title", testimonial.testimonialTitle);
            command.Parameters.AddWithValue("@body", testimonial.testimonialbody);
            command.Parameters.AddWithValue("@username", testimonial.username);
            command.Parameters.AddWithValue("@createdDate", DateTime.Now);
            command.Parameters.AddWithValue("@createdBy", testimonial.CreatedBy);
            command.Parameters.AddWithValue("@isDeleted", false);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public Task<bool> DeleteTestimonial(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<TestimonialModel?> GetTestimonialbyId(int id)
        {
            using var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();

            string query = @"
        SELECT *
        FROM testimonials
        WHERE testimonial_id = @id
        AND is_deleted = 0
        LIMIT 1;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new TestimonialModel
                {
                    testimonialid = reader.GetInt32("testimonial_id"),
                    testimonialUserid = reader.GetInt32("testimonial_user_id"),
                    testimonialTitle = reader.GetString("testimonial_title"),
                    testimonialbody = reader.GetString("testimonial_body"),
                    username = reader.GetString("username"),

                   // CreatedDate = reader.GetDateTime("created_date"),

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



        //GETS ALL THE ARTICLES And Returns The list
        public async Task<List<TestimonialModel>> GetTestimonialsAsync()
        {
            var testimonials = new List<TestimonialModel>();

            var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();
            string query = "SELECT * \r\nFROM testimonials\r\nORDER BY created_date\r\nLIMIT 3;";
            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();
           

            while (await reader.ReadAsync()) {
                testimonials.Add(new TestimonialModel
                {
                    testimonialid = reader.GetInt32("testimonial_id"),
                    testimonialUserid = reader.GetInt32("testimonial_user_id"),
                    testimonialTitle = reader.GetString("testimonial_title"),
                    testimonialbody = reader.GetString("testimonial_body"),
                    username = reader.GetString("username"),
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
            return testimonials;
        }

        public async Task<bool> UpdateTestimonial(TestimonialModel testimonial)
        {
            using var connection = _connectionFactory.GetConnection();
            await connection.OpenAsync();

            string query = @"
        UPDATE testimonials
        SET
            testimonial_title = @title,
            testimonial_body = @body,
            username = @username,
            updated_date = @updatedDate,
            updated_by = @updatedBy
        WHERE
            testimonial_id = @id
            AND is_deleted = 0;";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", testimonial.testimonialid);
            command.Parameters.AddWithValue("@title", testimonial.testimonialTitle);
            command.Parameters.AddWithValue("@body", testimonial.testimonialbody);
            command.Parameters.AddWithValue("@username", testimonial.username);
            command.Parameters.AddWithValue("@updatedDate", DateTime.Now);
            command.Parameters.AddWithValue("@updatedBy", testimonial.UpdatedBy);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
    }
}
