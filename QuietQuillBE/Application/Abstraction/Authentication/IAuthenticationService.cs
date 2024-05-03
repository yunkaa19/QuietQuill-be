namespace Application.Abstraction.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(string email, string password);
    Task<string> ResetPasswordAsync(string email);
    Task<string> ChangePasswordAsync(string email, string oldPassword, string newPassword);
}