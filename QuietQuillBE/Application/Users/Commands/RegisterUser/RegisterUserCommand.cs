using Application.Users.DTOs;
using MediatR;

namespace Application.Users.Commands;

public record RegisterUserCommand(string Email, string Password, string Username) : IRequest
{
    
}

