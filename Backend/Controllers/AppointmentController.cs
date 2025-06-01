using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        //[Authorize] // Uncomment if you want to restrict access
        public async Task<IActionResult> GetAppointments([FromQuery] AppointmentFilterDto filter)
        {
            var result = await _appointmentService.GetAppointmentsAsync(filter);

            if (!result.Succeeded)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }
        [HttpGet("doctor/{doctorId}/upcoming")]
        [Authorize(Roles = "Doctor,Admin")] // optional, but recommended
        public async Task<IActionResult> GetDoctorUpcomingAppointments(Guid doctorId)
        {
            var result = await _appointmentService.GetUpcomingAppointmentsForDoctorAsync(doctorId);
            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveAppointment([FromBody] ReserveAppointmentDto dto)
        {
            var result = await _appointmentService.ReserveAppointmentAsync(dto);
            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelAppointment([FromBody] CancelAppointmentDto dto)
        {
            var result = await _appointmentService.CancelAppointmentAsync(dto);
            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok();
        }
        [HttpGet("user/{userId}/upcoming")]
        public async Task<IActionResult> GetUserUpcomingAppointments(Guid userId)
        {
            var result = await _appointmentService.GetUpcomingAppointmentsForUserAsync(userId);
            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
        [HttpGet("user/{userId}/past")]
        public async Task<IActionResult> GetUserPastAppointments(Guid userId)
        {
            var result = await _appointmentService.GetPastAppointmentsForUserAsync(userId);
            if (!result.Succeeded)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
    }
}
