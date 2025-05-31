using Backend.Models;
using Backend.Models.LoginAuthModels;
using Backend.Services;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ITokenProviderService _tokenProviderService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService,
            ITokenProviderService tokenProviderService,
            IUserService userService,
            IConfiguration configuration)
        {
            _authService = authService;
            _userService = userService;
            _tokenProviderService = tokenProviderService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginDto)
        {
            var result = await _authService.AreValidCredentials(loginDto);

            if (result.Succeeded)
            {
                var user = result.Data;
                var username = user.UserName ?? string.Empty;
                var email = user.Email ?? string.Empty;
                var token = _tokenProviderService.GenerateToken(user);

                Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // change to true to enable https, currently http for local react app
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes"))
                });

                var response = new LoginResponseDto(user.Id, username, email);
                return Ok(response);
            }

            return Unauthorized("Username/Password is incorrect.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerDto)
        {
            var result = await _authService.Register(registerDto);

            if (!result.Succeeded)
            {
                return Conflict(result.Message);
            }

            Response.Cookies.Delete("jwt");
            return Ok();
        }
    }
}
