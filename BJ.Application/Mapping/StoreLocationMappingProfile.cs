using AutoMapper;
using BJ.Contract.StoreLocation;
using BJ.Contract.Translation.Store;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class StoreLocationMappingProfile : Profile
    {
        public StoreLocationMappingProfile()
        {
            CreateMap<StoreLocation, StoreLocationDto>().ForPath(dest => dest.StoreLocationOpenHourDtos, opt => opt.MapFrom(src => src.StoreLocationOpenHours))
                .ForPath(dest => dest.StoreLocationTranslationDtos, opt => opt.MapFrom(src => src.StoreLocationTranslations));
            CreateMap<StoreLocationOpenHour, StoreLocationOpenHourDto>().ForPath(dest => dest.StoreLocationDto, opt => opt.MapFrom(src => src.StoreLocation));
            CreateMap<StoreLocationTranslation, StoreLocationTranslationDto>();

            CreateMap<CreateStoreLocationDto, StoreLocation>();
            CreateMap<CreateStoreLocationOpenHourDto, StoreLocationOpenHour>();
            CreateMap<CreateStoreLocationTranslationDto, StoreLocationTranslation>();


            CreateMap<UpdateStoreLocationDto, StoreLocation>();
            CreateMap<UpdateStoreLocationOpenHourDto, StoreLocationOpenHour>();
            CreateMap<UpdateStoreLocationTranslationDto, StoreLocationTranslation>();


        }
    }
}
