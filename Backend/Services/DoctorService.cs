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
    }
}
