using Application.Abstraction.Authentication;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
{


    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<RegisterUserCommand> _validator;
    
    
    public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IAuthenticationService authService, IValidator<RegisterUserCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _authenticationService = authService;
        _validator = validator;
    }
    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Validate the request
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Create the user entity with a placeholder for IdentityID
        var user = new User(Guid.NewGuid(), "null", request.Email, request.Email, null);

        // Check if user already exists
        if (await _userRepository.UserExists(user.Email))
        {
            throw new Exception("User already exists.");
        }

        // Register the user in the authentication service and get the IdentityID
        var identityID = await _authenticationService.RegisterAsync(request.Email, request.Password);

        // Update the user entity with the IdentityID
        user.UpdateIdentityId(identityID);

        // Save the user entity to the repository
        await _userRepository.AddAsync(user);

        // Commit the changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return identityID;
    }

}