using realworldProject.DBModel;

namespace realworldProject.Models
{
    public class UserModel
    {
        //public UserModel(string email, string username,
        //                    string password, string token, string? bio,string? image)
        //{
        //    Email = email;
        //    Username = username;
        //    Password = password;
        //    Token = token;
        //    Bio = bio;
        //    Image = image;
        //}
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }
        public virtual List<FollowingModel> Following { get; set; }
        public virtual List<ArticlesModel> FavorietArticles { get; set; }
        public virtual List<ArticlesModel> MyArticles { get; set; }
        //public List<string>? Following { get; set; }




    }

    


}
