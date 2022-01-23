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
    /// <summary>
    /// Get information about the property with IdProperty equal to id including the owner
    /// </summary>
    public class QueryPropertyById
    {
        public class GetProperty : IRequest<PropertyDTO>
        {
            //  property Identifier
            public Guid IdProperty { get; set; }
            //token of the current user with the objective to get information about owner property in the owner microservice
            public string token { get; set; }
        }

        public class HandlerProperty : IRequestHandler<GetProperty, PropertyDTO>
        {
            //used to map property to propertyDTO
            public readonly IMapper _mapper;
            //used to record information about events
            private readonly ILogger<HandlerProperty> _logger;
            //allow to group transactions of the database and finally save changes
            private readonly IUnitofWork _unitofWork;
            //services that get information about owner property of the owner microservice
            private readonly IOwnerService _ownerService;

            public HandlerProperty(IUnitofWork unitofWork, IMapper mapper, ILogger<HandlerProperty> logger, IOwnerService ownerService)
            {
                _unitofWork = unitofWork;
                _mapper = mapper;
                _logger = logger;
                _ownerService = ownerService;
            }
            /// <summary>
            /// Get information about the property with IdProperty equal to id including the owner
            /// </summary>
            /// <param name="request">
            /// property identifier and 
            /// token of the current user with the objective to get information about owner property in the owner microservice
            /// </param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public async Task<PropertyDTO> Handle(GetProperty request, CancellationToken cancellationToken)
            {
                //get the property (including owner) identified with idProperty
                var property = await _unitofWork.Properties.Get(request.IdProperty, request.token);
                if (property == null)
                {
                    throw new Exception("Property doesnt exist");
                }
                return property;
            }
        }
    }
}
