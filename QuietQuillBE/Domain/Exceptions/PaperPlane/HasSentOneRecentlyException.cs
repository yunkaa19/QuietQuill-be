using Domain.Exceptions.Base;

namespace Domain.Exceptions.PaperPlane;

public sealed class HasSentOneRecentlyException : BadRequestException

{
    public HasSentOneRecentlyException(Guid userId)
        : base($"The user with id {userId} has sent a paperplane in the last 10 minutes.")
    {
    }
}