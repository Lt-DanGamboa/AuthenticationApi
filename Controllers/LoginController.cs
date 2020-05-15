using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthenticationApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config; 
        private List<UserModel> appUsers = new List<UserModel> 
        { 
            new UserModel { FullName = "Vaibhav Bhapkar", UserName = "admin", Password = "1234", UserRole = "Admin" }, 
            new UserModel { FullName = "Test User", UserName = "user", Password = "1234", UserRole = "User" } 
        };
        public LoginController(IConfiguration config) { _config = config; }
        
        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous] 
        public IActionResult Login(UserModel login) 
        { 
            IActionResult response = Unauthorized(); 
            UserModel user = AuthenticateUser(login); 
            if (user != null) 
            { 
                var tokenString = GenerateJWTToken(user); 
                response = Ok(new { token = tokenString, userDetails = user, }); 
            } 
            return response; 
        }
        UserModel AuthenticateUser(UserModel loginCredentials) 
        { 
            UserModel user = appUsers.SingleOrDefault(x => x.UserName == loginCredentials.UserName && x.Password == loginCredentials.Password); return user; 
        }
        string GenerateJWTToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim("fullName", userInfo.FullName.ToString()),
                new Claim("role",userInfo.UserRole),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            try{
                return new JwtSecurityTokenHandler().WriteToken(token);
            }catch(Exception e){
                Console.WriteLine(e.ToString());
                return e.ToString();
            }
        }
    }
}