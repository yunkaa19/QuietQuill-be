using System.Runtime.CompilerServices;
using Application.Abstraction.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Exceptions.Meditation;

[assembly: InternalsVisibleTo("ArchitectureTests")]
namespace Application.Meditation.Queries.GetMeditation
{
    internal sealed class GetMeditationHandler : IQueryHandler<GetMeditationQuery, GetMeditationResponse>
    {
        private readonly IDbQueryExecutor _dbQueryExecutor;

        public GetMeditationHandler(IDbQueryExecutor dbQueryExecutor)
        {
            _dbQueryExecutor = dbQueryExecutor;
        }

        public async Task<GetMeditationResponse> Handle(GetMeditationQuery request, CancellationToken cancellationToken)
        {
            const string sql = @"SELECT * FROM ""Meditations"" WHERE ""Id"" = @MeditationId";
            
            var meditation = await _dbQueryExecutor.QueryFirstOrDefaultAsync<GetMeditationResponse>(
                sql,
                new { MeditationId = request.Id });

            if (meditation is null)
            {
                throw new MeditationNotFoundException(request.Id);
            }

            return meditation;
        }
    }
}