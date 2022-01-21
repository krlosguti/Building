using AutoMapper;
using Building.PropertyAPI.Core.DTO;
using Building.PropertyAPI.Core.Entities;

namespace Building.PropertyAPI.Core.Applications
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Property, PropertyDTO>();
        }
    }
}
