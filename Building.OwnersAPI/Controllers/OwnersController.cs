using Building.OwnersAPI.Core.Application;
using Building.OwnersAPI.Core.DTO;
using Building.OwnersAPI.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        //inject mediator to use CQRS Pattern
        private readonly IMediator _mediator;

        public OwnersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get owners list agree to the parameters of filtering, ordering and pagination
        /// </summary>
        /// <param name="requestParameters">
        /// it has filterParameters with information about searching and ordering.  If it is null then it doesn't filter and doesn't order
        /// it has pageParameters with information about pagination. If it is null doesn't paginate
        /// </param>
        /// <returns>ownerDTO list agree previous criteria</returns>
        // GET: api/Owners
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<OwnerDTO>>> GetOwners([FromBody] RequestParameters requestParameters = null)
        {
            return await _mediator.Send(new QueryOwner.GetOwner(requestParameters));
        }

        /// <summary>
        /// Get information about the owner with IdOwner equal to id
        /// </summary>
        /// <param name="id">id of the owner</param>
        /// <returns>ownerDTO model with information about the owner</returns>
        // GET: api/Owners/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OwnerDTO>> GetOwner(Guid id)
        {
            return await _mediator.Send(new QueryOwnerById.GetOwner { Id = id });
        }

        /// <summary>
        /// Add a new owner
        /// </summary>
        /// <param name="data">
        /// name, address, birthday date and photo file.
        /// </param>
        /// <returns>
        /// add the new owner and updload the photo file to the local server
        /// </returns>
        // POST: api/Owners
        [HttpPost("AddOwner")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Unit>> Add([FromForm] NewOwner.ExecuteOwner data)
        {
            return await _mediator.Send(data);
        }

        /// <summary>
        /// Update owner information
        /// </summary>
        /// <param name="data">
        /// name, address, birthday and photo file
        /// </param>
        /// <returns></returns>
        // PUT: api/Owners
        [HttpPut("UpdateOwner")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Unit>> Update([FromForm] UpdateOwner.ExecuteOwner data)
        {
            return await _mediator.Send(data);
        }
    }
}
