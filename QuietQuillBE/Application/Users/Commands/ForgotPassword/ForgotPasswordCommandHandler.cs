using Application.Abstraction.Authentication;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using MediatR;


namespace Application.Users.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<ForgotPasswordCommand> _validator;
    
    
    public ForgotPasswordCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IAuthenticationService authService, IValidator<ForgotPasswordCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _authenticationService = authService;
        _validator = validator;
    }
    
    public async Task<string> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}