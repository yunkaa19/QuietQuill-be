using System.Net.WebSockets;
using MediatR;
using System.Collections.Concurrent;
using Application.PaperPlane.Commands.CreatePaperPlane;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace QuietQuillBE.MiddleWare
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<WebSocket, Task> _sockets = new ConcurrentDictionary<WebSocket, Task>();
        private readonly ILogger<WebSocketMiddleware> _logger;
        private readonly IServiceProvider _serviceProvider;

        public WebSocketMiddleware(RequestDelegate next, IServiceProvider serviceProvider, ILogger<WebSocketMiddleware> logger)
        {
            _next = next;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    // Authenticate the user
                    var authResult = await context.AuthenticateAsync();
                    if (!authResult.Succeeded || !context.User.Identity.IsAuthenticated)
                    {
                        context.Response.StatusCode = 401; // Unauthorized
                        await context.Response.WriteAsync("Unauthorized");
                        return;
                    }

                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var socketFinishedTcs = new TaskCompletionSource<object>();
                    _sockets.TryAdd(webSocket, socketFinishedTcs.Task);

                    try
                    {
                        _logger.LogInformation("WebSocket connection established.");
                        await HandleWebSocket(context, webSocket);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "WebSocket connection handling error.");
                    }
                    finally
                    {
                        _sockets.TryRemove(webSocket, out _);
                        socketFinishedTcs.TrySetResult(null);
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocket middleware", CancellationToken.None);
                        webSocket.Dispose();
                        _logger.LogInformation("WebSocket connection closed.");
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task HandleWebSocket(HttpContext context, WebSocket webSocket)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                while (!result.CloseStatus.HasValue)
                {
                    var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                    _logger.LogInformation($"Received message: {message}");

                    try
                    {
                        // Deserialize the message to a PaperPlaneDTO
                        var paperPlaneDto = JsonSerializer.Deserialize<PaperPlaneDTO>(message);

                        if (paperPlaneDto == null || paperPlaneDto.UserId == Guid.Empty || string.IsNullOrEmpty(paperPlaneDto.Content))
                        {
                            throw new Exception("Invalid payload");
                        }

                        // Create and send the CreatePaperPlaneCommand
                        var createCommand = new CreatePaperPlaneCommand(paperPlaneDto.Content, paperPlaneDto.UserId);
                        var paperPlaneId = await mediator.Send(createCommand);

                        _logger.LogInformation($"PaperPlane created with ID: {paperPlaneId}");

                        // Broadcast the message to all connected clients
                        var tasks = _sockets.Keys
                            .Where(s => s.State == WebSocketState.Open)
                            .Select(s => s.SendAsync(new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(message), 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None));

                        await Task.WhenAll(tasks);
                    }
                    catch (JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "Invalid JSON payload");
                        await webSocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Invalid JSON payload", CancellationToken.None);
                        return;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing the message");
                        await webSocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Invalid payload", CancellationToken.None);
                        return;
                    }

                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                _logger.LogInformation("WebSocket connection closing.");
            }
        }

        private class PaperPlaneDTO
        {
            public string Content { get; set; }
            public Guid UserId { get; set; }
        }
    }
}
