using FluentValidation;

namespace Application.Journals.Commands.DeleteEntry;

public class DeleteEntryValidator : AbstractValidator<DeleteEntryCommand>
{
    public DeleteEntryValidator()
    {
        RuleFor(x => x.journalId).NotEmpty();
    }
}