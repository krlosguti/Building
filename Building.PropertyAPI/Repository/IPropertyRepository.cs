using Building.PropertyAPI.Core.DTO;
using Building.PropertyAPI.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Building.PropertyAPI.Repository
{
    public interface IPropertyRepository
    {
        Task<PropertyDTO> Get(Guid id, string token);

        Task<List<PropertyDTO>> GetAll(string token, RequestParameters request = null);

        Task Insert(Property property, ICollection<IFormFile> ListImages = null);

        void UpdatePrice(Guid IdProperty, long NewPrice);

        Task AddImage(Guid IdProperty, IFormFile ImageFile);

        Task<bool> ExistProperty(Guid IdProperty);
    }
}
