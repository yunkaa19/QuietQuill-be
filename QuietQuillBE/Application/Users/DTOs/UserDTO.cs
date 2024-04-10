namespace Application.Users.DTOs;

public record UserDTO
{
    public Guid UserId { get; init; }
    public string Username { get; init; }
    public string Email { get; init; }
    public string Role { get; init; }
};
