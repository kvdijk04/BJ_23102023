using AutoMapper;
using BJ.Contract.News;
using BJ.Contract.Translation.News;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class NewsMappingProfile : Profile
    {
        public NewsMappingProfile()
        {
            CreateMap<News, NewsDto>().ForPath(dest => dest.NewsTranslationsDto, opt => opt.MapFrom(src => src.NewsTranslations));


            CreateMap<CreateNewsDto, News>();


            CreateMap<UpdateNewsDto, News>();

            CreateMap<NewsTranslation, NewsTranslationDto>().ForPath(dest => dest.NewsDto, opt => opt.MapFrom(src => src.News));

            CreateMap<CreateNewsTranslationDto, NewsTranslation>();


            CreateMap<UpdateNewsTranslationDto, NewsTranslation>();

        }
    }
}
