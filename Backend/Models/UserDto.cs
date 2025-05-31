namespace Backend.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }    
        public string Email { get; set; }
        public int Xp { get; set; }
        public int Coins { get; set; }  
    }
}
