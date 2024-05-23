using System.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Exceptions.Meditation;

namespace Application.Meditation.Queries.GetMeditation;

internal sealed class GetMeditationHandler : IQueryHandler<GetMeditationQuery, GetMeditationResponse>
{
    
    private readonly IDbConnection _dbConnection;
    
    public GetMeditationHandler(IDbConnection dbConnection) => _dbConnection = dbConnection;
    
    public Task<GetMeditationResponse> Handle(GetMeditationQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"SELECT * FROM ""Meditations"" WHERE ""Id"" = @MeditationId";
        var meditation =  _dbConnection.QueryFirstOrDefaultAsync<GetMeditationResponse>(
            sql,
            new { request.Id });
        if (meditation is null)
        {
            throw new MeditationNotFoundException(request.Id);
        }
        return meditation;
    }
}
