using FluentValidation;

namespace Application.Users.Commands.RegisterUser;

public class RegsterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegsterUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
    
}