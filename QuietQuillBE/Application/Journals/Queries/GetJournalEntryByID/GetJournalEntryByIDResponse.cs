using Application.Journals.DTOs;

namespace Application.Journals.Queries.GetJournalEntryByID;

public sealed record GetJournalEntryByIDResponse(JournalDTO Entry);