using Application.Abstractions.Messaging;
using Application.Users.DTOs;

namespace Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<FullUserDTO>;