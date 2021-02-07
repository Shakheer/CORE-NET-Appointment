using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DoctorAppointment.DatabaseContexts;
using DoctorAppointment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DoctorAppointment.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const string secretkey = "1234567890123456";
        public static readonly SymmetricSecurityKey SigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(UserController.secretkey));
        [HttpGet]
        [Route("api/User/GetUsers")]
        public List<User> Getusers()
        {
            DoctorAppointmentContext userDBC = new DoctorAppointmentContext();
            var lstUser =  userDBC.User.ToList();
            return lstUser;
        }

        [HttpPost]
        [Route("api/User/RegisterUser")]
        public IActionResult RegisterUser([FromBody]User user)
        {
            DoctorAppointmentContext userDbc = new DoctorAppointmentContext();
            userDbc.Add(user);
            userDbc.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("api/User/Login")]
        public IActionResult UserLogin([FromBody]UserCredentials cred)
        {
            DoctorAppointmentContext userDBC = new DoctorAppointmentContext();
            var isAvailable = userDBC.User.Any<User>(user => (user.UserName == cred.userName) && (user.Password == cred.password));
            
            if (isAvailable)
            {
                User uObj = userDBC.User.FirstOrDefault(u => u.UserName == cred.userName);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity( new Claim[]
                    {
                        new Claim("UserId",uObj.UserName.ToString())
                    })
                    ,Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(SigninKey,SecurityAlgorithms.HmacSha256Signature)
                };
                //return Ok(uObj);
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
            {
                return BadRequest(cred);
            }
        }
    }
}
