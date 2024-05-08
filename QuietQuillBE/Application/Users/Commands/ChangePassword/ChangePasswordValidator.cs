using Domain.Repositories;
using FluentValidation;
using Domain.Entities;

namespace Application.Users.Commands.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{

    public ChangePasswordValidator(IUserRepository userRepository)
    { 
        RuleFor(x => x.newPassword).NotEmpty();
        RuleFor(x => x.newPassword).MinimumLength(8);
        RuleFor(x => x.newPassword).Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter");
        RuleFor(x => x.newPassword).Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter");
        RuleFor(x => x.newPassword).Matches("[0-9]").WithMessage("Password must contain at least one number");
        RuleFor(x => x.newPassword).Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
    }
}