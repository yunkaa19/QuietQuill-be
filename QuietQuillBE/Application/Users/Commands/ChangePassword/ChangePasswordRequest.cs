namespace Application.Users.Commands.ChangePassword;

public sealed record ChangePasswordRequest(string Email, string OldPassword, string NewPassword);
