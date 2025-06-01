using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Backend.Entities;

namespace Backend.Controllers
{

    [Authorize(Roles = "Admin")]
    [Route("api/admin/users")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AdminUserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPatch("{id}/toggle-active")]
        public async Task<IActionResult> ToggleUserActiveStatus(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            user.IsActive = !user.IsActive; // Toggle the active status
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to update user status.");
            }
            return Ok(new { user.Id, user.IsActive });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to delete user.");
            }
            return Ok($"User {id} deleted.");
        }
    }
}
