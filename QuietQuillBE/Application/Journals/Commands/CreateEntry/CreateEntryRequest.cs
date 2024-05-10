using Application.Journals.DTOs;

namespace Application.Journals.Commands.CreateEntry;

public sealed record CreateEntryRequest(JournalDTO Entry);