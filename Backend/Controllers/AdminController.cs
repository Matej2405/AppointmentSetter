using Backend.Entities;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{



        [Authorize(Roles = "Admin")]
        [Route("api/[controller]")]
        [ApiController]
        public class AdminController : ControllerBase
        {
            private readonly IUserService _userService;
            private readonly UserManager<User> _userManager;

            public AdminController(IUserService userService, UserManager<User> userManager)
            {
                _userService = userService;
                _userManager = userManager;
            }

            [HttpGet("users")]
            public async Task<IActionResult> GetAllUsersWithRoles()
            {
                var users = _userManager.Users.ToList();

                var userDtos = new List<UserWithRolesDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userDtos.Add(new UserWithRolesDto
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        Roles = roles.ToList()
                    });
                }

                return Ok(userDtos);
            }
        }
}