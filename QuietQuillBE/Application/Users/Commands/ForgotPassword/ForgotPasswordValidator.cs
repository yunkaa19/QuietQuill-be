using Domain.Repositories;
using FluentValidation;

namespace Application.Users.Commands.ForgotPassword;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
    }
}