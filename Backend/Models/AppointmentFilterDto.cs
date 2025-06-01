namespace Backend.Models
{
    public class AppointmentFilterDto
    {
        public Guid? DoctorId { get; set; }
        public Guid? PatientId { get; set; }
        public DateTime? Date { get; set; }
        public string? Status { get; set; } // Optional: "Scheduled", "Completed", "Cancelled", etc.
    }
}
