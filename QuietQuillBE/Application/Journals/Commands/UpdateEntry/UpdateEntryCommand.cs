using Application.Abstractions.Messaging;
using Application.Journals.DTOs;

namespace Application.Journals.Commands.UpdateEntry;

public record UpdateEntryCommand(JournalDTO Entry) : ICommand<JournalDTO>;