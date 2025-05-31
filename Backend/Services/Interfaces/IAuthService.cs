using Backend.Entities;
using Backend.Models;
using Backend.Models.LoginAuthModels;

namespace Backend.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<Result<User>> AreValidCredentials(LoginRequestDto logInDto);
        public Task<Result<bool>> Register(RegisterRequestDto registerDto);
    }
}
