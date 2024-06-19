using Domain.Exceptions.Base;

namespace Domain.Exceptions.Users;

public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid userId)
        : base($"The user with the identifier {userId} was not found.")
    {
    }

    public UserNotFoundException(string Email) : base($"User with email {Email} was not found.")
    {
    }
}