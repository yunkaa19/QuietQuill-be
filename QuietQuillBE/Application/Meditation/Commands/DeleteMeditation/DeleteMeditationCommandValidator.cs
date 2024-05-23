using FluentValidation;

namespace Application.Meditation.Commands.DeleteMeditation;

public class DeleteMeditationCommandValidator : AbstractValidator<DeleteMeditationCommand>
{
    public DeleteMeditationCommandValidator()
    {
        RuleFor(x => x.MeditationSessionDto.Id).NotEmpty();
    }
}