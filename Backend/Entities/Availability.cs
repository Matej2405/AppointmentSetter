using Backend.Entities;

public class Availability
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public User Doctor { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    // Recurrence
    public bool IsRecurring { get; set; }
    public string? RecurrenceRule { get; set; } 
    public string? Notes { get; set; }
}
