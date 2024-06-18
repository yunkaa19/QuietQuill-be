using System.Runtime.CompilerServices;
using Application.Abstraction.Data;
using Application.Abstractions.Messaging;
using Application.Journals.DTOs;

[assembly: InternalsVisibleTo("ArchitectureTests")]
namespace Application.Journals.Queries.GetJournalsByMonth
{
    internal sealed class GetJournalsByMonthHandler : IQueryHandler<GetJournalsByMonthQuery, GetJournalsByMonthResponse>
    {
        private readonly IDbQueryExecutor _dbQueryExecutor;

        public GetJournalsByMonthHandler(IDbQueryExecutor dbQueryExecutor)
        {
            _dbQueryExecutor = dbQueryExecutor;
        }

        public async Task<GetJournalsByMonthResponse> Handle(GetJournalsByMonthQuery request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT 
                    Id, 
                    UserId, 
                    Content, 
                    EntryDate, 
                    DAY(EntryDate) AS Day,
                    MONTH(EntryDate) AS Month,
                    YEAR(EntryDate) AS Year,
                    Mood, 
                    Tags 
                FROM `journalentries`
                WHERE `UserId` = @UserId
                  AND (
                       (MONTH(`EntryDate`) = @Month AND YEAR(`EntryDate`) = @Year) OR
                       (MONTH(`EntryDate`) = CASE 
                                              WHEN @Month = 12 THEN 1 
                                              ELSE @Month + 1 
                                              END 
                        AND YEAR(`EntryDate`) = CASE 
                                                WHEN @Month = 12 THEN @Year + 1 
                                                ELSE @Year 
                                                END) OR
                       (MONTH(`EntryDate`) = CASE 
                                              WHEN @Month = 1 THEN 12 
                                              ELSE @Month - 1 
                                              END 
                        AND YEAR(`EntryDate`) = CASE 
                                                WHEN @Month = 1 THEN @Year - 1 
                                                ELSE @Year 
                                                END)
                      )";
                                    
            var journals = await _dbQueryExecutor.QueryAsync<JournalDTO>(
                sql,
                new { request.UserId, request.Month, request.Year });
            
            return new GetJournalsByMonthResponse(journals.ToArray());
        }
    }
}
