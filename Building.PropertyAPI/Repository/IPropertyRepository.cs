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
        //Get information about the property with IdProperty equal to id including the owner
        //the token is used to get information about owner property from owner microservice
        Task<PropertyDTO> Get(Guid id, string token);
        //Get properties list agree to the parameters of filtering, ordering and pagination
        Task<List<PropertyDTO>> GetAll(string token, RequestParameters request = null);
        //Insert a new property
        Task Insert(Property property, ICollection<IFormFile> ListImages = null);
        //Update the information about the price of the specific property
        void UpdatePrice(Guid IdProperty, long NewPrice);
        //Add an image file of a specific property
        Task AddImage(Guid IdProperty, IFormFile ImageFile);
        //Return true if the property with identifier IdProperty Exist, otherwise false
        Task<bool> ExistProperty(Guid IdProperty);
        //Update information about a property
        void UpdateProperty(Property property);
    }
}
