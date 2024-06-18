using Domain.Abstraction;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Journals.Commands.DeleteEntry;

public class DeleteEntryHandler : IRequestHandler<DeleteEntryCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly IValidator<DeleteEntryCommand> _validator;
    
    public DeleteEntryHandler(IUnitOfWork unitOfWork, IJournalEntryRepository journalEntryRepository, IValidator<DeleteEntryCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _journalEntryRepository = journalEntryRepository;
        _validator = validator;
    }
    
    public async Task<bool> Handle(DeleteEntryCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var journalEntryDeleted = _journalEntryRepository.DeleteJournalEntry(request.journalId);
        if (!journalEntryDeleted)
        {
            return false;
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}