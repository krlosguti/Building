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

        public class ExecuteValidation : AbstractValidator<ExecuteOwner>
        {
            public ExecuteValidation()
            {
                RuleFor(x => x.IdOwner).NotEmpty();
            }
        }
        public class HandlerOwner : IRequestHandler<ExecuteOwner>
        {
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly ILogger<HandlerOwner> _logger;
            private readonly IUnitofWork _unitofWork;
            public HandlerOwner(ILogger<HandlerOwner> logger, IUnitofWork unitofWork, IWebHostEnvironment webHostEnvironment)
            {
                _logger = logger;
                _unitofWork = unitofWork;
                _webHostEnvironment = webHostEnvironment;
            }
            public async Task<Unit> Handle(ExecuteOwner request, CancellationToken cancellationToken)
            {
                //delete previous file
                string uniqueFileName = UploadedFile(request.PhotoFile);

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
                    //string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Photos");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + PhotoFile.FileName;
                    //string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    PhotoFile.CopyTo(fileStream);
                    //}
                }
                return uniqueFileName;
            }
        }
    }
}
