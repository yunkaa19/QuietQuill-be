using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstraction.Data;
using Application.Abstractions.Messaging;
using Application.Journals.DTOs;


[assembly: InternalsVisibleTo("ArchitectureTests")]
namespace Application.Journals.Queries.GetJournalEntryByID
{
    internal sealed class GetJournalEntryByIDHandler : IQueryHandler<GetJournalEntryByIDQuery, GetJournalEntryByIDResponse>
    {
        private readonly IDbQueryExecutor _dbQueryExecutor;

        public GetJournalEntryByIDHandler(IDbQueryExecutor dbQueryExecutor)
        {
            _dbQueryExecutor = dbQueryExecutor;
        }

        public async Task<GetJournalEntryByIDResponse> Handle(GetJournalEntryByIDQuery request, CancellationToken cancellationToken)
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
                  AND `Id` = @EntryID";
                                    
            var journal = await _dbQueryExecutor.QueryFirstOrDefaultAsync<JournalDTO>(
                sql,
                new { request.UserId, request.EntryID });

            return new GetJournalEntryByIDResponse(journal);
        }
    }
}