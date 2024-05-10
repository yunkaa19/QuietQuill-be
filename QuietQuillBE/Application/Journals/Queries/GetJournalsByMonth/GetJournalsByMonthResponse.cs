using Application.Journals.DTOs;
using Domain.Entities;

namespace Application.Journals.Queries.GetJournalsByMonth;

public sealed record GetJournalsByMonthResponse(JournalDTO[] Journals);