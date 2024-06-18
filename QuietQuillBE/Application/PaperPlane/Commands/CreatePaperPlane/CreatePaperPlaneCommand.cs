using Application.Abstractions.Messaging;

namespace Application.PaperPlane.Commands.CreatePaperPlane;

public record CreatePaperPlaneCommand(string Content, Guid UserId) : ICommand<int>;