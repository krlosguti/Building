using AutoMapper;
using Building.PropertyAPI.Core.Context;
using Building.PropertyAPI.Core.DTO;
using Building.PropertyAPI.Core.Entities;
using Building.PropertyAPI.RemoteService.Entities;
using Building.PropertyAPI.RemoteService.Interface;
using Building.PropertyAPI.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Core.Applications
{
    public class QueryPropertyById
    {
        public class GetProperty : IRequest<PropertyDTO>
        {
            public Guid IdProperty { get; set; }
            public string token { get; set; }
        }

        public class HandlerProperty : IRequestHandler<GetProperty, PropertyDTO>
        {
            public readonly IMapper _mapper;
            private readonly ILogger<HandlerProperty> _logger;
            private readonly IUnitofWork _unitofWork;
            private readonly IOwnerService _ownerService;

            public HandlerProperty(IUnitofWork unitofWork, IMapper mapper, ILogger<HandlerProperty> logger, IOwnerService ownerService)
            {
                _unitofWork = unitofWork;
                _mapper = mapper;
                _logger = logger;
                _ownerService = ownerService;
            }

            public async Task<PropertyDTO> Handle(GetProperty request, CancellationToken cancellationToken)
            {
                var property = await _unitofWork.Properties.Get(request.IdProperty, request.token);
                if (property == null)
                {
                    throw new Exception("Property doesnt exist");
                }
                //var result = await _ownerService.GetOwner(property.IdOwner, request.token);
                //property.Owner = result.owner;
                return property;
            }
        }
    }
}
