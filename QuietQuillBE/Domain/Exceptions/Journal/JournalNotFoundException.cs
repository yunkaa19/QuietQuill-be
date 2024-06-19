using Domain.Exceptions.Base;

namespace Domain.Exceptions.Journal;

public sealed class JournalNotFoundException : NotFoundException
{
    public JournalNotFoundException(string Id)
        : base($"The journal with the identifier {Id} was not found.")
    {
    }
}