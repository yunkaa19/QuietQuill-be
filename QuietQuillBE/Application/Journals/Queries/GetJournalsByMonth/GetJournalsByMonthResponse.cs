using Application.Journals.DTOs;

namespace Application.Journals.Queries.GetJournalsByMonth;

public sealed record GetJournalsByMonthResponse(JournalDTO[] Journals);