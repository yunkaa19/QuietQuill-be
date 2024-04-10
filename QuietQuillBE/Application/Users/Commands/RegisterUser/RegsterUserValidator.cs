using FluentValidation;

namespace Application.Users.Commands.CreateUser;

public class RegsterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegsterUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
    
}