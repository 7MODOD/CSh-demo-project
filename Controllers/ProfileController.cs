using Microsoft.AspNetCore.Mvc;
using realworldProject.DBModel;
using realworldProject.Models;
using realworldProject.repos;

namespace realworldProject.Controllers
{
    public class ProfileController : ControllerBase
    {
        [HttpGet]
        [Route("/profiles/{username}")]
        public ActionResult GetUserProfile(string username)
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
            profile authorProfile;
            using (var context = new MyDatabase())
            {
                var isFollowed = ServicesRepo.isFollowing(context, user.Username, username);
                var followedUser = ServicesRepo.GetUserByUsername(context, username);
                authorProfile = new profile(followedUser.Username, followedUser.Bio, followedUser.Image, isFollowed);
            }
            return Ok(new UserRequestEnv<profile>(authorProfile));
        }


        [HttpPost]
        [Route("/profiles/{username}/follow")]
        public ActionResult FollowProfile(string username)
        {
            string token = Request.Headers["Autorization"];
            if (token == string.Empty)
            {
                return BadRequest("you should log in first");
            }

            var user = ServicesRepo.GetCurrentUser(token);
            if (user == null)
            {
                return BadRequest("you are not Authorized");
            }
            UserModel? followUser;
            FollowingModel? raw;
            profile result;
            using (var context = new MyDatabase())
            {
                followUser = ServicesRepo.GetUserByUsername(context, username);
                if (followUser == null)
                {
                    return BadRequest("this profile is not exist");
                }
                raw = new FollowingModel()
                {
                    FollowingName = username,
                    Username = user.Username,
                };
                context.Following.Add(raw);
                context.SaveChanges();
                var isfollowed = ServicesRepo.isFollowing(context, user.Username, username);
                result = new profile(username, followUser.Bio, followUser.Image, isfollowed);
            }
            return Ok(new UserResponseEnv<profile>(result));
        }

        [HttpDelete]
        [Route("/profiles/{username}/follow")]
        public ActionResult UnFollowProfile(string username)
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
            bool isFollowed;
            FollowingModel? userToUnFollow;
            using (var Context =new MyDatabase())
            {
                isFollowed = ServicesRepo.isFollowing(Context, user.Username, username);
                if (!isFollowed)
                {
                    return BadRequest("you didn't follow him to do unfollow");
                }
                userToUnFollow = Context.Following.FirstOrDefault(x => (x.Username == user.Username && x.FollowingName == username));
                Context.Following.Remove(userToUnFollow);
                Context.SaveChanges();
            }

            var profile = new profile(userToUnFollow.FollowingName, user.Bio, user.Image, !isFollowed);
            return Ok(new UserResponseEnv<profile>(profile));
        }



    }
}
