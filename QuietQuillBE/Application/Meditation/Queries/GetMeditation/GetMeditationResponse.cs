using Application.Meditation.DTOs;
using Domain.Entities.Meditation;

namespace Application.Meditation.Queries.GetMeditation;

public sealed record GetMeditationResponse(MeditationSession MeditationSession);