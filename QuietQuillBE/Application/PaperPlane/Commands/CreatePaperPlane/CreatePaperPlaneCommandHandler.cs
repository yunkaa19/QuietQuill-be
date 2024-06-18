using Domain.Abstraction;
using Domain.Repositories;
using Domain.Entities.PaperPlane;
using FluentValidation;
using MediatR;


namespace Application.PaperPlane.Commands.CreatePaperPlane;

public class CreatePaperPlaneCommandHandler : IRequestHandler<CreatePaperPlaneCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreatePaperPlaneCommand> _validator;
    private readonly IPaperPlaneRepository _paperPlaneRepository;

    public CreatePaperPlaneCommandHandler(IUnitOfWork unitOfWork, IValidator<CreatePaperPlaneCommand> validator, IPaperPlaneRepository paperPlaneRepository)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        _paperPlaneRepository = paperPlaneRepository;
    }

    public async Task<int> Handle(CreatePaperPlaneCommand request, CancellationToken cancellationToken)
    {
        
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var paperPlane = new PaperPlaneEntity(request.Content, request.UserId);

        await _paperPlaneRepository.AddPaperPlane(paperPlane);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return the ID of the newly created PaperPlane
        return paperPlane.id;
    }
}