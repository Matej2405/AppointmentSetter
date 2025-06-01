// Backend/Controllers/AvailabilityController.cs
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AvailabilityController : ControllerBase
{
    private readonly IAvailabilityService _service;

    public AvailabilityController(IAvailabilityService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(AvailabilityDto dto)
    {
        var result = await _service.CreateAvailabilityAsync(dto);
        if (!result.Succeeded) return BadRequest(result.Message);
        return Ok(result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, AvailabilityDto dto)
    {
        dto.Id = id;
        var result = await _service.UpdateAvailabilityAsync(dto);
        if (!result.Succeeded) return NotFound(result.Message);
        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAvailabilityAsync(id);
        if (!result.Succeeded) return NotFound(result.Message);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetAvailabilityByIdAsync(id);
        if (!result.Succeeded) return NotFound(result.Message);
        return Ok(result.Data);
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetDoctorAvailabilities(Guid doctorId)
    {
        var result = await _service.GetDoctorAvailabilitiesAsync(doctorId);
        return Ok(result.Data);
    }
}
