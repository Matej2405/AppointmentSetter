using System.Text.RegularExpressions;
using Backend.Entities;
using Backend.Models;
using Backend.Models.LoginAuthModels;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Backend.Services
{
    public class AuthService(UserManager<User> userManager, SignInManager<User> signinManager) : IAuthService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signinManager = signinManager;

        private bool IsEmail(string input)
        {
            var emailCheckRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            return Regex.IsMatch(input, emailCheckRegex);
        }
        
        public async Task<Result<User>> AreValidCredentials(LoginRequestDto logInDto)
        {
            try
            {
                if (IsEmail(logInDto.EmailUsername))
                {
                    var existingUser = await _userManager.FindByEmailAsync(logInDto.EmailUsername);
                    if (existingUser == null) return Result<User>.Failure("No user found with specified Email.");

                    if (!existingUser.IsActive)
                        return Result<User>.Failure("Account is deactivated.");


                    var result = await _signinManager.CheckPasswordSignInAsync(existingUser, logInDto.Password, false);

                    if (result.Succeeded) return Result<User>.Success(existingUser);
                    else return Result<User>.Failure("Incorrect Password.");
                }
                else
                {
                    var existingUser = await _userManager.FindByNameAsync(logInDto.EmailUsername);
                    if (existingUser == null) return Result<User>.Failure("No user found with specified Username.");


                    if (!existingUser.IsActive)
                        return Result<User>.Failure("Account is deactivated.");

                    var result = await _signinManager.CheckPasswordSignInAsync(existingUser, logInDto.Password, false);

                    if (result.Succeeded) return Result<User>.Success(existingUser);
                    else return Result<User>.Failure("Incorrect Password");
                }
            }
            catch(Exception e)
            {
                return Result<User>.Failure(e.Message);
            }
        }

        public async Task<Result<bool>> Register(RegisterRequestDto registerDto)
        {
            // Email syntax check should be done on the frontend to avoid overhead.
            // No need to check for SQL injections because EF is used.

            var existingUser = await _userManager.FindByNameAsync(registerDto.Username);
            if (existingUser != null) return Result<bool>.Failure("Username/Email already taken.");

            existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null) return Result<bool>.Failure("Username/Email already taken.");

            if(registerDto.Password != registerDto.RepeatedPassword)
            {
                return Result<bool>.Failure("Passwords don't match.");
            }

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                // Log the errors for debugging purposes.
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<bool>.Failure($"User creation failed: {errors}");
            }

            return Result<bool>.Success(true);
        }
    }
}
