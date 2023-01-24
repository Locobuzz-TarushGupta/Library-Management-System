using library_management_system.DatabaseLayer;
using library_management_system.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace library_management_system.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        DbAuth dbAuth = new DbAuth();

        [HttpPost("Librarian/login")]
        public async Task<ActionResult<string>> LibrarianLogin(Userdto request)
        {
            if (user.Username != request.Username)
            {
                return BadRequest("User not Found.");
            }
            /*if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong Password.");
            }*/
            string token = CreateStudentToken(user);
            return Ok(token);
        }

        public static User user = new User();

        [HttpPost("student/register")]
        public async Task<ActionResult<User>> StudentRegister(Userdto request)
        {
            //string hashString = CreatePasswordHash(request.Password, out byte[] passwordHashverify, out byte[] passwordSalt);
            byte[] hashString = CreatePasswordHash(request.Password, out byte[] passwordHashverify, out byte[] passwordSalt);
            user.Username = request.Username;
            user.PasswordHash = passwordHashverify;
            user.PasswordSalt = passwordSalt;

            string md5Hash = CreateMD5(request.Password);
            string result = dbAuth.AddCredentials(user.Username, md5Hash);
            if(result == "0")  {
                return BadRequest("Username already exists. Please try again with different username or try logging in");
            } else if(result == "1")  {
                return Ok("Student Registered successfully. Please login to continue.");
            }
            return BadRequest(result);
        }

        [HttpPost("student/login")]
        public async Task<ActionResult<string>> StudentLogin(Userdto request)
        {
            byte[] passwordHashverify = CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            string md5Hash = CreateMD5(request.Password);
            string result = dbAuth.VerifyCredentials(request.Username, md5Hash);
            if(result == "0")
            {
                return BadRequest("Invalid Username or Password.");
            } else if(result == "1") {
                return BadRequest("Invalid Username or Password.");
            } else if(result == "2") {
                string token = CreateStudentToken(user);
                return Ok("Student Logged in successfully.\n"+ "Token:"+token);
            }
            return BadRequest(result);
        }

        private string CreateLibrarianToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Librarian")
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return string.Empty;
        }
        private string CreateStudentToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Student")
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        /*
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }*/

        public static string CreateMD5(string input)
        {
            using(System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }

        private byte[] CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            string result = string.Empty;
            byte[] passHash = new byte[1000];
            using (var hmac = new HMACMD5())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.Default.GetBytes(password));
                passHash = passwordHash;
                result = Encoding.Default.GetString(passwordHash);
            }
            return passHash;
        }

        /*
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACMD5(user.PasswordSalt))
            {
                var computerHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes((string)password));
            }
        }
        */
    }
}
