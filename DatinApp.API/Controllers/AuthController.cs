using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatinApp.API.Data;
using DatinApp.API.Dtos;
using DatinApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatinApp.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public IAuthRepository _repo { get; }
        public AuthController(IAuthRepository _repo, IConfiguration config)
        {
            this._config = config;
            this._repo = _repo;

        }



        [HttpPost("register")]

        public async Task<IActionResult> Register(UserForRegisterDto userforRegisterDto)
        {
            userforRegisterDto.Username = userforRegisterDto.Username.ToLower();

            if (await _repo.UserExist(userforRegisterDto.Username))
                return BadRequest("Username already exist");

            var userToCreate = new tbl_user
            {
                Username = userforRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate, userforRegisterDto.Password);

            return StatusCode(201);

        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(UserForRegisterDto UserForLoginDto)
        {

            var userFromRepo = await _repo.Login(UserForLoginDto.Username.ToLower(), UserForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claim = new[]
            {
                   new Claim(ClaimTypes.NameIdentifier ,userFromRepo.Id.ToString()),
                   new Claim(ClaimTypes.Name ,userFromRepo.Username)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new{

                token = tokenHandler.WriteToken(token)
            });
        }
    }
}