using Building.OwnersAPI.Core.Entities;
using Building.OwnersAPI.Repository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Core.Application
{
    public class UpdateOwner
    {
        public class ExecuteOwner : IRequest
        {
            /// <summary>
            /// Identifier of the owner changed
            /// </summary>
            public Guid IdOwner { get; set; }
            /// <summary>
            /// Name of the owner
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Address of the owner of the property
            /// </summary>
            public string Address { get; set; }
            /// <summary>
            /// Localization in of the photo file on the server
            /// </summary>
            public IFormFile PhotoFile { get; set; }
            /// <summary>
            /// Owner's birthday
            /// </summary>
            public DateTime Birthday { get; set; } = DateTime.UtcNow;
        }

        /// <summary>
        /// validate that id isn't empty
        /// </summary>
        public class ExecuteValidation : AbstractValidator<ExecuteOwner>
        {
            public ExecuteValidation()
            {
                RuleFor(x => x.IdOwner).NotEmpty();
            }
        }
        public class HandlerOwner : IRequestHandler<ExecuteOwner>
        {
            //used to ge information about local server
            private readonly IWebHostEnvironment _webHostEnvironment;
            //used to record information about events
            private readonly ILogger<HandlerOwner> _logger;
            //allows to group transactions of the database and finally save changes
            private readonly IUnitofWork _unitofWork;
            public HandlerOwner(ILogger<HandlerOwner> logger, IUnitofWork unitofWork, IWebHostEnvironment webHostEnvironment)
            {
                _logger = logger;
                _unitofWork = unitofWork;
                _webHostEnvironment = webHostEnvironment;
            }

            /// <summary>
            /// Update owner information
            /// </summary>
            /// <param name="request">name, address, birthday and photo file</param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public async Task<Unit> Handle(ExecuteOwner request, CancellationToken cancellationToken)
            {
                //delete previous file. It is not impelemented because it is neccesary to grant permissions in the local server
                //updload the photo file to the local server. Really get the unique file name. 
                //It doesn't upload the photo file because it is neccesary to grant permissiones in the local server
                string uniqueFileName = UploadedFile(request.PhotoFile);
                //create a new onwer
                var owner = new Owner
                {
                    IdOwner = request.IdOwner,
                    Name = request.Name,
                    Address = request.Address,
                    Photo = uniqueFileName,
                    Birthday = request.Birthday
                };

                try
                {
                    //update the owner in the Owners database
                    _unitofWork.Owners.Update(owner);
                    await _unitofWork.Save();
                    return Unit.Value;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateOwner)}");
                    throw new Exception("500 Internal Server Error. Please Try Again Later.");
                };
            }
            /// <summary>
            /// Upload a image file to the server
            /// </summary>
            /// <param name="PhotoFile"></param>
            /// <returns></returns>
            private string UploadedFile(IFormFile PhotoFile)
            {
                string uniqueFileName = null;

                if (PhotoFile != null)
                {
                    //get the folder name of the local server
                    //string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Photos");
                    //assgin a unique file name to the photo file
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + PhotoFile.FileName;
                    //string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    upload the photo file to the local server
                    //    PhotoFile.CopyTo(fileStream);
                    //}
                }
                return uniqueFileName;
            }
        }
    }
}
