using Domain.Exceptions.Base;

namespace Domain.Exceptions.Meditation;

public sealed class MeditationNotFoundException : NotFoundException
{
    public MeditationNotFoundException(string Id)
        : base($"The session with the identifier {Id} was not found.")
    {
    }
}