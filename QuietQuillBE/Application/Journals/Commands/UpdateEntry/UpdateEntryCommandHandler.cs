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

        // Update the properties of the journalEntry object with the new values from the request.Entry object
        journalEntry.Content = request.Entry.Content;
        journalEntry.EntryDate = DateOnly.Parse(request.Entry.Day + "/" + request.Entry.Month + "/" + request.Entry.Year);
        journalEntry.Mood = request.Entry.Mood;
        journalEntry.Tags = request.Entry.Tags;

        _journalEntryRepository.UpdateJournalEntry(journalEntry);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new JournalDTO
        {
            UserId = request.Entry.UserId,
            Id = request.Entry.Id,
            Content = request.Entry.Content,
            Day = request.Entry.Day,
            Month = request.Entry.Month,
            Year = request.Entry.Year,
            Mood = request.Entry.Mood,
            Tags = request.Entry.Tags
        };
    }
}