using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Repository
{
    /// <summary>
    /// Repository Interface to management owner transactions 
    /// </summary>
    public interface IOwnerRepository 
    {
        //returns the context
        public OwnerContext GetContext();

        //get the owner by id
        Task<Owner> Get(Guid id);
        //Get owner list agree to the parameters of filtering, ordering and pagination
        Task<List<Owner>> GetAll(RequestParameters request=null);
        //insert a new owner
        Task Insert(Owner owner);
        //delete one owner by id
        Task Delete(Guid id);

        //update information of the owner
        void Update(Owner owner);
    }
}
