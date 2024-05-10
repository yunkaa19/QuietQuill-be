using Application.Abstractions.Messaging;

namespace Application.Journals.Commands.DeleteEntry;

public record DeleteEntryCommand(string journalId) : ICommand<bool>;