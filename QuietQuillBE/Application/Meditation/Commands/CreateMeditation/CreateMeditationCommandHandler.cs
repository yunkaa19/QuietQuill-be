using Application.Meditation.DTOs;
using Domain.Abstraction;
using Domain.Entities.Meditation;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Meditation.Commands.CreateMeditation;

public class CreateMeditationCommandHandler : IRequestHandler<CreateMeditationCommand, MeditationSessionDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateMeditationCommand> _validator;
    private readonly IMeditationSessionRepository _meditationSessionRepository;
    
    public CreateMeditationCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateMeditationCommand> validator, IMeditationSessionRepository meditationSessionRepository)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _meditationSessionRepository = meditationSessionRepository;
    }
    
    
    
    public async Task<MeditationSessionDTO> Handle(CreateMeditationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var session = new MeditationSession(request.MeditationSessionDto.Title, request.MeditationSessionDto.Duration, request.MeditationSessionDto.Description, request.MeditationSessionDto.Type);

        _meditationSessionRepository.CreateMeditationSession(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return request.MeditationSessionDto;
        
    }
}