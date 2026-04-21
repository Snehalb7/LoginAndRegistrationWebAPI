using LoginAndRegistrationWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoginAndRegistrationWebAPI.Filters;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LoginAndRegistrationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ActionFilter]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext dbContext;
        private readonly IConfiguration _configuration;
        public UserController(MyDbContext dbContext,IConfiguration configuration)
        {
            this.dbContext = dbContext;
            _configuration = configuration;
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

                var claim = new[]
                {
                    new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                    new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("UserId",user.UserId.ToString()),
                    new Claim("Email",user.Email.ToString())
                    

                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"], //who created token
                    _configuration["Jwt:Audience"], // who will consume(use) the token
                    claim,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signIn // security key and algorithm used to sign the token
                    );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new {Token=tokenString,User=user });

                //return Ok(user);
            }
            else
            {
                return BadRequest("Invalid email or password");
            }

        }
        [HttpGet]
        [Route("GetAllUsers")]
        // [ActionFilter]
        [ExceptionFilter]
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
        [HttpPut]
        [Route("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = dbContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                user.Firstrname = userDTO.Firstrname;
                user.Lastname = userDTO.Lastname;
                user.Email = userDTO.Email;
                user.Password = userDTO.Password;
                user.CreatedOn = DateTime.Now;
                return Ok("User updated successfully");
            }
            else
            {
                return NotFound("User not found");
            }
        }
        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = dbContext.Users.FirstOrDefault(user => user.UserId == id);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
                return Ok("User deleted successfully");
            }
            else
            {
                return NotFound("User not found");
            }
        }
        [HttpPatch]
        [Route("PatchUser/{id}")]
        public IActionResult PatchUser(int id, UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = dbContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {

                if (!string.IsNullOrEmpty(userDTO.Password))
                {
                    user.Password = userDTO.Password;
                }
                user.CreatedOn = DateTime.Now;
                dbContext.SaveChanges();
                return Ok("User updated successfully");
            }
            else
            {
                return NotFound("User not found");
            }

        }
    }
}
