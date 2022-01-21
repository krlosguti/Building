using Building.PropertyAPI.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Core.Applications
{
    public class UpdatePrice
    {
        public class ExecuteUpdatePrice : IRequest
        {
            public Guid IdProperty { get; set; }
            public long NewPrice { get; set; }
        }

        public class HandlerUpdatePrice : IRequestHandler<ExecuteUpdatePrice>
        {
            private readonly ILogger<HandlerUpdatePrice> _logger;
            private readonly IUnitofWork _unitofWork;
            public HandlerUpdatePrice(ILogger<HandlerUpdatePrice> logger, IUnitofWork unitofWork)
            {
                _logger = logger;
                _unitofWork = unitofWork;
            }
            public async Task<Unit> Handle(ExecuteUpdatePrice request, CancellationToken cancellationToken)
            {
                try
                {
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
