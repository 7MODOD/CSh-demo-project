using Microsoft.AspNetCore.Mvc;
using realworldProject.Models;

namespace realworldProject.repos
{
    public class GetArticlesHendler
    {
        public static async Task<List<ArticleResponse>> GetAllArticles(UserModel user)
        {
            var articles = await ServicesRepo.GetAllArticles();
            List<ArticleResponse> responses = new List<ArticleResponse>();
            foreach (var article in articles)
            {
                var resp = ServicesRepo.CreateResponse(article, user);
                responses.Add(resp);
            }
            return responses;
        }

        public static async Task<List<ArticleResponse>> GetArticlesByAuthor(string author, UserModel user)
        {
            var articles = await ServicesRepo.GetArticlesByAuthor(author);
            List<ArticleResponse> responses = new List<ArticleResponse>();
            foreach (var article in articles)
            {
                var resp = ServicesRepo.CreateResponse(article, user);
                responses.Add(resp);
            }
            return responses;
        }

        public static async Task<List<ArticleResponse>> GetArticlesFavoritedByUser([FromQuery] string? favorited, UserModel user)
        {
            var articles = await ServicesRepo.GetArticlesFavoritedByUser(favorited);
            List<ArticleResponse> responses = new List<ArticleResponse>();
            foreach (var article in articles)
            {
                var resp = ServicesRepo.CreateResponse(article, user);
                responses.Add(resp);
            }
            return responses;
        }

        public static async Task<List<ArticleResponse>> GetArticlesByTag([FromQuery] string? tag, UserModel user)
        {
            var articles = await ServicesRepo.GetArticlesByTag(tag);
            List<ArticleResponse> responses = new List<ArticleResponse>();
            foreach (var article in articles)
            {
                var resp = ServicesRepo.CreateResponse(article, user);
                responses.Add(resp);
            }
            return responses;
        }


    }
}
