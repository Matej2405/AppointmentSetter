namespace Backend.Entities
{
    public class UserSpecialization
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid SpecializationId { get; set; }
        public Specializations Specialization { get; set; } = null!;
    }

}

    