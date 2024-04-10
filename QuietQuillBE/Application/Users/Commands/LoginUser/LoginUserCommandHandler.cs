using Application.Abstraction.Authentication;
using MediatR;

namespace Application.Users.Commands.LoginUser;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IJWTProvider _jwtProvider;
    
    public LoginUserCommandHandler(IJWTProvider jwtProvider)
    {
        _jwtProvider = jwtProvider;
    }
    
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}