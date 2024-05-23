using System.Text.Json;
using Application.Abstraction.Authentication;
using Domain.Abstraction;
using Domain.Entities;
using Domain.Exceptions.Base;
using Domain.Exceptions.Users;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Users.Commands.LoginUser;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJWTProvider _jwtProvider;
    private readonly IValidator<LoginUserCommand> _validator;
    private readonly IUserRepository _userRepository;
    
    
    public LoginUserCommandHandler(IJWTProvider jwtProvider, IUnitOfWork unitOfWork, IValidator<LoginUserCommand> validator, IUserRepository userRepository)
    {
        _jwtProvider = jwtProvider;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _userRepository = userRepository;
    }
    
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        User user = await _userRepository.GetUserByEmail(request.Email);
        
        if (user == null)
        {
            throw new UserNotFoundException(new Guid());
        }
        string token = await _jwtProvider.GetForCredentialsAsync(request.Email, request.Password);
        
        // I need to return a JSON with the ID, Email, Role and Token
        
        return JsonSerializer.Serialize(new
        {
            Token = token,
            user.UserId,
            user.Email,
            user.Role
        });
    }
}