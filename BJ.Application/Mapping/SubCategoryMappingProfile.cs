using AutoMapper;
using BJ.Contract.SubCategory;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class SubCategoryMappingProfile : Profile
    {
        public SubCategoryMappingProfile()
        {
            CreateMap<SubCategory, SubCategoryDto>().ForPath(dest => dest.SubCategorySpecificProductDtos, opt => opt.MapFrom(src => src.SubCategorySpecificProducts))
                .ForPath(dest => dest.SubCategoryTranslationDtos, opt => opt.MapFrom(src => src.SubCategoryTranslations));
            CreateMap<CreateSubCategoryDto, SubCategory>();


            CreateMap<UpdateSubCategoryDto, SubCategory>();

            CreateMap<SubCategorySpecificProduct, SubCategorySpecificProductDto>().ForPath(dest => dest.ProductDto, opt => opt.MapFrom(src => src.Product))
                .ForPath(dest => dest.SubCategoryDto, opt => opt.MapFrom(src => src.SubCategory));/*.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Size.Price))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.Name))
                .ForPath(dest => dest.ProductDto, opt => opt.MapFrom(src => src.Product))*/
            ;
            CreateMap<SubCategorySpecificProduct, UpdateSubCategorySpecificProduct>();
            CreateMap<SubCategorySpecificProduct, UserSubCategorySpecificProductDto>().ForMember(dest => dest.ActiveProduct, opt => opt.MapFrom(src => src.Active))
                .ForMember(dest => dest.SubActive, opt => opt.MapFrom(src => src.SubCategory.Active))
                  .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.SubCategory.ImagePath))
                  .ForPath(dest => dest.SubCategoryDto, opt => opt.MapFrom(src => src.SubCategory));

            CreateMap<CreateSubCategorySpecificDto, SubCategorySpecificProduct>();


            CreateMap<UpdateSubCategorySpecificProduct, SubCategorySpecificProduct>();



        }
    }
}
