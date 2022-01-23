using AutoMapper;
using Building.PropertyAPI.Core.DTO;
using Building.PropertyAPI.Core.Entities;

namespace Building.PropertyAPI.Core.Applications
{
    public class MappingProfile : Profile
    {
        //creates the map from property to propertyDTO
        public MappingProfile()
        {
            CreateMap<Property, PropertyDTO>();
        }
    }
}
