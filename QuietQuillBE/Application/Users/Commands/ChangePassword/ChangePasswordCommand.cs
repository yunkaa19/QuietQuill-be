using Application.Abstractions.Messaging;
using MediatR;

namespace Application.Users.Commands.ChangePassword;

public record ChangePasswordCommand(string Email, string oldPassword, string newPassword) : ICommand<string>;