using Application._Example.Queries.ExampleQuery;
using Application.Abstractions.Messaging;

namespace Application.Journals.Queries.GetJournalsByMonth;

public sealed record GetJournalsByMonthQuery(string UserId, string Day, string Month, string Year) : IQuery<GetJournalsByMonthResponse>;