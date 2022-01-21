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

        private string GetToken()
        {
            var token = Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value.FirstOrDefault();
            return token;
        }

        [HttpPost]
        [Route("AddImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Unit>> AddImage([FromForm] AddImage.ExecuteAddImage data)
        {
            return await _mediator.Send(data);
        }

        // GET: api/Owners
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetProperties([FromBody] RequestParameters requestParameters = null)
        {
            var _token = GetToken();
            return await _mediator.Send(new QueryProperty.GetProperty(requestParameters, _token));
        }

        // GET: api/Owners/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PropertyDTO>> GetProperty(Guid id)
        {
            var _token = GetToken();
            return await _mediator.Send(new QueryPropertyById.GetProperty { IdProperty = id, token = _token });
        }

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
