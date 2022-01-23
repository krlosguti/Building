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
    /// <summary>
    /// Get properties list agree to the parameters of filtering, ordering and pagination
    /// </summary>
    public class QueryProperty
    {
        public class GetProperty : IRequest<List<PropertyDTO>>
        {
            /// <summary>
            /// Get properties list agree to the parameters of filtering, ordering and pagination
            /// </summary>
            /// <param name="requestParameters">
            /// it has filterParameters with information about searching and ordering.  If it is null then it doesn't filter and doesn't order
            /// it has pageParameters with information about pagination. If it is null doesn't paginate
            /// it has token to send request to the owner microservice with the objective to get information about property owner
            /// </param>
            /// <param name="token"></param>
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
                //used to map property to propertyDTO
                _mapper = mapper;
                //used to record information about events
                _logger = logger;
                //allow to group transactions of the database and finally save changes
                _unitofWork = unitofWork;
            }

            /// <summary>
            /// Get properties list agree to the parameters of filtering, ordering and pagination
            /// </summary>
            /// <param name="request">
            /// it has filterParameters with information about searching and ordering.  If it is null then it doesn't filter and doesn't order
            /// it has pageParameters with information about pagination. If it is null doesn't paginate
            /// it has token to send request to the owner microservice with the objective to get information about property owner
            /// </param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public async Task<List<PropertyDTO>> Handle(GetProperty request, CancellationToken cancellationToken)
            {
                try
                {
                    //Call unit of work to retrieve the property list agree parameters of filtering and pagination
                    var propertiesDTO = await _unitofWork.Properties.GetAll(request.token, request.requestParameters);
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
