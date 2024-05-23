using FluentValidation;

namespace Application.Meditation.Commands.CreateMeditation;

public class CreateMeditationCommandValidator : AbstractValidator<CreateMeditationCommand>
{
    public CreateMeditationCommandValidator()
    {
        RuleFor(x => x.MeditationSessionDto.Title).NotEmpty();
        RuleFor(x => x.MeditationSessionDto.Description).NotEmpty();
        RuleFor(x => x.MeditationSessionDto.Duration).NotEmpty();
    }
}