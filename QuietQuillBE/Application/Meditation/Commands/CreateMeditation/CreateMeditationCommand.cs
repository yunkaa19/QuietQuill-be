using Application.Abstractions.Messaging;
using Application.Meditation.DTOs;

namespace Application.Meditation.Commands.CreateMeditation;

public record CreateMeditationCommand(MeditationSessionDTO MeditationSessionDto) : ICommand<MeditationSessionDTO>;