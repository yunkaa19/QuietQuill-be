using Application.Journals.Commands.CreateEntry;
using Application.Journals.Commands.DeleteEntry;
using Application.Journals.Commands.UpdateEntry;
using Application.Journals.Queries.GetJournalsByMonth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;


namespace QuietQuillBE.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JournalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpPost("Create"), Authorize]
        public async Task<IActionResult> CreateEntry([FromBody] CreateEntryCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(token);
        }
        
        
        [HttpPost("Delete"), Authorize]
        public async Task<IActionResult> DeleteEntry([FromBody] DeleteEntryCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(token);
        }
        
        [HttpPost("Update"), Authorize]
        public async Task<IActionResult> UpdateEntry([FromBody] UpdateEntryCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(token);
        }
        
        [HttpGet("GetMonth"), Authorize]
        public async Task<IActionResult> GetEntries([FromBody] GetJournalsByMonthQuery query)
        {
            var token = await _mediator.Send(query);
            return Ok(token);
        }
        
        
    }
}
