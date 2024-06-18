using Application.Abstraction.Data;
using Application.Abstractions.Messaging;
using Domain.Exceptions.Users;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ArchitectureTests")]

namespace Application.Users.Queries.GetUserById
{
    internal sealed class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IDbQueryExecutor _dbQueryExecutor;

        public GetUserByIdHandler(IDbQueryExecutor dbQueryExecutor)
        {
            _dbQueryExecutor = dbQueryExecutor;
        }

        public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            const string sql = @"SELECT * FROM ""Users"" WHERE ""Id"" = @UserId";

            var user = await _dbQueryExecutor.QueryFirstOrDefaultAsync<UserResponse>(
                sql,
                new { UserId = request.UserId });

            if (user is null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            return user;
        }
    }
}