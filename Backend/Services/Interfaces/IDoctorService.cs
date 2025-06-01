using Backend.Entities;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<Result<bool>> UpdateDoctorSpecializationsAsync(DoctorSpecializationDto dto);

        Task<Result<List<Specializations>>> GetDoctorSpecializationsAsync(Guid doctorId);

        Task<Result<List<UserDto>>> SearchDoctorsAsync(DoctorFilterDto filter);

    }
}
