﻿using Building.OwnersAPI.Core.Application;
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
        private readonly IMediator _mediator;

        public OwnersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Owners
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<OwnerDTO>>> GetOwners([FromBody] RequestParameters requestParameters = null)
        {
            return await _mediator.Send(new QueryOwner.GetOwner(requestParameters));
        }

        // GET: api/Owners/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OwnerDTO>> GetOwner(Guid id)
        {
            return await _mediator.Send(new QueryOwnerById.GetOwner { Id = id });
        }

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
