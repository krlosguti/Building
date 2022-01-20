using Building.OwnersAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Building.OwnersAPI.Repository
{
    public interface IOwnerRepository 
    {
        Task<Owner> Get(Guid id);

        Task<List<Owner>> GetAll(RequestParameters request=null);

        Task Insert(Owner owner);

        Task Delete(Guid id);

        void Update(Owner owner);
    }
}
