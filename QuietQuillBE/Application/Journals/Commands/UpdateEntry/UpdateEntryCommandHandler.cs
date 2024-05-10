using Application.Journals.DTOs;
using Domain.Abstraction;
using Domain.Exceptions.Base;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Journals.Commands.UpdateEntry;

public class UpdateEntryCommandHandler : IRequestHandler<UpdateEntryCommand, JournalDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly IValidator<UpdateEntryCommand> _validator;
    
    public UpdateEntryCommandHandler(IUnitOfWork unitOfWork, IJournalEntryRepository journalEntryRepository, IValidator<UpdateEntryCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _journalEntryRepository = journalEntryRepository;
        _validator = validator;
    }
    
    
    
    public async Task<JournalDTO> Handle(UpdateEntryCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var journalEntry =  _journalEntryRepository.GetJournalEntryById(request.Entry.Id);
        if (journalEntry == null)
        {
            throw new Exception("Journal Entry not found.");
        }

        _journalEntryRepository.UpdateJournalEntry(journalEntry);
        
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new JournalDTO
        {
            
        };
    }
}