using Building.IdentityServer.Core.Application;
using Building.IdentityServer.Core.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Building.IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("add")]
        public async Task<ActionResult<UserDTO>> PostUser(AddNewUser.UserAdd request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> LoginUser(Login.UserLogin request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetUser()
        {
            return await _mediator.Send(new UserActual.GetUserActual());
        }
    }
}
