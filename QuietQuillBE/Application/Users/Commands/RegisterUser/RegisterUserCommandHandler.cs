using Application.Abstraction.Authentication;
using Application.Users.DTOs;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Repositories;
using MediatR;


namespace Application.Users.Commands.Handlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{


    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    
    public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IAuthenticationService authService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _authenticationService = authService;
    }
    
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var IdentityID = await _authenticationService.RegisterAsync(request.Email, request.Password);
        var user = new User(Guid.NewGuid(), request.Username, request.Email, request.Email, IdentityID);

        _userRepository.AddAsync(user);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);


    }
}