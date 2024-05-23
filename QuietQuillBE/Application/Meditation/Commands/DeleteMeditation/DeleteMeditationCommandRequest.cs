using Application.Meditation.DTOs;
using Domain.Entities.Meditation;

namespace Application.Meditation.Commands.DeleteMeditation;

public sealed record DeleteMeditationCommandRequest(MeditationSessionDTO MeditationSessionDto);