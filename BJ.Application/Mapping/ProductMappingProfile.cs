using AutoMapper;
using BJ.Contract.Product;
using BJ.Contract.Translation.Product;
using BJ.Domain.Entities;

namespace BJ.Application.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, UserProductDto>()/*.ForMember(dest => dest.CatName, opt => opt.MapFrom(src => src.Category.CatName))*/
                .ForPath(dest => dest.UserSubCategorySpecificProductDto, opt => opt.MapFrom(src => src.SubCategorySpecificProducts));
            CreateMap<Product, ProductDto>()/*.ForPath(dest => dest.CategoryDto, opt => opt.MapFrom(src => src.Category))*/;
            CreateMap<Product, ViewAllProduct>();

            CreateMap<CreateProductDto, Product>();


            CreateMap<UpdateProductDto, Product>();

            CreateMap<ProductTranslation, ProductTranslationDto>().ForPath(dest => dest.ProductDto, opt => opt.MapFrom(src => src.Product));

            CreateMap<CreateProductTranslationDto, ProductTranslation>();


            CreateMap<UpdateProductTranslationDto, ProductTranslation>();
            //CreateMap<ProductImage, ProductImageDto>();

            //CreateMap<CreateProductImage, ProductImage>();


            //CreateMap<UpdateProductImage, ProductImage>();
        }
    }
}
