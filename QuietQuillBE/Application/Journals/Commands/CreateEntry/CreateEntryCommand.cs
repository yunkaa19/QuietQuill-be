using Application.Abstractions.Messaging;
using Application.Journals.DTOs;

namespace Application.Journals.Commands.CreateEntry;

public record CreateEntryCommand(JournalDTO Entry) : ICommand<JournalDTO>;