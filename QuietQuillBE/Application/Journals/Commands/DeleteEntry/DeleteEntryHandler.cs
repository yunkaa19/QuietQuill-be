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
        
        var journalEntry =  _journalEntryRepository.DeleteJournalEntry(request.journalId);
        if (journalEntry)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return journalEntry;            
        }
        
        
        return journalEntry;
    }
}