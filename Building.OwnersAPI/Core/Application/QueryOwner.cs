using AutoMapper;
using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.DTO;
using Building.OwnersAPI.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Building.OwnersAPI.Repository;
using System.Linq.Expressions;
using System.Linq;

namespace Building.OwnersAPI.Core.Application
{
    public class QueryOwner
    {
        public class GetOwner : IRequest<List<OwnerDTO>>
        {
            public GetOwner(RequestParameters requestParameters)
            {
                this.requestParameters = requestParameters;
            }

            public RequestParameters requestParameters { get; set; }
        }

        public class HandlerOwner : IRequestHandler<GetOwner, List<OwnerDTO>>
        {
            private readonly IMapper _mapper;
            private readonly ILogger<HandlerOwner> _logger;
            private readonly IUnitofWork _unitofWork;
            public HandlerOwner(IMapper mapper, ILogger<HandlerOwner> logger, IUnitofWork unitofWork)
            {
                _mapper = mapper;
                _logger = logger;
                _unitofWork = unitofWork;
            }

            public async Task<List<OwnerDTO>> Handle(GetOwner request, CancellationToken cancellationToken)
            {
                try 
                {
                    var owners = await _unitofWork.Owners.GetAll(request.requestParameters);
                    var ownersDTO = _mapper.Map<List<Owner>, List<OwnerDTO>>(owners);
                    return ownersDTO;
                } 
                catch(Exception ex)
                {
                    _logger.LogError($"Something went wrong in the {nameof(GetOwner)}");
                    throw new Exception($"Something went wrong {ex.Message}");
                }
            }
        }
    }
}
