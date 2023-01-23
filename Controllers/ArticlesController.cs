using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using realworldProject.DBModel;
using realworldProject.Models;
using realworldProject.repos;

namespace realworldProject.Controllers
{
    public class ArticlesController: ControllerBase
    {
        [HttpPost]
        [Route("articles")]
        public ActionResult CreateArticle([FromBody] ArticleRequestEnv<ArticleCreateRequest> req)
        {
            string token = Request.Headers["Authorization"];
            if (!AuthRepo.isAuthenticated(token))
            {
                return BadRequest("you should login first");
            }
            if (!AuthRepo.isAuthorized(token))
            {
                return BadRequest("you are not Authorized");
            }
            var user = ServicesRepo.GetCurrentUser(token);
            string slug = req.article.title.Replace(' ', '-');
            var article = new ArticlesModel()
            {
                Slug = slug,
                Title = req.article.title,
                Description = req.article.description,
                Body = req.article.body,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                AuthorName = user.Username,
            };
            ServicesRepo.AddNewArticle(article, user.Username);
            ServicesRepo.AddNewTags(slug, req.article.tagList);
            ServicesRepo.AddNewFavoriets(slug, user.Username);
            var tags = ServicesRepo.GetArticleTags(slug);
            var author = new profile(user.Username, user.Bio, user.Image, true);
            var resp = new ArticleResponse(article.Slug, article.Title,
                article.Description, article.Body, tags, article.CreatedAt,
                article.UpdatedAt, true, 1, author);
            return Ok(new ArticleResponseEnv<ArticleResponse>(resp));
        }
        [HttpGet]
        [Route("articles")]
        public ActionResult GetArticlesWithParamsHandler()
        {
            string token = Request.Headers["Authorization"];
            if (!AuthRepo.isAuthenticated(token))
            {
                return BadRequest("you should login first");
            }
            if (!AuthRepo.isAuthorized(token))
            {
                return BadRequest("you are not Authorized");
            }
            var user = ServicesRepo.GetCurrentUser(token);
            List<ArticleResponse> article;
            
            if (Request.Query["author"] != string.Empty)
            {
                article = GetArticlesHendler.GetArticlesByAuthor(Request.Query["author"], user);
            }
            else if(Request.Query["tag"] != string.Empty)
            {
                article = GetArticlesHendler.GetArticlesByTag(Request.Query["tag"], user);
            }
            else if(Request.Query["favorited"] != string.Empty)
            {
                article = GetArticlesHendler.GetArticlesFavoritedByUser(Request.Query["favorited"], user);
            }
            else
            {
                article = GetArticlesHendler.GetAllArticles(user);
            }
            return Ok(new ArticleResponseEnv<List<ArticleResponse>>(article));

        }

        [HttpGet]
        [Route("articles/feed")]
        public ActionResult GetFeed()
        {
            string token = Request.Headers["Authorization"];
            if (!AuthRepo.isAuthenticated(token))
            {
                return BadRequest("you should login first");
            }
            if (!AuthRepo.isAuthorized(token))
            {
                return BadRequest("you are not Authorized");
            }
            var user = ServicesRepo.GetCurrentUser(token);
            var articles = ServicesRepo.GetArticlesFavoritedByUser(user.Username);
            List<ArticleResponse> responses = new List<ArticleResponse>();
            foreach (var article in articles)
            {
                var resp = ServicesRepo.CreateResponse(article, user);
                responses.Add(resp);
            }
            return Ok(new ArticleResponseEnv<List<ArticleResponse>>(responses));
        }
        [HttpGet]
        [Route("articles/{slug}")]
        public ActionResult GetArticleBySlug(string slug)
        {
            string token = Request.Headers["Authorization"];
            if (!AuthRepo.isAuthenticated(token))
            {
                return BadRequest("you should login first");
            }
            if (!AuthRepo.isAuthorized(token))
            {
                return BadRequest("you are not Authorized");
            }
            var user = ServicesRepo.GetCurrentUser(token);
            var article = ServicesRepo.GetArticleBySlug(slug);
            var resp = ServicesRepo.CreateResponse(article, user);
            return Ok(new ArticleResponseEnv<ArticleResponse>(resp));
        }

        [HttpPost]
        [Route("articles/{slug}/favorite")]
        public ActionResult FavoriteArticle(string slug)
        {
            string token = Request.Headers["Authorization"];
            if (!AuthRepo.isAuthenticated(token))
            {
                return BadRequest("you should login first");
            }
            if (!AuthRepo.isAuthorized(token))
            {
                return BadRequest("you are not Authorized");
            }
            var user = ServicesRepo.GetCurrentUser(token);
            bool isfavorited;
            using (var context = new MyDatabase())
            {
                isfavorited = ServicesRepo.isFavorieted(context, user.Username, slug);
            }
            if (isfavorited)
            {
                return BadRequest("this article is already favorited");
            }
            var article = ServicesRepo.FavoriteArticle(slug,user.Username);
            var resp = ServicesRepo.CreateResponse(article, user);
            return Ok(new ArticleResponseEnv<ArticleResponse>(resp));
        }

        [HttpDelete]
        [Route("articles/{slug}/favorite")]
        public ActionResult UnFavoriteArticle(string slug)
        {
            string token = Request.Headers["Authorization"];
            if (!AuthRepo.isAuthenticated(token))
            {
                return BadRequest("you should login first");
            }
            if (!AuthRepo.isAuthorized(token))
            {
                return BadRequest("you are not Authorized");
            }
            var user = ServicesRepo.GetCurrentUser(token);
            bool isfavorited;
            using (var context = new MyDatabase())
            {
                isfavorited = ServicesRepo.isFavorieted(context, user.Username, slug);
            }
            if (!isfavorited)
            {
                return BadRequest("this article is not favorited");
            }
            var article = ServicesRepo.UnFavoriteArticle(slug, user.Username);
            var resp = ServicesRepo.CreateResponse(article, user);
            return Ok(new ArticleResponseEnv<ArticleResponse>(resp));
        }

        [HttpDelete]
        [Route("articles/{slug}")]
        public ActionResult DeleteArticle(string slug)
        {
            string token = Request.Headers["Authorization"];
            if (!AuthRepo.isAuthenticated(token))
            {
                return BadRequest("you should login first");
            }
            if (!AuthRepo.isAuthorized(token))
            {
                return BadRequest("you are not Authorized");
            }
            var user = ServicesRepo.GetCurrentUser(token);
            
            var article = ServicesRepo.UnFavoriteArticle(slug, user.Username);
            var resp = ServicesRepo.CreateResponse(article, user);
            return Ok(new ArticleResponseEnv<ArticleResponse>(resp));
        }
    }
}
