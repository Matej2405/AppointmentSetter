using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // Endpoints will go here
        [HttpGet("{doctorId}/specializations")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetDoctorSpecializations(Guid doctorId)
        {
            var result = await _doctorService.GetDoctorSpecializationsAsync(doctorId);
            if (!result.Succeeded)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPut("specializations")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctorSpecializations([FromBody] DoctorSpecializationDto dto)
        {
            var result = await _doctorService.UpdateDoctorSpecializationsAsync(dto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }
    }
}
