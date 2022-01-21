using Building.PropertyAPI.Core.Context;
using Building.PropertyAPI.Core.Entities;
using Building.PropertyAPI.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Building.PropertyAPI.Core.Applications.QueryProperty;

namespace Building.PropertyAPI.Core.Applications
{
    public class AddImage
    {
        public class ExecuteAddImage : IRequest
        {
            public Guid IdProperty { get; set; }
            public IFormFile ImageFile { get; set; }
        }

        public class HandlerAddImage : IRequestHandler<ExecuteAddImage>
        {
            private readonly ILogger<HandlerProperty> _logger;
            private readonly IUnitofWork _unitofWork;
            public HandlerAddImage(ILogger<HandlerProperty> logger, IUnitofWork unitofWork)
            {
                _logger = logger;
                _unitofWork = unitofWork;
            }
            public async Task<Unit> Handle(ExecuteAddImage request, CancellationToken cancellationToken)
            {
                try
                {
                    await _unitofWork.Properties.AddImage(request.IdProperty,request.ImageFile);
                    await _unitofWork.Save();
                    return Unit.Value;
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $"Something Went Wrong in the {nameof(AddImage)}");
                    throw new Exception("500 Internal Server Error. Please Try Again Later.");
                }
                
            }
        }
    }
}
