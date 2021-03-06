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
    /// <summary>
    ///  Get owner list about to the parameters of filtering, ordering and pagination
    /// </summary>
    public class QueryOwner
    {
        public class GetOwner : IRequest<List<OwnerDTO>>
        {
            /// <summary>
            /// it has filterParameters with information about searching and ordering.  If it is null then it doesn't filter and doesn't order
            /// it has pageParameters with information about pagination. If it is null doesn't paginate
            /// </summary>
            /// <param name="requestParameters"></param>
            public GetOwner(RequestParameters requestParameters)
            {
                this.requestParameters = requestParameters;
            }

            public RequestParameters requestParameters { get; set; }
        }

        public class HandlerOwner : IRequestHandler<GetOwner, List<OwnerDTO>>
        {
            //used to map owner to ownerDTO
            private readonly IMapper _mapper;
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
            /// Get he ownerDTO list agree search, order, paginate criteria if these exist
            /// </summary>
            /// <param name="request">
            /// it has filterParameters with information about searching and ordering.  If it is null then it doesn't filter and doesn't order
            /// it has pageParameters with information about pagination. If it is null doesn't paginate 
            /// </param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public async Task<List<OwnerDTO>> Handle(GetOwner request, CancellationToken cancellationToken)
            {
                try 
                {
                    //Get owner list about to the parameters of filtering, ordering and pagination
                    var owners = await _unitofWork.Owners.GetAll(request.requestParameters);
                    //map owners list to ownersDTO list
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
