using Application.Users.Commands.ChangePassword;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuietQuillBE.Endpoints;

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(token);
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(token);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(token);
        }
        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUser([FromBody] GetUserByIdQuery query)
        {
            var token = await _mediator.Send(query);
            return Ok(token);
        }
        [HttpPost("UpdatePassword")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(token);
        }
    }