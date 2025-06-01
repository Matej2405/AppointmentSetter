// Backend/Services/AvailabilityService.cs
using Backend.Data;
using Backend.Entities;
using Backend.Models;
using Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AvailabilityService : IAvailabilityService
{
    private readonly AppDbContext _context;

    public AvailabilityService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<AvailabilityDto>> CreateAvailabilityAsync(AvailabilityDto dto)
    {
        var availability = new Availability
        {
            Id = Guid.NewGuid(),
            DoctorId = dto.DoctorId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Notes = dto.Notes
        };

        // Check for overlap before saving
        bool overlaps = await _context.Availabilities
            .AnyAsync(a =>
                a.DoctorId == dto.DoctorId &&
               
                a.Id != dto.Id &&
                a.StartTime < dto.EndTime &&
                dto.StartTime < a.EndTime
            );

        if (overlaps)
        {
            return Result<AvailabilityDto>.Failure("This time slot overlaps with an existing availability.");
        }


        _context.Availabilities.Add(availability);
        await _context.SaveChangesAsync();

        dto.Id = availability.Id;
        return Result<AvailabilityDto>.Success(dto);
    }

    public async Task<Result<AvailabilityDto>> UpdateAvailabilityAsync(AvailabilityDto dto)
    {
        var availability = await _context.Availabilities.FindAsync(dto.Id);
        if (availability == null) return Result<AvailabilityDto>.Failure("Not found.");

        availability.StartTime = dto.StartTime;
        availability.EndTime = dto.EndTime;
        availability.Notes = dto.Notes;

        await _context.SaveChangesAsync();
        return Result<AvailabilityDto>.Success(dto);
    }

    public async Task<Result<bool>> DeleteAvailabilityAsync(Guid id)
    {
        var availability = await _context.Availabilities.FindAsync(id);
        if (availability == null) return Result<bool>.Failure("Not found.");

        _context.Availabilities.Remove(availability);
        await _context.SaveChangesAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<AvailabilityDto>> GetAvailabilityByIdAsync(Guid id)
    {
        var availability = await _context.Availabilities.FindAsync(id);
        if (availability == null) return Result<AvailabilityDto>.Failure("Not found.");

        var dto = new AvailabilityDto
        {
            Id = availability.Id,
            DoctorId = availability.DoctorId,
            StartTime = availability.StartTime,
            EndTime = availability.EndTime,
            Notes = availability.Notes
        };

        return Result<AvailabilityDto>.Success(dto);
    }

    public async Task<Result<List<AvailabilityDto>>> GetDoctorAvailabilitiesAsync(Guid doctorId)
    {
        var availabilities = await _context.Availabilities
            .Where(a => a.DoctorId == doctorId)
            .Select(a => new AvailabilityDto
            {
                Id = a.Id,
                DoctorId = a.DoctorId,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Notes = a.Notes
            })
            .ToListAsync();

        return Result<List<AvailabilityDto>>.Success(availabilities);
    }
}
