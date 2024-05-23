using Application.Abstractions.Messaging;
using Application.Meditation.DTOs;

namespace Application.Meditation.Commands.DeleteMeditation;

public record DeleteMeditationCommand(MeditationSessionDTO MeditationSessionDto) : ICommand<bool>;