﻿using Application.Abstractions.Messaging;
using MediatR;

namespace Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Password) : ICommand<string>;

