using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public ActionResult login(string username, string password)
        {
            if (username == "admin" && password == "Pa$$w0rd")
            {
                #region claims
                List<Claim> userData = new List<Claim>();
                userData.Add(new Claim(ClaimTypes.Name, "admin"));
                userData.Add(new Claim(ClaimTypes.Email, "adlyahmed1690@gmail.com"));
                #endregion
                string key = "This is my Secret Key -Ahmed Adly-";
                var SecKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                var signingCred = new SigningCredentials(SecKey, SecurityAlgorithms.HmacSha256);
                #region generate token
                var token = new JwtSecurityToken(
                    claims: userData,
                    expires: DateTime.Now.AddMinutes(45),
                    signingCredentials: signingCred
                    );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(tokenString);
                #endregion
            }
            else
                return Unauthorized();
        }
    }
}
