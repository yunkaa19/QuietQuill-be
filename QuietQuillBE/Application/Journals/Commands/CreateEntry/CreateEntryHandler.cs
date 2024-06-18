using Application.Journals.DTOs;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Journals.Commands.CreateEntry;

public class CreateEntryHandler : IRequestHandler<CreateEntryCommand,JournalDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJournalEntryRepository _journalEntryRepository;
    private readonly IValidator<CreateEntryCommand> _validator;
    
    public CreateEntryHandler(IUnitOfWork unitOfWork, IJournalEntryRepository journalEntryRepository, IValidator<CreateEntryCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _journalEntryRepository = journalEntryRepository;
        _validator = validator;
    }
    
    
    
    public async Task<JournalDTO> Handle(CreateEntryCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        Guid userId = request.Entry.UserId;
        string date = request.Entry.Day + "/" + request.Entry.Month + "/" + request.Entry.Year;
        DateOnly entryDate = DateOnly.Parse(date);
        var entry = new JournalEntry(userId, request.Entry.Content, entryDate, request.Entry.Mood, request.Entry.Tags);
        
        bool isAdded = _journalEntryRepository.CreateJournalEntry(entry);
        if (!isAdded)
        {
            throw new Exception("Failed to add entry.");
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new JournalDTO
        {
            UserId = entry.UserId,
            Id = entry.Id,
            Content = entry.Content,
            Day = entry.EntryDate.Day.ToString(),
            Month = entry.EntryDate.Month.ToString(),
            Year = entry.EntryDate.Year.ToString(),
            Mood = entry.Mood,
            Tags = entry.Tags
        };
    }
}