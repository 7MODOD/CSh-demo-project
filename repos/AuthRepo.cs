namespace realworldProject.repos
{
    public class AuthRepo
    {
        public static bool isAuthenticated(string token)
        {
            return token !=string.Empty;
        }
        public static bool isAuthorized(string token)
        {
            var user = ServicesRepo.GetCurrentUser(token);
            return user != null;
        }





    }
}
