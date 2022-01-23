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
        /// <summary>
        /// injecting MediatR to use CQRS Pattern
        /// </summary>
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// api to add an user
        /// </summary>
        /// <param name="request">username, email and password</param>
        /// <returns>UserDTO: username, id, token</returns>
        [HttpPost("add")]
        public async Task<ActionResult<UserDTO>> PostUser(AddNewUser.UserAdd request)
        {
            return await _mediator.Send(request);
        }
        /// <summary>
        /// user login
        /// </summary>
        /// <param name="request">username and password</param>
        /// <returns>UserDTO: username, id, authentication token</returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> LoginUser(Login.UserLogin request)
        {
            return await _mediator.Send(request);
        }

        /// <summary>
        /// Get user connected currently 
        /// </summary>
        /// <returns>UserDTO: username, id, token</returns>
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetUser()
        {
            return await _mediator.Send(new UserActual.GetUserActual());
        }
    }
}
