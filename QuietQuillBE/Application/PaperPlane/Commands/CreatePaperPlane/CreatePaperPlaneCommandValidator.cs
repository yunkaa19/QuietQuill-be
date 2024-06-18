using FluentValidation;

namespace Application.PaperPlane.Commands.CreatePaperPlane;

public class CreatePaperPlaneCommandValidator : AbstractValidator<CreatePaperPlaneCommand>
{
    public CreatePaperPlaneCommandValidator()
    {
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}