namespace Backend.Models
{
    public class DoctorSpecializationDto
    {
        public Guid DoctorId { get; set; }
        public List<Guid> SpecializationIds{ get; set; }
    }
}
