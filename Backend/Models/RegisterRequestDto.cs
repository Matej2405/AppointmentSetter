namespace Backend.Models
{
    public record RegisterRequestDto(string Email, string Username, string Password, string RepeatedPassword);
}