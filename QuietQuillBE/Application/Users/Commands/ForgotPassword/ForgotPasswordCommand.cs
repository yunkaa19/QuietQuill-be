using Application.Abstractions.Messaging;

namespace Application.Users.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) : ICommand<string>;