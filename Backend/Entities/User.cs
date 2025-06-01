using Microsoft.AspNetCore.Identity;

namespace Backend.Entities;

public class User : IdentityUser<Guid>
{
    public bool IsActive { get; set; } = true;  
    public ICollection<UserSpecialization> UserSpecializations { get; set; } = new List<UserSpecialization>();
    public ICollection<Appointment> AppointmentsAsDoctor { get; set; } = new List<Appointment>();
    public ICollection<Appointment> AppointmentsAsPatient { get; set; } = new List<Appointment>();
}