using Application.Journals.DTOs;

namespace Application.Journals.Commands.UpdateEntry;

public sealed record UpdateEntryCommandRequest(JournalDTO Entry);