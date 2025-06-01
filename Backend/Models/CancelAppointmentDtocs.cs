namespace Backend.Models
{
    public class CancelAppointmentDto
    {
        public Guid AppointmentId { get; set; }
        public string? Reason { get; set; }
    }
}
