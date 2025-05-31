using Backend.Entities;

namespace Backend.Entities
{
    public class Specializations
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<UserSpecialization> UserSpecializations = new List<UserSpecialization>();
    }
}
