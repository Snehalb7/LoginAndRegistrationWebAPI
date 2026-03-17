using LoginAndRegistrationWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoginAndRegistrationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext dbContext;

        public UserController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = dbContext.Users.FirstOrDefault(u => u.Email == userDTO.Email);
            if (user == null)
            {
                dbContext.Users.Add(new User
                {
                    Firstrname = userDTO.Firstrname,
                    Lastname = userDTO.Lastname,
                    Email = userDTO.Email,
                    Password = userDTO.Password
                });
                dbContext.SaveChanges();
                return Ok("User registered successfully");
            }
            else
            {
                return BadRequest("User with this email already exists");
            }


        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDTO LoginDTO)
        {

            var user = dbContext.Users.FirstOrDefault(u => u.Email == LoginDTO.Email && u.Password == LoginDTO.Password);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("Invalid email or password");
            }

        }
        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = dbContext.Users.ToList();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound("User not found");
            }
        }
    }
}
