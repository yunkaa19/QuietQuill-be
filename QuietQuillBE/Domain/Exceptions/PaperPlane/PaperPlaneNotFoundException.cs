using Domain.Exceptions.Base;

namespace Domain.Exceptions.PaperPlane;

public sealed class PaperPlaneNotFoundException : NotFoundException
{
    public PaperPlaneNotFoundException(int id)
        : base($"The paper plane with the identifier {id} was not found.")
    {
    }
    public PaperPlaneNotFoundException(Guid userId)
        : base($"The user with the identifier {userId} has not sent a paper plane recently.")
    {
    }
    public PaperPlaneNotFoundException()
        : base("No paper planes have been sent yet.")
    {
    }
}