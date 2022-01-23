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
    /// <summary>
    /// add image to specific property
    /// </summary>
    public class AddImage
    {
        //information about property identifier and image file
        public class ExecuteAddImage : IRequest
        {
            public Guid IdProperty { get; set; }
            public IFormFile ImageFile { get; set; }
        }

        /// <summary>
        /// method to upload image in the local server y associate it to the specific property
        /// </summary>
        public class HandlerAddImage : IRequestHandler<ExecuteAddImage>
        {
            private readonly ILogger<HandlerProperty> _logger;
            private readonly IUnitofWork _unitofWork;
            public HandlerAddImage(ILogger<HandlerProperty> logger, IUnitofWork unitofWork)
            {
                //used to record information about events
                _logger = logger;
                //allows to group transactions of the database and finally save changes
                _unitofWork = unitofWork;
            }
            public async Task<Unit> Handle(ExecuteAddImage request, CancellationToken cancellationToken)
            {
                try
                {
                    //add new image and save changes
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
