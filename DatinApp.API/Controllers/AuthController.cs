using System.Threading.Tasks;
using DatinApp.API.Data;
using DatinApp.API.Dtos;
using DatinApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatinApp.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public IAuthRepository _repo { get; }
        public AuthController(IAuthRepository repo)
        {
            this._repo = repo;
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
    }
}