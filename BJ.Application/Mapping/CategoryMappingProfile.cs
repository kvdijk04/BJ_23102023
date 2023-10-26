using AutoMapper;
using BJ.Contract.Category;
using BJ.Contract.Translation.Category;
using BJ.Contract.Translation.SubCategory;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryDto>().ForPath(dest => dest.ProductDtos, opt => opt.MapFrom(src => src.Products)).ForPath(dest => dest.CategoryTranslationDtos, opt => opt.MapFrom(src => src.CategoryTranslations));
            CreateMap<Category, UserCategoryDto>().ForPath(dest => dest.UserProductDtos, opt => opt.MapFrom(src => src.Products))
                                                .ForPath(dest => dest.CategoryTranslationDtos, opt => opt.MapFrom(src => src.CategoryTranslations));

            CreateMap<CreateCategoryDto, Category>();


            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<CategoryTranslation, CategoryTranslationDto>().ForPath(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<CreateCategoryTranslationDto, CategoryTranslation>();


            CreateMap<UpdateCategoryTranslationDto, CategoryTranslation>();


            CreateMap<SubCategoryTranslation, SubCategoryTranslationDto>().ForPath(dest => dest.SubCategoryDto, opt => opt.MapFrom(src => src.SubCategory));

            CreateMap<CreateSubCategoryTranslationDto, SubCategoryTranslation>();


            CreateMap<UpdateSubCategoryTranslationDto, SubCategoryTranslation>();
        }
    }
}
