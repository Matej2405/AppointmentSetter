using Backend.Entities;
using Backend.Models;
using Backend.Models.Leaderboard;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;
namespace Backend.Services
{
    public class UserService(UserManager<User> userManager) : IUserService
    {
        private readonly UserManager<User> _userManager = userManager;
        public async Task<Result<User>> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if(user == null)
                {
                    return Result<User>.Failure("User not found by email");
                }

                return Result<User>.Success(user);
            }
            catch(Exception e)
            {
                return Result<User>.Failure(e.Message);
            }
        }

        public async Task<Result<User>> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if(user == null)
                {
                    return Result<User>.Failure("User not found by id");
                }

                return Result<User>.Success(user);
            }
            catch(Exception e)
            {
                return Result<User>.Failure(e.Message);
            }
        }

        public async Task<Result<LeaderboardDTO>> GetLeaderboardAsync()
        {
            try
            {
                var users = await _userManager.Users
                    .OrderByDescending(u => u.XP) 
                    .Select(u => new LeaderboardEntryDTO
                    {
                        UserId = u.Id,
                        Username = u.UserName,
                        Xp = u.XP,
                        Coins = u.Coins
                    })
                    .ToListAsync();

                if (!users.Any())
                {
                    return Result<LeaderboardDTO>.Failure("No leaderboard data available.");
                }

                var leaderboard = new LeaderboardDTO { Entries = users };

                return Result<LeaderboardDTO>.Success(leaderboard);
            }
            catch (Exception e)
            {
                return Result<LeaderboardDTO>.Failure($"Failed to fetch leaderboard: {e.Message}");
            }
        }

        public async Task<Result<UserDto>> GetUserDtoByEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    return Result<UserDto>.Failure("User not found by email");
                }

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                                    // Ensure User entity has this property or adjust accordingly
                    Xp = user.XP,       // Assuming entity property is 'XP'
                    Coins = user.Coins
                };

                return Result<UserDto>.Success(userDto);
            }
            catch (Exception e)
            {
                return Result<UserDto>.Failure(e.Message);
            }
        }

        public async Task<Result<bool>> UpdateUserByIdAsync(User user)
        {
            try
            {
                var result = await _userManager.UpdateAsync(user);
                
                if(!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Result<bool>.Failure($"Failed to update User:\n{errors}");
                }
                
                return Result<bool>.Success(true);
            }
            catch (Exception e)
            {
                return Result<bool>.Failure(e.Message);
            }
        }
    }
}
