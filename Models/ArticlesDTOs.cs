namespace realworldProject.Models
{
    public record ArticleResponse(string slug, string title, 
        string description, string body, List<string> tagList,
        DateTime createdAt, DateTime updatedAt, bool favorited,
        int favoritesCount, profile Author);

    public record FavorietsModel(string username, string slug);
    public record ArticleCreateRequest(string title, string description, string body, List<string> tagList);
    public record ArticleRequestEnv<T>(T article);
    public record ArticleResponseEnv<T>(T article);


}
/*
"article": {
    "slug": "how-to-train-your-dragon",
    "title": "How to train your dragon",
    "description": "Ever wonder how?",
    "body": "It takes a Jacobian",
    "tagList": ["dragons", "training"],
    "createdAt": "2016-02-18T03:22:56.637Z",
    "updatedAt": "2016-02-18T03:48:35.824Z",
    "favorited": false,
    "favoritesCount": 0,
    "author": {
        "username": "jake",
      "bio": "I work at statefarm",
      "image": "https://i.stack.imgur.com/xHWG8.jpg",
      "following": false
    }*/