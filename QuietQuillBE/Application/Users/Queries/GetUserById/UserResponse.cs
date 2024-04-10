namespace Application.Users.Queries.GetUserById;

public sealed record UserResponse(Guid UserId, string Email);