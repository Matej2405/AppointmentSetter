using Backend.Models;
using Backend.Entities;

namespace Backend.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<Result<List<Appointment>>> GetFilteredAppointmentsAsync(AppointmentFilterDto filter);
        Task<Result<List<Appointment>>> GetAppointmentsAsync(AppointmentFilterDto filter);
        Task<Result<List<Appointment>>> GetUpcomingAppointmentsForDoctorAsync(Guid doctorId);
        Task<Result<Appointment>> ReserveAppointmentAsync(ReserveAppointmentDto dto);
        Task<Result<bool>> CancelAppointmentAsync(CancelAppointmentDto dto);

        Task<Result<List<Appointment>>> GetUpcomingAppointmentsForUserAsync(Guid userId);
        Task<Result<List<Appointment>>> GetPastAppointmentsForUserAsync(Guid userId);



    }
}
