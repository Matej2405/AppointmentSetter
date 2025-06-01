namespace Backend.Models
{
    public class DoctorFilterDto
    {
        public Guid? SpecializationId { get; set; }
        public string? Location { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableTo { get; set; }

    }

}
