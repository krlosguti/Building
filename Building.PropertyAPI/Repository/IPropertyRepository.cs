using Building.PropertyAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Repository
{
    public interface IPropertyRepository
    {
        Task<Property> Get(Guid id);

        Task<List<Property>> GetAll(RequestParameters request = null);

        Task Insert(Property property);

        Task Delete(Guid id);

        void Update(Property property);
    }
}
