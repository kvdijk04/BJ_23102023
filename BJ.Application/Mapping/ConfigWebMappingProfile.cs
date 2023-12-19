using AutoMapper;
using BJ.Contract.ConfigWeb;
using BJ.Contract.ConfigWeb.CreateConfigWeb;
using BJ.Contract.ConfigWeb.UpdateConfigWeb;
using BJ.Contract.Translation.ConfigWeb;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class ConfigWebMappingProfile : Profile
    {
        public ConfigWebMappingProfile()
        {
            CreateMap<ConfigWebsite, ConfigWebDto>().ForPath(dest => dest.DetailConfigWebDto, opt => opt.MapFrom(src => src.DetailConfigWeb));

            CreateMap<DetailConfigWebsite, DetailConfigWebDto>();
            CreateMap<DetailConfigWebsiteTranslation, DetailConfigWebTranslationDto>();


            CreateMap<CreateConfigWebDto, ConfigWebsite>();
            CreateMap<CreateDetailConfigWebDto, DetailConfigWebsite>();
            CreateMap<CreateDetailConfigWebTranslationDto, DetailConfigWebsiteTranslation>();


            CreateMap<UpdateConfigWebDto, ConfigWebsite>();
            CreateMap<UpdateDetailConfigWebDto, DetailConfigWebsite>();
            CreateMap<UpdateDetailConfigWebTranslationDto, DetailConfigWebsiteTranslation>();


        }
    }
}
