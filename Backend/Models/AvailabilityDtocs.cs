
namespace Backend.Models
{
    public class AvailabilityDto
    {
        public Guid? Id { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Notes { get; set; }
        public bool IsRecurring { get; set; }
        public string? RecurrenceRule { get; set; }

    }
}
