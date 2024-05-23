
using Application.Meditation.DTOs;
using Domain.Abstraction;
using Domain.Exceptions.Meditation;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Meditation.Commands.UpdateMeditation;

public class UpdateMeditationCommandHandler : IRequestHandler<UpdateMeditationCommand, MeditationSessionDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateMeditationCommand> _validator;
    private readonly IMeditationSessionRepository _meditationSessionRepository;
    public UpdateMeditationCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateMeditationCommand> validator, IMeditationSessionRepository meditationSessionRepository)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _meditationSessionRepository = meditationSessionRepository;
    }
    
    
    
    public async Task<MeditationSessionDTO> Handle(UpdateMeditationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var meditationSession = _meditationSessionRepository.GetMeditationSessionById(request.Session.Id);
        if (meditationSession == null)
        {
            throw new MeditationNotFoundException(request.Session.Id);
        }
        
        _meditationSessionRepository.UpdateMeditationSession(meditationSession);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return request.Session;
    }
}