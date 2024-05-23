using Domain.Abstraction;
using Domain.Entities.Meditation;
using Domain.Exceptions.Base;
using Domain.Exceptions.Meditation;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Meditation.Commands.DeleteMeditation;

public class DeleteMeditationCommandHandler : IRequestHandler<DeleteMeditationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteMeditationCommand> _validator;
    private readonly IMeditationSessionRepository _meditationSessionRepository;
    public DeleteMeditationCommandHandler(IUnitOfWork unitOfWork, IValidator<DeleteMeditationCommand> validator, IMeditationSessionRepository meditationSessionRepository)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _meditationSessionRepository = meditationSessionRepository;
    }
    
    
    
    public async Task<bool> Handle(DeleteMeditationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var session = _meditationSessionRepository.GetMeditationSessionById(request.MeditationSessionDto.Id);
        if (session == null)
        {
            throw new MeditationNotFoundException(request.MeditationSessionDto.Id);
        }

        _meditationSessionRepository.DeleteMeditationSession(request.MeditationSessionDto.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}