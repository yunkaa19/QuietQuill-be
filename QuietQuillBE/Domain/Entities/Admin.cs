namespace Domain.Entities;

public class Admin : User
{
    private Admin()
    {

    }

    public Admin(Guid id, string username, string passwordHash, string email, string identityId)
            : base(id, username, passwordHash, email, identityId)
        {
            UserId = id;
            Username = username;
            PasswordHash = passwordHash;
            Email = email;
            base.IdentityId = identityId;


            Role = "Admin";
        }

    // Admin-specific methods
}