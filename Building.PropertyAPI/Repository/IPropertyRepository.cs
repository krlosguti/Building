using Building.PropertyAPI.Core.DTO;
using Building.PropertyAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Repository
{
    public interface IPropertyRepository
    {
        Task<PropertyDTO> Get(Guid id, string token);

        Task<List<PropertyDTO>> GetAll(string token, RequestParameters request = null);

        Task Insert(Property property);

        Task Delete(Guid id);

        void Update(Property property);
    }
}
