using System.Data;
using Application.Abstractions.Messaging;
using Application.Journals.DTOs;
using Dapper;

namespace Application.Journals.Queries.GetJournalsByMonth;

internal sealed class GetJournalsByMonthHandler : IQueryHandler<GetJournalsByMonthQuery, GetJournalsByMonthResponse>
{
    
    private readonly IDbConnection _dbConnection;
    
    public GetJournalsByMonthHandler(IDbConnection dbConnection) => _dbConnection = dbConnection;
    
    public async Task<GetJournalsByMonthResponse> Handle(GetJournalsByMonthQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"SELECT * FROM ""Journals"" WHERE ""UserId"" = @UserId AND MONTH(""EntryDate"") = @Month AND YEAR(""EntryDate"") = @Year";
        
        var journals = await _dbConnection.QueryAsync<JournalDTO>(
            sql,
            new { request.UserId, request.Month, request.Year });

        return new GetJournalsByMonthResponse(journals.ToArray());
    }
}
