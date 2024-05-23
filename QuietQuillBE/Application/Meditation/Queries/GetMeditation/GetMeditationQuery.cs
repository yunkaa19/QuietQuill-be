using Application.Abstractions.Messaging;

namespace Application.Meditation.Queries.GetMeditation;

public sealed record GetMeditationQuery(string Id) : IQuery<GetMeditationResponse>;