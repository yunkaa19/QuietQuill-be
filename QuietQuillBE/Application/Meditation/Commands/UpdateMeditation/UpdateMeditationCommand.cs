using Application.Abstractions.Messaging;
using Application.Meditation.DTOs;

namespace Application.Meditation.Commands.UpdateMeditation;

public record UpdateMeditationCommand(MeditationSessionDTO Session) : ICommand<MeditationSessionDTO>;