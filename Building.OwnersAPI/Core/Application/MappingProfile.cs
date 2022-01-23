using AutoMapper;
using Building.OwnersAPI.Core.DTO;
using Building.OwnersAPI.Core.Entities;

namespace Building.OwnersAPI.Core.Application
{
    public class MappingProfile : Profile
    {
        //creates the map from owner to ownerDTO
        public MappingProfile()
        {
            CreateMap<Owner, OwnerDTO>();
        }
    }
}
