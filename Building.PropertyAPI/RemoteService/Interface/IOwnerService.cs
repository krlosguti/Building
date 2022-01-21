using Building.PropertyAPI.RemoteService.Entities;
using System;
using System.Threading.Tasks;

namespace Building.PropertyAPI.RemoteService.Interface
{
    public interface IOwnerService
    {/// <summary>
    /// Get the owner identified with Id. It is obtained of the Owner Microservice
    /// </summary>
    /// <param name="Id">Identifier of the owner</param>
    /// <returns>result true, Owner y ErrorMessage null. Or result false, owner null, Error Message</returns>
        Task<(bool result, OwnerDTO owner, string ErrorMessage)> GetOwner(Guid Id, string token);
    }
}
