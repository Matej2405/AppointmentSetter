// Backend/Services/Interfaces/IAvailabilityService.cs
using Backend.Models;

public interface IAvailabilityService
{
    Task<Result<AvailabilityDto>> CreateAvailabilityAsync(AvailabilityDto dto);
    Task<Result<AvailabilityDto>> UpdateAvailabilityAsync(AvailabilityDto dto);
    Task<Result<bool>> DeleteAvailabilityAsync(Guid id);
    Task<Result<AvailabilityDto>> GetAvailabilityByIdAsync(Guid id);
    Task<Result<List<AvailabilityDto>>> GetDoctorAvailabilitiesAsync(Guid doctorId);
}
