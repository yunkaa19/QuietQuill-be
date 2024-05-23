using FluentValidation;

namespace Application.Meditation.Commands.UpdateMeditation;

public class UpdateMeditationCommandValidator : AbstractValidator<UpdateMeditationCommand>
{
    public UpdateMeditationCommandValidator()
    {
        RuleFor(x => x.Session.Id).NotEmpty();
    }
}