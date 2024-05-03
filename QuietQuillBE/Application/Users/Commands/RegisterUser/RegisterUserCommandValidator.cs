using Domain.Repositories;
using FluentValidation;

namespace Application.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required");
        RuleFor(c => c.Email).MustAsync(async (email, _) =>
        {
            var exists = await userRepository.UserExists(email);
            return !exists;
        }).WithMessage("The email must be unique");
        
        
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Password).MinimumLength(8);
        RuleFor(x => x.Password).Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter");
        RuleFor(x => x.Password).Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter");
        RuleFor(x => x.Password).Matches("[0-9]").WithMessage("Password must contain at least one number");
        RuleFor(x => x.Password).Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
    }
    
}