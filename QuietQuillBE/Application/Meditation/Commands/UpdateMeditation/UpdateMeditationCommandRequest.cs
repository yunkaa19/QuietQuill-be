using Application.Meditation.DTOs;

namespace Application.Meditation.Commands.UpdateMeditation;

public sealed record UpdateMeditationCommandRequest(MeditationSessionDTO Session);