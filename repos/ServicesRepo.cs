using Microsoft.EntityFrameworkCore;
using realworldProject.DBModel;
using realworldProject.Models;

namespace realworldProject.repos
{
    public class ServicesRepo
    {

        public static UserModel? GetCurrentUser(string token)
        {
            UserModel? user;
            using (var userTable = new MyDatabase())
            {
                user = userTable.Users.FirstOrDefault(x => x.Token == token);

            }
            return user;
        }

        public static bool isFollowing(MyDatabase context ,string username,string following)
        {
            FollowingModel? result = context.Following.FirstOrDefault(x => (x.Username == username && x.FollowingName == following));
            return result != null;
        }

        public static bool isFavorieted(MyDatabase context, string username, string slug)
        {
            ArticlesFavoriets? result = context.Favoriets.FirstOrDefault(x => (x.Username == username && x.Slug == slug));
            return result != null;
        }

        public static UserModel? GetUserByUsername(MyDatabase context, string username)
        {
            UserModel? user = context.Users.FirstOrDefault(x => x.Username == username);
            return user;
        }

        public static void AddNewTags(string slug, List<string> tags)
        {
            using (var context = new MyDatabase())
            {
                foreach(var tag in tags)
                {
                    var raw = new ArticlesTags() { Slug = slug, Tagname = tag };
                    context.ArticleTags.Add(raw);
                }
                context.SaveChanges();
            }
        }

        public static void AddNewFavoriets(string slug, string username)
        {
            using (var context = new MyDatabase())
            {
                var favorite = new ArticlesFavoriets() { Slug = slug, Username = username };
                context.Favoriets.Add(favorite);
                context.SaveChanges();
            }
        }
        public static void AddNewArticle(ArticlesModel article, string username)
        {
            using (var context = new MyDatabase())
            {
                context.Articles.Add(article);
                context.SaveChanges();
            }

        }

        public static List<string>? GetArticleTags(string slug)
        {
            List<string> tags;
            using (var context = new MyDatabase())
            {
                tags = context.ArticleTags.Where(x => x.Slug == slug).Select(x => x.Tagname).ToList();
            }
            return tags;
        }

        public static List<ArticlesModel> GetAllArticles()
        {
            List<ArticlesModel> articles;
            using(var context = new MyDatabase())
            {
                articles = context.Articles.ToList();
            }
            return articles;
        }

        public static ArticleResponse CreateResponse(ArticlesModel article, UserModel user )
        {
            ArticleResponse resp;
            using (var context = new MyDatabase())
            {
                var favorietsCount = NumberOfFavoriets(context, article.Slug);
                var isfollowing = isFollowing(context, user.Username, article.AuthorName);
                var isfavorieted = isFavorieted(context, user.Username, article.Slug);
                var author = GetUserByUsername(context, article.AuthorName);
                var authorProfile = new profile(author.Username, author.Bio, author.Image, isfollowing);
                var tags = GetArticleTags(article.Slug);
                resp = new ArticleResponse(
                     article.Slug,
                     article.Title,
                     article.Description,
                     article.Body,
                     tags,
                     article.CreatedAt,
                     article.UpdatedAt,
                     isfavorieted,
                     favorietsCount,
                     authorProfile
                );
            }
            return resp;
        }
        

        public static int NumberOfFavoriets(MyDatabase context,string slug)
        {            
            int count = context.Favoriets.Where(x=>x.Slug == slug).Count();
            return count;
        }
        public static List<ArticlesModel> GetArticlesByAuthor(string author)
        {
            List<ArticlesModel> articles;
            using(var context = new MyDatabase())
            {
                articles = context.Articles.Where(x => x.AuthorName == author).ToList();
            }
            return articles;
        }
        public static List<ArticlesModel> GetArticlesFavoritedByUser(string favorited)
        {
            List<ArticlesModel> articles = new List<ArticlesModel>();
            using (var context = new MyDatabase())
            {
                var slugs = context.Favoriets.Where(a => a.Username == favorited).Select(a => a.Slug).ToList();
                foreach (var slug in slugs)
                {
                    articles.Add(context.Articles.FirstOrDefault(x => x.Slug == slug));
                }
                return articles;
            }
        }

        public static List<ArticlesModel> GetArticlesByTag(string tag)
        {
            List<ArticlesModel> articles = new List<ArticlesModel>();
            using (var context = new MyDatabase())
            {
                var slugs = context.ArticleTags.Where(a => a.Tagname == tag).Select(a => a.Slug).ToList();
                foreach (var slug in slugs)
                {
                    articles.Add(context.Articles.FirstOrDefault(x => x.Slug == slug));
                }
                return articles;
            }
        }

        public static ArticlesModel GetArticleBySlug(string slug)
        {
            ArticlesModel articles;
            using (var context = new MyDatabase())
            {
                articles = context.Articles.FirstOrDefault(a => a.Slug == slug);
            }
            return articles;
        }

        public static ArticlesModel FavoriteArticle(string slug,string username)
        {
            using (var context = new MyDatabase())
            {
                context.Favoriets.Add(new ArticlesFavoriets() { Username = username,Slug=slug,});
                context.SaveChanges();
            }
            var article = GetArticleBySlug(slug);
            return article;
        }

        public static ArticlesModel UnFavoriteArticle(string slug, string username)
        {
            
            ArticlesFavoriets favorited;
            using (var context = new MyDatabase())
            {
                favorited = context.Favoriets.FirstOrDefault(x => (x.Username == username && x.Slug == slug));
                context.Favoriets.Remove(favorited);
                context.SaveChanges();
            }
            var article = GetArticleBySlug(slug);
            return article;
        }

        public static ArticlesModel DeleteArticle(string slug, string username)
        {

            ArticlesFavoriets favorited;
            using (var context = new MyDatabase())
            {
                favorited = context.Favoriets.FirstOrDefault(x => (x.Username == username && x.Slug == slug));
                context.Favoriets.Remove(favorited);
                context.SaveChanges();
            }
            var article = GetArticleBySlug(slug);
            return article;
        }
    }

}
