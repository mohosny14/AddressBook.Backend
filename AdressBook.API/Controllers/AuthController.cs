using AdressBook.Domain.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using AdressBook.Services.IServices;

namespace AdressBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.Login(model);
                return (response.Succeeded) ? Ok(response) : BadRequest(response);
            }
            return Unauthorized();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.RegisterAsync(model);
            return (response.Succeeded) ? Ok(response) : BadRequest(response);
        }
    }
}