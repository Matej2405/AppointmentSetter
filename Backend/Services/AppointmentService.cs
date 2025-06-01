using Backend.Data;
using Backend.Entities;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }

        // ...other methods...

        public async Task<Result<List<Appointment>>> GetFilteredAppointmentsAsync(AppointmentFilterDto filter)
        {
            try
            {
                var query = _context.Appointments.AsQueryable();

                if (filter.DoctorId.HasValue)
                    query = query.Where(a => a.DoctorId == filter.DoctorId.Value);

                if (filter.PatientId.HasValue)
                    query = query.Where(a => a.PatientId == filter.PatientId.Value);

                if (filter.Date.HasValue)
                    query = query.Where(a => a.StartTime.Date == filter.Date.Value.Date);

                if (!string.IsNullOrEmpty(filter.Status))
                    query = query.Where(a => a.Status == filter.Status);

                var appointments = await query.ToListAsync();
                return Result<List<Appointment>>.Success(appointments);
            }
            catch (Exception ex)
            {
                return Result<List<Appointment>>.Failure(ex.Message);
            }
        }
        public async Task<Result<List<Appointment>>> GetAppointmentsAsync(AppointmentFilterDto filter)
        {
            try
            {
                var query = _context.Appointments.AsQueryable();

                if (filter.DoctorId.HasValue)
                    query = query.Where(a => a.DoctorId == filter.DoctorId.Value);

                if (filter.PatientId.HasValue)
                    query = query.Where(a => a.PatientId == filter.PatientId.Value);

                if (filter.Date.HasValue)
                    query = query.Where(a => a.StartTime.Date == filter.Date.Value.Date);

                if (!string.IsNullOrEmpty(filter.Status))
                    query = query.Where(a => a.Status == filter.Status);

                var appointments = await query.ToListAsync();
                return Result<List<Appointment>>.Success(appointments);
            }
            catch (Exception ex)
            {
                return Result<List<Appointment>>.Failure(ex.Message);
            }
        }
    }
}
