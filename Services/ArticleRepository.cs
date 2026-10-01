using GPACARICOMAPI.Services.Interfaces;
using GPACARICOMAPI.Models;
using MySql.Data.MySqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using GPACARICOMAPI.Models.DTO.GPACARICOMAPI.Models;


namespace GPACARICOMAPI.Services.Interfaces
{
    public class ArticleRepository : IArticleRepository
    {

        public ArticleRepository(IConnectionFactory dbconn)
        {
            _dbconn = dbconn;
        }
        private readonly IConnectionFactory _dbconn;


        public async Task<bool> AddArticle(ArticleDTO article)
        {
            using var connection = _dbconn.GetConnection();
            await connection.OpenAsync();

            const string query = @"
        INSERT INTO articles
        (
            name,
            title,
            short_description,
            full_description,
            hyperlink,
            created_date,
            created_by,
            is_deleted
        )
        VALUES
        (
            @name,
            @title,
            @shortDescription,
            @fullDescription,
            @hyperlink,
            @createdDate,
            @createdBy,
            @isDeleted
        );";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@name", article.name);
            command.Parameters.AddWithValue("@title", article.title);
            command.Parameters.AddWithValue("@shortDescription", article.shortDescription);
            command.Parameters.AddWithValue("@fullDescription", article.fullDescription);
            command.Parameters.AddWithValue("@hyperlink", article.hyperlink);
            command.Parameters.AddWithValue("@createdDate", DateTime.Now);
            command.Parameters.AddWithValue("@createdBy", article.CreatedBy);
            command.Parameters.AddWithValue("@isDeleted", false);

            int rows = await command.ExecuteNonQueryAsync();

            return rows > 0;
        }


        //DELETE ARTICLE
        public async Task<bool> DeleteArticle(int id)
        {
            using var connection = _dbconn.GetConnection();
            await connection.OpenAsync();

            const string query = @"
        UPDATE articles
        SET
            is_deleted = 1,
            deleted_date = @deletedDate,
            deleted_by = @deletedBy
        WHERE article_id = @id;";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@deletedDate", DateTime.Now);
            command.Parameters.AddWithValue("@deletedBy", "System"); // Replace with logged-in username

            int rows = await command.ExecuteNonQueryAsync();

            return rows > 0;
        }

        public async Task<Article?> GetSingleArticle(int id)
        {
            using var connection = _dbconn.GetConnection();
            await connection.OpenAsync();

            string query = @"
        SELECT *
        FROM articles
        WHERE article_id = @id
        AND is_deleted = 0
        LIMIT 1;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Article
                {
                    articleId = reader.GetInt32("article_id"),
                    name = reader.GetString("name"),
                    title = reader.GetString("title"),

                    shortDescription = reader.IsDBNull(reader.GetOrdinal("short_description"))
                        ? string.Empty
                        : reader.GetString("short_description"),

                    fullDescription = reader.IsDBNull(reader.GetOrdinal("full_description"))
                        ? string.Empty
                        : reader.GetString("full_description"),

                    hyperlink = reader.IsDBNull(reader.GetOrdinal("hyperlink"))
                        ? string.Empty
                        : reader.GetString("hyperlink"),

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

        public async Task<List<Article>> GetArticles()
        {
            var articles = new List<Article>();

            var connection =  _dbconn.GetConnection();
            await connection.OpenAsync();
            string query = "SELECT * \r\nFROM articles\r\nORDER BY created_date\r\nLIMIT 3;";
            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                articles.Add(new Article
                {
                    articleId = reader.GetInt32("article_id"),
                    name = reader.GetString("name"),
                    title = reader.GetString("title"),

                    shortDescription = reader.IsDBNull(reader.GetOrdinal("short_description"))
        ? string.Empty
        : reader.GetString("short_description"),

                    fullDescription = reader.IsDBNull(reader.GetOrdinal("full_description"))
        ? string.Empty
        : reader.GetString("full_description"),

                    hyperlink = reader.IsDBNull(reader.GetOrdinal("hyperlink"))
        ? string.Empty
        : reader.GetString("hyperlink"),

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
             return articles;
        }

        public async Task<bool> UpdateArticle(Article article)
        {
            using var connection = _dbconn.GetConnection();
            await connection.OpenAsync();

            string query = @"
        UPDATE articles
        SET
            name = @name,
            title = @title,
            short_description = @shortDescription,
            full_description = @fullDescription,
            hyperlink = @hyperlink,
            updated_date = @updatedDate,
            updated_by = @updatedBy
        WHERE
            article_id = @id
            AND is_deleted = 0;";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", article.articleId);
            command.Parameters.AddWithValue("@name", article.name);
            command.Parameters.AddWithValue("@title", article.title);
            command.Parameters.AddWithValue("@shortDescription", article.shortDescription);
            command.Parameters.AddWithValue("@fullDescription", article.fullDescription);
            command.Parameters.AddWithValue("@hyperlink", article.hyperlink);
            command.Parameters.AddWithValue("@updatedDate", DateTime.Now);
            command.Parameters.AddWithValue("@updatedBy", article.UpdatedBy);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
    }
}
