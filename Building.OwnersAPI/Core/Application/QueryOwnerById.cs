using AutoMapper;
using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.DTO;
using Building.OwnersAPI.Core.Entities;
using Building.OwnersAPI.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Core.Application
{
    /// <summary>
    /// Get information about the owner with IdOwner equal to id
    /// </summary>
    public class QueryOwnerById
    {
        /// <summary>
        /// id of the owner to be retrieved
        /// </summary>
        public class GetOwner : IRequest<OwnerDTO>
        {
            public Guid Id { get; set; }
        }

        public class HandlerOwner : IRequestHandler<GetOwner, OwnerDTO>
        {
            //used to map owner to ownerDTO
            public readonly IMapper _mapper;
            //used to record information about events
            private readonly ILogger<HandlerOwner> _logger;
            //allows to group transactions of the database and finally save changes
            private readonly IUnitofWork _unitofWork;
            public HandlerOwner(IMapper mapper, ILogger<HandlerOwner> logger, IUnitofWork unitofWork)
            {
                _mapper = mapper;
                _logger = logger;
                _unitofWork = unitofWork;
            }
            /// <summary>
            /// Get information about the owner with IdOwner equal to id
            /// </summary>
            /// <param name="request">
            /// id of the owner
            /// </param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public async Task<OwnerDTO> Handle(GetOwner request, CancellationToken cancellationToken)
            {
                //gets the owner using id of the UoW
                var owner = await _unitofWork.Owners.Get(request.Id);
                if (owner == null) throw new Exception("Owner doesnt exist");
                //map owner to ownerDTO
                var ownerDTO = _mapper.Map<Owner, OwnerDTO>(owner);
                return ownerDTO;
            }
        }
    }
}
