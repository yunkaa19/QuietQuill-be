using Application.Abstractions.Messaging;

namespace Application.Users.Commands.LoginUser;

public record LoginUserCommand(string Email, string Password) : ICommand<string>;