using AutoMapper;
using BJ.Contract.Translation;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class LanguageMappingProfile : Profile
    {
        public LanguageMappingProfile()
        {
            CreateMap<Language, LanguageDto>().ForPath(dest => dest.CategoryTranslationDtos, opt => opt.MapFrom(src => src.CategoryTranslations))
                .ForPath(dest => dest.SubCategoryTranslationDtos, opt => opt.MapFrom(src => src.SubCategoryTranslations))
                .ForPath(dest => dest.ProductTranslationDtos, opt => opt.MapFrom(src => src.ProductTranslations));

            CreateMap<CreateLanguageDto, Language>();


            CreateMap<UpdateLanguageDto, Language>();


        }
    }
}
