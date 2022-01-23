using Building.PropertyAPI.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Core.Applications
{
    /// <summary>
    /// Update the information about the price of the specific property
    /// </summary>
    public class UpdatePrice
    {
        public class ExecuteUpdatePrice : IRequest
        {
            //Property identifier
            public Guid IdProperty { get; set; }
            //New price of the property
            public long NewPrice { get; set; }
        }

        public class HandlerUpdatePrice : IRequestHandler<ExecuteUpdatePrice>
        {
            //used to record information about events
            private readonly ILogger<HandlerUpdatePrice> _logger;
            //allows to group transactions of the database and finally save changes
            private readonly IUnitofWork _unitofWork;
            public HandlerUpdatePrice(ILogger<HandlerUpdatePrice> logger, IUnitofWork unitofWork)
            {
                _logger = logger;
                _unitofWork = unitofWork;
            }
            /// <summary>
            /// Update the information about the price of the specific property
            /// </summary>
            /// <param name="request">
            /// identifier property
            /// new price of the property
            /// </param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public async Task<Unit> Handle(ExecuteUpdatePrice request, CancellationToken cancellationToken)
            {
                try
                {
                    //Calls update price in the UoW and save changes
                    _unitofWork.Properties.UpdatePrice(request.IdProperty, request.NewPrice);
                    await _unitofWork.Save();
                    return Unit.Value;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdatePrice)}");
                    throw new Exception("500 Internal Server Error. Please Try Again Later.");
                };
                
            }
        }
    }
}
