using AutoMapper;
using BJ.Contract.StoreLocation;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class StoreLocationMappingProfile : Profile
    {
        public StoreLocationMappingProfile()
        {
            CreateMap<StoreLocation, StoreLocationDto>();

            CreateMap<CreateStoreLocationDto, StoreLocation>();


            CreateMap<UpdateStoreLocationDto, StoreLocation>();


        }
    }
}
