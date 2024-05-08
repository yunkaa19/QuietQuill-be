using Application.Abstraction.Authentication;
using FirebaseAdmin.Auth;

namespace Infrastructure.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
    
    public async Task<string> RegisterAsync(string email, string password)
    {
        
            var userArgs = new UserRecordArgs
            {
                Email = email,
                Password = password
            };

            var userRecord= await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

            return userRecord.Uid;
    }

    public async Task<string> ResetPasswordAsync(string email)
    {
        // This is generating a password reset link. This link can be sent to the user's email, and when the user clicks on it, they can reset their password.
        // I did not implement the email sending part in this as I don't have an SMTP server to send emails.
        // Maybe this can be implemented in the future.
        // Until then I will be using the frontend implementation of this feature.
        var resetLink = await FirebaseAuth.DefaultInstance.GeneratePasswordResetLinkAsync(email);
        return resetLink;
    }

    public async Task<string> ChangePasswordAsync(string email, string oldPassword, string newPassword)
    {
        var user = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);

        var userArgs = new UserRecordArgs
        {
            Uid = user.Uid,
            Email = email,
            Password = newPassword
        };
        await FirebaseAuth.DefaultInstance.UpdateUserAsync(userArgs);
        return user.Email;
    }
    
}