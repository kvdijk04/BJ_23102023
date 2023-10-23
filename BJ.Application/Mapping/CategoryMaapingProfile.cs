using AutoMapper;
using BJ.Contract.Category;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class CategoryMaapingProfile : Profile
    {
        public CategoryMaapingProfile()
        {
            CreateMap<Category, CategoryDto>().ForPath(dest => dest.ProductDtos, opt => opt.MapFrom(src => src.Products));
            CreateMap<Category, UserCategoryDto>().ForPath(dest => dest.UserProductDtos, opt => opt.MapFrom(src => src.Products));

            CreateMap<CreateCategoryDto, Category>();


            CreateMap<UpdateCategoryDto, Category>();


        }
    }
}
