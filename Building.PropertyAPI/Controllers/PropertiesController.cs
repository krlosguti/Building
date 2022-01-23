using Building.PropertyAPI.Core.Applications;
using Building.PropertyAPI.Core.DTO;
using Building.PropertyAPI.Core.Entities;
using Building.PropertyAPI.Repository;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PropertiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get token of the header of the authenticated user
        /// </summary>
        /// <returns></returns>
        private string GetToken()
        {
            var token = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.FirstOrDefault();
            return token;
        }

        /// <summary>
        /// Add an image file of a specific property
        /// </summary>
        /// <param name="data">
        /// model with information about property identifier and image file
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Unit>> AddImage([FromForm] AddImage.ExecuteAddImage data)
        {
            return await _mediator.Send(data);
        }

        /// <summary>
        /// Get properties list agree to the parameters of filtering, ordering and pagination
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns>
        /// it has filterParameters with information about searching and ordering.  If it is null then it doesn't filter and doesn't order
        /// it has pageParameters with information about pagination. If it is null doesn't paginate
        /// </returns>
        // GET: api/Properties
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetProperties([FromBody] RequestParameters requestParameters = null)
        {
            var _token = GetToken();
            return await _mediator.Send(new QueryProperty.GetProperty(requestParameters, _token));
        }

        /// <summary>
        /// Get information about the property with IdProperty equal to id including the owner
        /// </summary>
        /// <param name="id">property identifier</param>
        /// <returns></returns>
        // GET: api/Owners/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PropertyDTO>> GetProperty(Guid id)
        {
            //get token of the current user
            var _token = GetToken();
            //get the property including information about the owner.
            return await _mediator.Send(new QueryPropertyById.GetProperty { IdProperty = id, token = _token });
        }

        /// <summary>
        /// Add a property
        /// </summary>
        /// <param name="data"></param>
        /// <returns>information about the property: name, address, codeinternal, year, price, idowner, images list</returns>
        // POST: api/Properties
        [HttpPost("AddProperty")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Unit>> Add(NewProperty.ExecuteProperty data)
        {
            return await _mediator.Send(data);
        }
        /// <summary>
        /// Update the information about the price of the specific property
        /// the method used is post method. It was possible to use patch too because is partial updating.
        /// </summary>
        /// <param name="data">property identifier and new price</param>
        /// <returns></returns>
        // PUT: api/Owners
        [HttpPut("UpdatePrice")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Unit>> Update(UpdatePrice.ExecuteUpdatePrice data)
        {
            return await _mediator.Send(data);
        }
    }
}
