using Application.Abstraction.Authentication;
using Application.Users.Commands.RegisterUser;
using Domain.Abstraction;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, string>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<ChangePasswordCommand> _validator;
    
    public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IAuthenticationService authService, IValidator<ChangePasswordCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _authenticationService = authService;
        _validator = validator;
    }


    public async Task<string> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var user = await _userRepository.GetUserByEmail(request.Email);
        if (user == null)
        {
            throw new Exception("User not found.");
        }
        var identityID = await _authenticationService.ChangePasswordAsync(user.Email, request.oldPassword, request.newPassword);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return identityID;
    }
}