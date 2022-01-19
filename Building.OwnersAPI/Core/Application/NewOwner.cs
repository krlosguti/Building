using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Core.Application
{
    public class NewOwner
    {
        public class ExecuteOwner : IRequest
        {
            /// <summary>
            /// Name of the owner
            /// </summary>
            public string Name { get; set; } = "";
            /// <summary>
            /// Address of the owner of the property
            /// </summary>
            public string Address { get; set; } = "";
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
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Address).NotEmpty();
                RuleFor(x => x.Birthday).LessThan(DateTime.Now);
            }
        }
        public class HandlerOwner : IRequestHandler<ExecuteOwner>
        {
            public readonly OwnerContext _context;
            private readonly IWebHostEnvironment _webHostEnvironment;
            public HandlerOwner(OwnerContext context, IWebHostEnvironment webHostEnvironment)
            {
                _context = context;
                _webHostEnvironment = webHostEnvironment;
            }
            public async Task<Unit> Handle(ExecuteOwner request, CancellationToken cancellationToken)
            {
                string uniqueFileName = UploadedFile(request.PhotoFile);

                var owner = new Owner
                {
                    IdOwner = Guid.NewGuid(),
                    Name = request.Name,
                    Address = request.Address,
                    Photo = uniqueFileName,
                    Birthday = request.Birthday
                };
                _context.Owners.Add(owner);
                var result = await _context.SaveChangesAsync();
                return (result > 0) ? Unit.Value : throw new Exception("Cannot insert new Owner");
            }

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
