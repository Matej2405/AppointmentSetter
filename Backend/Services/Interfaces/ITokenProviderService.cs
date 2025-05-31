using Backend.Entities;

namespace Backend.Services.Interfaces
{
    public interface ITokenProviderService
    {
        public string GenerateToken(User user);
    }
}
