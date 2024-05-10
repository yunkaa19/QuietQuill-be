using FluentValidation;

namespace Application.Journals.Commands.CreateEntry;

public class CreateEntryValidator : AbstractValidator<CreateEntryCommand>
{
    public CreateEntryValidator()
    {
        RuleFor(x => x.Entry.Content).NotEmpty();
        RuleFor(x => x.Entry.Day).NotEmpty();
        RuleFor(x => x.Entry.Month).NotEmpty();
        RuleFor(x => x.Entry.Year).NotEmpty();
        
    }
}