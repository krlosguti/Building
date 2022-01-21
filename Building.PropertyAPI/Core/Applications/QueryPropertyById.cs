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

            public HandlerProperty(IUnitofWork unitofWork, IMapper mapper, ILogger<HandlerProperty> logger)
            {
                _unitofWork = unitofWork;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<PropertyDTO> Handle(GetProperty request, CancellationToken cancellationToken)
            {
                var property = await _unitofWork.Properties.Get(request.IdProperty);
                if (property == null)
                {
                    throw new Exception("Property doesnt exist");
                }
                    
                var propertyDTO = _mapper.Map<Property, PropertyDTO>(property);
                return propertyDTO;
            }
        }
    }
}
