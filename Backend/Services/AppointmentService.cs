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
        public async Task<Result<List<Appointment>>> GetUpcomingAppointmentsForDoctorAsync(Guid doctorId)
        {
            var now = DateTime.UtcNow;

            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId && a.StartTime >= now)
                .OrderBy(a => a.StartTime)
                .ToListAsync();

            return Result<List<Appointment>>.Success(appointments);
        }
        public async Task<Result<Appointment>> ReserveAppointmentAsync(ReserveAppointmentDto dto)
        {
            // Check for overlapping appointments
            bool conflict = await _context.Appointments.AnyAsync(a =>
                a.DoctorId == dto.DoctorId &&
                a.Status == "Scheduled" &&
                a.StartTime < dto.EndTime &&
                dto.StartTime < a.EndTime
            );

            if (conflict)
                return Result<Appointment>.Failure("Time slot is already taken.");

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                DoctorId = dto.DoctorId,
                PatientId = dto.PatientId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = "Scheduled"
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Result<Appointment>.Success(appointment);
        }
        public async Task<Result<bool>> CancelAppointmentAsync(CancelAppointmentDto dto)
        {
            var appointment = await _context.Appointments.FindAsync(dto.AppointmentId);

            if (appointment == null)
                return Result<bool>.Failure("Appointment not found.");

            if (appointment.Status == "Cancelled")
                return Result<bool>.Failure("Appointment is already cancelled.");

            appointment.Status = "Cancelled";
            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        public async Task<Result<List<Appointment>>> GetUpcomingAppointmentsForUserAsync(Guid userId)
        {
            var now = DateTime.UtcNow;

            var appointments = await _context.Appointments
                .Where(a => a.PatientId == userId && a.StartTime >= now && a.Status == "Scheduled")
                .OrderBy(a => a.StartTime)
                .ToListAsync();

            return Result<List<Appointment>>.Success(appointments);
        }
        public async Task<Result<List<Appointment>>> GetPastAppointmentsForUserAsync(Guid userId)
        {
            var now = DateTime.UtcNow;

            var appointments = await _context.Appointments
                .Where(a => a.PatientId == userId && a.StartTime < now && a.Status == "Scheduled")
                .OrderByDescending(a => a.StartTime)
                .ToListAsync();

            return Result<List<Appointment>>.Success(appointments);
        }
    }
}
