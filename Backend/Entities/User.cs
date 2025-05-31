using Microsoft.AspNetCore.Identity;

namespace Backend.Entities;

public class User : IdentityUser<Guid>
{
    public int XP { get; set; } = 0;
    public int Coins { get; set; } = 0;
    public int Health { get; set; } = 100;
    // Todo -> .ignore all unused
    // props from this class in
    // OnModelCreating().
}