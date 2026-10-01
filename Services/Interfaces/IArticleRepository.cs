using GPACARICOMAPI.Controllers;
using GPACARICOMAPI.Models;
using GPACARICOMAPI.Models.DTO.GPACARICOMAPI.Models;
namespace GPACARICOMAPI.Services.Interfaces
{
    public interface IArticleRepository
    {
        public Task<List<Article>> GetArticles();
        public Task<Article?> GetSingleArticle(int id);
        public Task<bool> AddArticle(ArticleDTO article);
        public Task<bool> UpdateArticle(Article article);
        public Task<bool> DeleteArticle(int id);

    }
}
