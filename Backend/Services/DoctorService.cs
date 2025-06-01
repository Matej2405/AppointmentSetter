using Backend.Entities;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Services.Interfaces;



namespace Backend.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext _context;

        public DoctorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<bool>> UpdateDoctorSpecializationsAsync(DoctorSpecializationDto dto)
        {
            var doctor = await _context.Users
                .Include(u => u.UserSpecializations)
                .FirstOrDefaultAsync(u => u.Id == dto.DoctorId);

            if (doctor == null) return Result<bool>.Failure("Doctor not found.");

            doctor.UserSpecializations.Clear();

            foreach (var specId in dto.SpecializationIds)
            {
                doctor.UserSpecializations.Add(new UserSpecialization
                {
                    UserId = dto.DoctorId,
                    SpecializationId = specId
                });
            }

            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<List<Specializations>>> GetDoctorSpecializationsAsync(Guid doctorId)
        {
            var specIds = await _context.UserSpecializations
                .Where(us => us.UserId == doctorId)
                .Select(us => us.SpecializationId)
                .ToListAsync();

            var specializations = await _context.Specializations
                .Where(s => specIds.Contains(s.Id))
                .ToListAsync();

            return Result<List<Specializations>>.Success(specializations);
        }
        public async Task<Result<List<UserDto>>> SearchDoctorsAsync(DoctorFilterDto filter)
        {
            // Only users who are doctors
            var doctorRoleId = await _context.Roles
                .Where(r => r.Name == "Doctor")
                .Select(r => r.Id)
                .FirstAsync();

            var query = _context.Users
                .Where(u => u.UserRoles.Any(ur => ur.RoleId == doctorRoleId));

            // Filter by specialization
            if (filter.SpecializationId.HasValue)
                query = query.Where(u =>
                    u.UserSpecializations.Any(us => us.SpecializationId == filter.SpecializationId.Value));

            // Filter by location
            if (!string.IsNullOrWhiteSpace(filter.Location))
                query = query.Where(u => u.Location == filter.Location);

            // Filter by availability
            if (filter.AvailableFrom.HasValue && filter.AvailableTo.HasValue)
            {
                query = query.Where(u =>
                    u.Availabilities.Any(a =>
                        a.StartTime <= filter.AvailableFrom.Value &&
                        a.EndTime >= filter.AvailableTo.Value
                    ));
            }

            var doctors = await query.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email,
                // Add more UserDto properties if needed
            }).ToListAsync();

            return Result<List<UserDto>>.Success(doctors);
        }

    }
}
