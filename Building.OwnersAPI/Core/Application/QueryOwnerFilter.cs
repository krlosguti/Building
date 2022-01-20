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
    public class QueryOwnerFilter
    {
        public class GetOwner : IRequest<OwnerDTO>
        {
            public Guid Id { get; set; }
        }

        public class HandlerOwner : IRequestHandler<GetOwner, OwnerDTO>
        {
            public readonly IMapper _mapper;
            private readonly ILogger<HandlerOwner> _logger;
            private readonly IUnitofWork _unitofWork;
            public HandlerOwner(IMapper mapper, ILogger<HandlerOwner> logger, IUnitofWork unitofWork)
            {
                _mapper = mapper;
                _logger = logger;
                _unitofWork = unitofWork;
            }
            public async Task<OwnerDTO> Handle(GetOwner request, CancellationToken cancellationToken)
            {
                var owner = await _unitofWork.Owners.Get(q => q.IdOwner == request.Id);
                if (owner == null)
                    throw new Exception("Owner doesnt exist");
                var ownerDTO = _mapper.Map<Owner, OwnerDTO>(owner);
                return ownerDTO;
            }
        }
    }
}
