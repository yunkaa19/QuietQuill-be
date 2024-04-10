using System.Data;
using Application.Abstractions.Messaging;
using Domain.Exceptions.Users;
using Dapper;

namespace Application.Users.Queries.GetUserById;

internal sealed class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IDbConnection _dbConnection;
    
    public GetUserByIdHandler(IDbConnection dbConnection) => _dbConnection = dbConnection;
    
    public async Task<UserResponse> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        const string sql = @"SELECT * FROM ""Users"" WHERE ""Id"" = @UserId";
        
        var user = await _dbConnection.QueryFirstOrDefaultAsync<UserResponse>(
            sql,
            new { request.UserId });
        
        if (user is null)
        {
            throw new UserNotFoundException(request.UserId);
        }
        
        return user;
    }
}