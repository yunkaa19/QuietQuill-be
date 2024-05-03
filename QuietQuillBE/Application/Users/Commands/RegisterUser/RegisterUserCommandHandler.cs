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
        var validationResult = _validator.ValidateAsync(request);
        if (!validationResult.Result.IsValid)
        {
            throw new ValidationException(validationResult.Result.Errors);
        }
        
        var IdentityID = await _authenticationService.RegisterAsync(request.Email, request.Password);
        var user = new User(Guid.NewGuid(),"null" , request.Email, request.Email, IdentityID);

        _userRepository.AddAsync(user);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return IdentityID;
    }
}