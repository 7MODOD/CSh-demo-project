
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using realworldProject.DBModel;
using realworldProject.Models;
using realworldProject.Validators;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using realworldProject.repos;

namespace realworldProject.Controllers
{
    
    //[ApiController]
    public class userController : ControllerBase
    {
     
        [HttpPost("/users")]
        public ActionResult CreateUser([FromBody] UserRequestEnv<UserCreateRequest> req)
        {
            var valodateor = new UserCreateValidator();
            var isValid = valodateor.Validate(req.user);
            if (isValid == null)
            {
                return BadRequest("some information is not valid\n");
            }

            var user = new UserModel()
            {
                Email = req.user.Email,
                Username = req.user.username,
                Password = req.user.Password,
                Token = $"{Guid.NewGuid()}",
                Bio = "",
                Image = "",
                FavorietArticles = new List<ArticlesModel>(),
                Following = new List<FollowingModel>(),
            };

            using (var userTable = new MyDatabase())
            {
                userTable.Users.Add(user);
                userTable.SaveChanges();
            }
            var resp = new UserResponse(user.Email, user.Token, user.Username, user.Bio, user.Image);

            return Ok(new UserResponseEnv<UserResponse>(resp));
        }

        [HttpPost]
        [Route("/users/login")]
        public ActionResult UserLogin([FromBody] UserRequestEnv<UserLoginRequest> req)
        {
            UserModel? user;
            var valodateor = new UserLoginValidator();
            var isValid = valodateor.Validate(req.user);
            if (isValid == null)
            {
                return BadRequest("some information is not valid\n");
            }
            using (var userTable= new MyDatabase())
            {
                 user = userTable.Users.FirstOrDefault(x => x.Email == req.user.Email && x.Password == req.user.Password);
                
            }
            if(user == null)
            {
                return BadRequest("Email or password is wrong\n");

            }

            var resp = new UserResponse(user.Email, user.Token, user.Username,user.Bio,user.Image);
            return new JsonResult(new UserResponseEnv<UserResponse>(resp));
        }


        [HttpGet]
        [Route("/user")]
        public ActionResult GetCurrentUser()
        {
            if (Request.Headers["Authorization"] == string.Empty)
            {
                return BadRequest("you should be logged in to see this");

            }

            var user = ServicesRepo.GetCurrentUser(Request.Headers["Authorization"]);
            if (user == null)
            {
                return BadRequest("you are not Authorized");
            }
            return Ok(new UserResponseEnv<UserModel> (user));
        }


        //here i will user the serialization its just to apply the Async and Deserialization 
        // i can use [fromBody] request....etc,,,,,,,
        // but i think i should show every thing that i have in this project

        [HttpPut]
        [Route("/user")]
        public async Task<ActionResult> EditEmail()
        {
            if(Request.Headers["Authorization"] == string.Empty)
            {
                return BadRequest("you should be logged in first\n");
            }
            string body = "";
            using (var read = new StreamReader(Request.Body))
            {
                body = await read.ReadToEndAsync();
            }
            var req = JsonSerializer.Deserialize<UserRequestEnv<UserUpdateInfo>> (body,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var user = ServicesRepo.GetCurrentUser(Request.Headers["Authorization"]);
            if(user == null)
            {
                return BadRequest("you are not Authorized\n");

            }
            using (var userTable = new MyDatabase())
            {
                userTable.Users.Remove(user);

                if(req.user.Email != null && req.user.Email != user.Email)
                {
                    user.Email = req.user.Email;
                }
                if (req.user.password != null && req.user.password != user.Password)
                {
                    user.Password = req.user.password;
                }
                if (req.user.username != null && req.user.username != user.Username)
                {
                    user.Username = req.user.username;
                }
                if (req.user.bio != null && req.user.bio != user.Bio)
                {
                    user.Bio = req.user.bio;
                }
                if (req.user.image != null && req.user.image != user.Image)
                {
                    user.Image = req.user.image;
                }
                userTable.Users.Add(user);
                return Ok(user);
            }
        }





    }
}
