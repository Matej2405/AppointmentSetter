namespace Backend.Models
{
    public class UserWithRolesDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }    
        public List<string> Roles { get; set; } = new List<string>();
    }
}
