namespace Application.Abstraction.Authentication;

public interface IJWTProvider
{
    Task<string> GetForCredentialsAsync(string email, string password);
}