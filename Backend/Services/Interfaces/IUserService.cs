using Backend.Entities;
using Backend.Models;
namespace Backend.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Result<User>> GetUserByEmailAsync(string email);

        public Task<Result<User>> GetUserByIdAsync(Guid id);
        public Task<Result<UserDto>> GetUserDtoByEmail(string email);
        public Task<Result<bool>> UpdateUserByIdAsync(User user);
    }
}
