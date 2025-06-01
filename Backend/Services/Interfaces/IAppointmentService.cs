using Backend.Models;
using Backend.Entities;

namespace Backend.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<Result<List<Appointment>>> GetFilteredAppointmentsAsync(AppointmentFilterDto filter);
        Task<Result<List<Appointment>>> GetAppointmentsAsync(AppointmentFilterDto filter);

    }
}
