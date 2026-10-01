using GPACARICOMAPI.Models;
using GPACARICOMAPI.Models.DTO.GPACARICOMAPI.Models;
using GPACARICOMAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GPACARICOMAPI.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        [AllowAnonymous]
        [HttpGet("getAllArticles")]
        public async Task<ActionResult<List<Article>>> GetArticles()
        {
            var articles = await _articleRepository.GetArticles();
            return Ok(new ApiResponse<List<Article>> { 
             success = true ,
             message = "Successfully retreived articles",
             data = articles
            }
                );
         }

        [AllowAnonymous]
        [HttpGet("getArticle/{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _articleRepository.GetSingleArticle(id);

            if (article == null)
            {
                return NotFound( new ApiResponse<string> {success = false});
            }
            return Ok(new ApiResponse<Article> {success = true, message = "Retreived article", data = article});
        }


        //ADDS A NEW ARTICLE 
        [HttpPost("addArticle")]
        public async Task<IActionResult> AddArticle([FromBody] ArticleDTO article)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = await _articleRepository.AddArticle(article);

            if (!success)
                return BadRequest(new ApiResponse<string>{ success = true, message = "Unable to create article." });

            return Ok(new ApiResponse<string>
            {
                success = true,
                message = "Article created successfully."
            });
        }


            // UPDATES THE ARTICLE USING THE ID 
            [HttpPut("updateArticle/{id}")]
            public async Task<IActionResult> UpdateArticle(int id, [FromBody] Article article)
            {
                bool success = await _articleRepository .UpdateArticle(article);

                if (!success)
                    return NotFound(new ApiResponse<string>
                    {
                        success = true,
                        message = "Article not found."
                    });

                return Ok(new ApiResponse<string>
                {
                    success = true,
                    message = "Article updated successfully."
                });
            }

    }
}
