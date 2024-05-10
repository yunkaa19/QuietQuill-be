using FluentValidation;

namespace Application.Journals.Commands.UpdateEntry;

public class UpdateEntryCommandValidator : AbstractValidator<UpdateEntryCommand>
{
    public UpdateEntryCommandValidator()
    {
        RuleFor(x => x.Entry).NotNull();
        RuleFor(x => x.Entry.Content).NotEmpty();
        RuleFor(x => x.Entry.Day).NotEmpty();
        RuleFor(x => x.Entry.Month).NotEmpty();
        RuleFor(x => x.Entry.Year).NotEmpty();
    }
}