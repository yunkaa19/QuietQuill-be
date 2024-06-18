
using Application.Abstractions.Messaging;

namespace Application.Journals.Queries.GetJournalsByMonth;

public sealed record GetJournalsByMonthQuery(String UserId, string Month, string Year) : IQuery<GetJournalsByMonthResponse>;