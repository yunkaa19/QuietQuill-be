using Application.Users.DTOs;

namespace Application.Users.Queries.GetUserById;

public sealed record UserResponse(FullUserDTO User);