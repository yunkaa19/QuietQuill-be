using Application.Meditation.DTOs;

namespace Application.Meditation.Commands.CreateMeditation;

public sealed record CreateMeditationCommandRequest(MeditationSessionDTO MeditationSessionDto);