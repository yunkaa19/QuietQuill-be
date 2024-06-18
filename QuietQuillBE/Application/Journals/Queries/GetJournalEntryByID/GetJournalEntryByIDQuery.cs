using Application.Abstractions.Messaging;

namespace Application.Journals.Queries.GetJournalEntryByID;

public sealed record GetJournalEntryByIDQuery(String UserId, String EntryID) : IQuery<GetJournalEntryByIDResponse>;
