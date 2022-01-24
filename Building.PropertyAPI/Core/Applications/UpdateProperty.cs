using Building.PropertyAPI.Core.Entities;
using Building.PropertyAPI.Repository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Core.Applications
{
    public class UpdateProperty
    {
        /// <summary>
        /// Update specifif property
        /// </summary>
        public class RequestUpdateProperty: IRequest
        {
            /// <summary>
            /// Identifier of the property to be modified
            /// </summary>
            public Guid IdProperty { get; set; }
            /// <summary>
            /// Name of the property
            /// </summary>
            public string Name { get; set; } = "";
            /// <summary>
            /// Address of the property
            /// </summary>
            public string Address { get; set; } = "";
            /// <summary>
            /// Code internal
            /// </summary>
            public string CodeInternal { get; set; }
            /// <summary>
            /// Price of the property
            /// </summary>
            public long Price { get; set; }
            /// <summary>
            /// Year of the property
            /// </summary>
            public int Year { get; set; }
            /// <summary>
            /// Guid of the owner 
            /// </summary>
            public Guid IdOwner { get; set; }
        }

        public class ExecuteValidation : AbstractValidator<RequestUpdateProperty>
        {
            /// <summary>
            /// validates name and address are not empty.  validates year is less or equal to the current year.
            /// </summary>
            public ExecuteValidation()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Address).NotEmpty();
                RuleFor(x => x.Year).LessThanOrEqualTo(DateTime.Now.Year);
            }
        }

        public class HandlerProperty : IRequestHandler<RequestUpdateProperty>
        {
            //used to record information about events
            private readonly ILogger<HandlerProperty> _logger;
            //allow to group transactions of the database and finally save changes
            private readonly IUnitofWork _unitofWork;
            public HandlerProperty(ILogger<HandlerProperty> logger, IUnitofWork unitofWork)
            {
                _logger = logger;
                _unitofWork = unitofWork;
            }
            /// <summary>
            /// Update a new property
            /// </summary>
            /// <param name="request">information about the property to be updated: name, address, codeinternal, year, price, idowner, images list</param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public async Task<Unit> Handle(RequestUpdateProperty request, CancellationToken cancellationToken)
            {
                try
                {
                    //Create a new property
                    var property = new Property
                    {
                        IdProperty = request.IdProperty,
                        Name = request.Name,
                        Address = request.Address,
                        Price = request.Price,
                        CodeInternal = request.CodeInternal,
                        Year = request.Year,
                        IdOwner = request.IdOwner
                    };
                     _unitofWork.Properties.UpdateProperty(property);
                    await _unitofWork.Save();
                    return Unit.Value;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something Went Wrong in the {nameof(NewProperty)}");
                    throw new Exception("500 Internal Server Error. Please Try Again Later.");
                }

            }
        }
    }
}
