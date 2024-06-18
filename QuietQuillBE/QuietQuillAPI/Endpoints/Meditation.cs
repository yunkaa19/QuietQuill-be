using Application.Meditation.Commands.CreateMeditation;
using Application.Meditation.Commands.DeleteMeditation;
using Application.Meditation.Commands.UpdateMeditation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuietQuillBE.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class Meditation : ControllerBase
    {
        private readonly IMediator _mediator;

        public Meditation(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("Create"), Authorize]
        public async Task<IActionResult> CreateEntry([FromBody] CreateMeditationCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(token);
        }
        
        [HttpDelete("Delete"), Authorize]
        public async Task<IActionResult> DeleteEntry([FromBody] DeleteMeditationCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(token);
        }

        [HttpPost("Update"), Authorize]
        public async Task<IActionResult> UpdateEntry([FromBody] UpdateMeditationCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(token);
        }
        
        
    }
}
