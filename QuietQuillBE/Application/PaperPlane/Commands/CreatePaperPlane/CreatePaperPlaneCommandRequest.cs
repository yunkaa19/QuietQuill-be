namespace Application.PaperPlane.Commands.CreatePaperPlane;

public sealed record CreatePaperPlaneCommandRequest(string Content, Guid UserId);