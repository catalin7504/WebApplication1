using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("/Views/Auth/Auth.cshtml");
        }

        [HttpPost]
        public IActionResult Index([FromForm]Login user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (ValidateCredentials(user.UserName.ToLower(), user.Password))
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this_is_my_secret_key_2020"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var expiration = DateTime.Now.AddMinutes(60);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:5000",
                    audience: "http://localhost:5000",
                    claims: new List<Claim>(),
                    expires: expiration,
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString, Expires = expiration });
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool ValidateCredentials(string userName, string password)
        {
            // We need to check the credentials using the Customers table
            return true;
        }
    }
}