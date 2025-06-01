namespace Backend.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }   
        public Guid DoctorId { get; set; }
        public User Doctor { get; set; }

        public Guid PatientId { get; set; }
        public User Patient { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string? Status { get; set; } // e.g., "Scheduled", "Completed", "Cancelled"
    }
}
