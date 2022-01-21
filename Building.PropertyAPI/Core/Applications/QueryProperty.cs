using AutoMapper;
using Building.PropertyAPI.Core.Context;
using Building.PropertyAPI.Core.DTO;
using Building.PropertyAPI.Core.Entities;
using Building.PropertyAPI.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Core.Applications
{
    public class QueryProperty
    {
        public class GetProperty : IRequest<List<PropertyDTO>>
        {
            public GetProperty(RequestParameters requestParameters,string token)
            {
                this.requestParameters = requestParameters;
                this.token = token;
            }

            public RequestParameters requestParameters { get; set; }
            public string token { get; set; }
        }

        public class HandlerProperty : IRequestHandler<GetProperty, List<PropertyDTO>>
        {
            private readonly IMapper _mapper;
            private readonly ILogger<HandlerProperty> _logger;
            private readonly IUnitofWork _unitofWork;
            public HandlerProperty(IUnitofWork unitofWork, IMapper mapper, ILogger<HandlerProperty> logger)
            {
                _mapper = mapper;
                _logger = logger;
                _unitofWork = unitofWork;
            }

            public async Task<List<PropertyDTO>> Handle(GetProperty request, CancellationToken cancellationToken)
            {
                try
                {
                    var propertiesDTO = await _unitofWork.Properties.GetAll(request.token, request.requestParameters);
                    //var propertiesDTO = _mapper.Map<List<Property>, List<PropertyDTO>>(properties);
                    return propertiesDTO;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong in the {nameof(GetProperty)}");
                    throw new Exception($"Something went wrong {ex.Message}");
                }
            }
        }
    }
}
