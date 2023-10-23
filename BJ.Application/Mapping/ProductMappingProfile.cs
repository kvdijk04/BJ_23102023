using AutoMapper;
using BJ.Contract.Product;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, UserProductDto>().ForMember(dest => dest.CatName, opt => opt.MapFrom(src => src.Category.CatName))
                .ForPath(dest => dest.UserSubCategorySpecificProductDto, opt => opt.MapFrom(src => src.SubCategorySpecificProducts));
            CreateMap<Product, ProductDto>().ForPath(dest => dest.CategoryDto, opt => opt.MapFrom(src => src.Category))
                                            .ForPath(dest => dest.SizeSpecificProducts, opt => opt.MapFrom(src => src.SizeSpecificProducts))
                                            .ForPath(dest => dest.SubCategorySpecificProductDtos, opt => opt.MapFrom(src => src.SubCategorySpecificProducts)); CreateMap<Product, ViewAllProduct>();

            CreateMap<CreateProductDto, Product>();


            CreateMap<UpdateProductDto, Product>();

            //CreateMap<ProductImage, ProductImageDto>();

            //CreateMap<CreateProductImage, ProductImage>();


            //CreateMap<UpdateProductImage, ProductImage>();
        }
    }
}
